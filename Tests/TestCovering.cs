﻿
using Steganography;
using Steganography.Cryptography;
using Steganography.Huffman;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestClass]
    public class TestCovering
    {
        [ClassInitialize]
        public static void Initialize( TestContext context )
        {          
            projectPath = Directory.GetParent( Directory.GetCurrentDirectory() ).Parent.FullName;
            bitmapPath = Path.Combine( projectPath, "Resources\\graphicToTest.jpg" );
            filePath = Path.Combine( projectPath, "Resources\\fileToTest.txt" );
            referenceLongData = new List< byte >( File.ReadAllBytes( filePath ) );
            referenceShortData = new List< byte >( Encoding.Unicode.GetBytes( "This text is to be hidden" ) );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void PrepareData( bool isCompression )
        {          
            emptyBitmap = new Bitmap( 50, 50 );
            colorBitmap = new Bitmap( bitmapPath );
            longData = new List< byte >( referenceLongData );
            shortData = new List< byte >( referenceShortData );
            compression = isCompression;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestCoveringInEmptyBitmapByCheckingPixels()
        {            
            PrepareData( false );
            new Covering().CoverData( emptyBitmap, shortData, false );
            
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
        public void TestCoveringInEmptyBitmap()
        {
            PrepareData( false );
            new Covering().CoverData( emptyBitmap, shortData, false );
            List< byte > data = new Uncovering().UncoverData( emptyBitmap, ref compression, ref code );

            CollectionAssert.AreEqual( data, referenceShortData );            
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestCoveringInColorBitmap()
        {
            PrepareData( false );
            new Covering().CoverData( colorBitmap, longData, compression );
            List< byte > unCoveredData = new Uncovering().UncoverData( colorBitmap, ref compression, ref code );
                        
            CollectionAssert.AreEqual( unCoveredData, referenceLongData );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestCoveringInColorBitmapWithCompression()
        {
            PrepareData( true );
            List< byte > compressedData = new Compression().MakeCompressedStream( longData );
            new Covering().CoverData( colorBitmap, compressedData, compression );
            List< byte > uncoveredData = new Uncovering().UncoverData( colorBitmap, ref compression, ref code );
            List< byte > decompressedData = new Decompression().Decompress( uncoveredData, ref code );

            CollectionAssert.AreEqual( decompressedData, referenceLongData );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestCoveringInColorBitmapWithEncryption()
        {
            PrepareData( false );
            List< byte > encryptedData = new Encryption().Encrypt( longData, password );
            new Covering().CoverData( colorBitmap, encryptedData, false );
            List< byte > uncoveredData = new Uncovering().UncoverData( colorBitmap, ref compression, ref code );
            List< byte > decryptedData = new Decryption().Decrypt( uncoveredData, password, ref code );

            CollectionAssert.AreEqual( decryptedData, referenceLongData );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestCoveringInColorBitmapWithCompressionAndEncryption()
        {
            PrepareData( true );
            List< byte > encryptedData = new Encryption().Encrypt( longData, password );
            List< byte > compressedData = new Compression().MakeCompressedStream( encryptedData );
            new Covering().CoverData( colorBitmap, compressedData, compression );
            List< byte > uncoveredData = new Uncovering().UncoverData( colorBitmap, ref compression, ref code );
            List< byte > decompressedData = new Decompression().Decompress( uncoveredData, ref code );
            List< byte > decryptedData = new Decryption().Decrypt( decompressedData, password, ref code );

            CollectionAssert.AreEqual( decryptedData, referenceLongData );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        static String projectPath;
        static String bitmapPath;
        static String filePath;
        static List< byte > referenceShortData;
        static List< byte > referenceLongData;
        static string password = "de3@JH^@";
        static Result code = Result.OK;

        Bitmap emptyBitmap;
        Bitmap colorBitmap;
        List< byte > shortData;
        List< byte > longData;
        bool compression;
    }
}

