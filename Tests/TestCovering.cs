
using Steganography;
using Steganography.Cryptography;
using Steganography.Huffman;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestClass]
    public class TestCovering
    {
        public void Initialize( bool isCompression )
        {
            string projectPath = Directory.GetParent( Directory.GetCurrentDirectory() ).Parent.FullName;
            string filePath = Path.Combine( projectPath, "Resources\\fileToTest.txt" );
            string bitmapPath = Path.Combine( projectPath, "Resources\\graphicToTest.jpg" );
            bitmap = new Bitmap( bitmapPath );

            referenceShortData = new List< byte >( Encoding.Unicode.GetBytes( "This text is to be hidden" ));
            referenceLongData = new List< byte >( File.ReadAllBytes(filePath) );
            compression = isCompression;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void CoverDataInEmptyBitmapPartialCheck()
        {
            Initialize( false );
            var dataToCover = new List< byte >( referenceShortData );
            Bitmap emptyBitmap = new Bitmap( 50, 50 );
            new Covering().CoverData( emptyBitmap, dataToCover, compression );
            
            // Test size of data covever in first eight pixels
            Assert.AreEqual( emptyBitmap.GetPixel( 0, 1 ).R % 2, 0 );
            Assert.AreEqual( emptyBitmap.GetPixel( 0, 1 ).G % 2, 0 );
            Assert.AreEqual( emptyBitmap.GetPixel( 6, 1 ).G % 2, 1 );
            Assert.AreEqual( emptyBitmap.GetPixel( 7, 1 ).G % 2, 1 );
            Assert.AreEqual( emptyBitmap.GetPixel( 7, 1 ).B % 2, 0 );

            // Test compression flag in a nineth pixel
            Assert.AreEqual( emptyBitmap.GetPixel( 8, 1 ).R % 2, 0 );

            // Test hidden data
            Assert.AreEqual( emptyBitmap.GetPixel( 0, 2 ).B % 2, 0 );
            Assert.AreEqual( emptyBitmap.GetPixel( 1, 2 ).R % 2, 1 );
            Assert.AreEqual( emptyBitmap.GetPixel( 9, 3 ).R % 2, 1 );
            Assert.AreEqual( emptyBitmap.GetPixel( 9, 3 ).B % 2, 1 );
            Assert.AreEqual( emptyBitmap.GetPixel( 2, 4 ).B % 2, 1 );
            Assert.AreEqual( emptyBitmap.GetPixel( 3, 4 ).G % 2, 0 );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void CoverDataInEmptyBitmap()
        {
            Initialize( false );
            var dataToCover = new List< byte >( referenceShortData );
            Bitmap emptyBitmap = new Bitmap( 50, 50 );
            new Covering().CoverData( emptyBitmap, dataToCover, compression );
            var uncoveredData = new Uncovering().UncoverData( emptyBitmap, ref compression, ref code );

            CollectionAssert.AreEqual( uncoveredData, referenceShortData );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void CoverData()
        {
            Initialize( false );
            var dataToCover = new List< byte >( referenceLongData );
            new Covering().CoverData( bitmap, dataToCover, compression );
            var uncoveredData = new Uncovering().UncoverData( bitmap, ref compression, ref code );
                        
            CollectionAssert.AreEqual( uncoveredData, referenceLongData );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void CoverCompressedData()
        {
            Initialize( true );
            var dataToCover = new List< byte >( referenceLongData );
            var compressedData = new Compression().MakeCompressedStream( dataToCover );
            new Covering().CoverData( bitmap, compressedData, compression );
            var uncoveredData = new Uncovering().UncoverData( bitmap, ref compression, ref code );
            var decompressedData = new Decompression().Decompress( uncoveredData, ref code );

            CollectionAssert.AreEqual( decompressedData, referenceLongData );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void CoverEncryptedData()
        {
            Initialize( false );
            var dataToCover = new List< byte >( referenceLongData );
            var encryptedData = new Encryption().Encrypt( dataToCover, password );
            new Covering().CoverData( bitmap, encryptedData, compression );
            var uncoveredData = new Uncovering().UncoverData( bitmap, ref compression, ref code );
            var decryptedData = new Decryption().Decrypt( uncoveredData, password, ref code );

            CollectionAssert.AreEqual( decryptedData, referenceLongData );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void CoverCompressedAndEncryptedData()
        {
            Initialize( true );
            var dataToCover = new List< byte >( referenceLongData );
            var encryptedData = new Encryption().Encrypt( dataToCover, password );
            var compressedData = new Compression().MakeCompressedStream( encryptedData );
            new Covering().CoverData( bitmap, compressedData, compression );
            var uncoveredData = new Uncovering().UncoverData( bitmap, ref compression, ref code );
            var decompressedData = new Decompression().Decompress( uncoveredData, ref code );
            var decryptedData = new Decryption().Decrypt( decompressedData, password, ref code );

            CollectionAssert.AreEqual( decryptedData, referenceLongData );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        List< byte > referenceShortData;
        List< byte > referenceLongData;
        readonly string password = "de3@JH^@";
        Result code = Result.OK;

        Bitmap bitmap;
        bool compression;
    }
}

