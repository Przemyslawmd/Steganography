using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using Steganography;
using Compression;
using System.IO;

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
        }

        /***********************************************************************************************/
        /* PREPARE DATA ********************************************************************************/

        private void PrepareData()
        {          
            emptyBitmap = new Bitmap( 50, 50 );
            colorBitmap = new Bitmap( bitmapPath );
            shortData = "This text is to be hidden";
            fullData = File.ReadAllBytes( filePath );
        }        
         
        /***********************************************************************************************/
        /* TEST COVERING SHORT DATA IN EMPTY BITMAP BY CHECKING PIXEL VALUES ***************************/
        [TestMethod]
        public void TestCoveringInEmptyBitmapByCheckingPixels()
        {            
            PrepareData();
            byte[] data = new byte[shortData.Length * sizeof( char )];
            System.Buffer.BlockCopy( shortData.ToCharArray(), 0, data, 0, data.Length );          

            new Covering().CoverData( emptyBitmap, data, false );
            
            // Test size of data in first six pixels
            Assert.AreEqual( emptyBitmap.GetPixel( 0, 0 ).R % 2, 0 );
            Assert.AreEqual( emptyBitmap.GetPixel( 0, 0 ).G % 2, 0 );
            Assert.AreEqual( emptyBitmap.GetPixel( 4, 0 ).G % 2, 1 );
            Assert.AreEqual( emptyBitmap.GetPixel( 5, 0 ).G % 2, 1 );
            Assert.AreEqual( emptyBitmap.GetPixel( 5, 0 ).B % 2, 0 );

            // Test compression flag in pixel number six
            Assert.AreEqual( emptyBitmap.GetPixel( 6, 0 ).R % 2, 0 );

            // Test hidden data
            Assert.AreEqual( emptyBitmap.GetPixel( 0, 1 ).B % 2, 0 );
            Assert.AreEqual( emptyBitmap.GetPixel( 1, 1 ).R % 2, 1 );
            Assert.AreEqual( emptyBitmap.GetPixel( 9, 2 ).R % 2, 1 );
            Assert.AreEqual( emptyBitmap.GetPixel( 9, 2 ).B % 2, 1 );
            Assert.AreEqual( emptyBitmap.GetPixel( 2, 3 ).B % 2, 1 );
            Assert.AreEqual( emptyBitmap.GetPixel( 3, 3 ).G % 2, 0 );            
        }

        /*****************************************************************************************************/
        /* TEST COVERING SHORT DATA IN EMPTY BITMAP **********************************************************/
        [TestMethod]
        public void TestCoveringInEmptyBitmap()
        {
            PrepareData();
            byte[] data = new byte[shortData.Length * sizeof( char )];
            System.Buffer.BlockCopy( shortData.ToCharArray(), 0, data, 0, data.Length );

            new Covering().CoverData( emptyBitmap, data, false );
            Boolean compression = false;
            data = new Uncovering().UncoverData( emptyBitmap, ref compression );
            String uncoveredText = System.Text.Encoding.Unicode.GetString( data );

            Assert.AreNotEqual( uncoveredText, "This text is to be hidden_" );
            Assert.AreEqual( uncoveredText, shortData );            
        }

        /*******************************************************************************************************/
        /* TEST COVERING ***************************************************************************************/

        [TestMethod]
        public void TestCoveringInColorBitmap()
        {
            PrepareData();
            bool compression = false;                        
            new Covering().CoverData( colorBitmap, fullData, compression );
            byte[] unCoveredData = new Uncovering().UncoverData( colorBitmap, ref compression );
                        
            CollectionAssert.AreEqual( unCoveredData, fullData );
        }

        /*******************************************************************************************************/
        /* TEST COVERING WITH COMPRESSION **********************************************************************/

        [TestMethod]
        public void TestCoveringInColorBitmapWithCompression()
        {
            PrepareData();
            bool compression = true;
            byte[] dataCopy = new byte[fullData.Length];
            Array.Copy( fullData, dataCopy, fullData.Length );

            byte[] compressedData = new Compress().CompressData( dataCopy );
            new Covering().CoverData( colorBitmap, compressedData, compression );
            byte[] uncoveredData = new Uncovering().UncoverData( colorBitmap, ref compression );
            byte[] decompressedData = new Decompress().decompressData( uncoveredData );

            CollectionAssert.AreEqual( decompressedData, fullData );
        }

        /********************************************************************************************************/
        /********************************************************************************************************/

        static String projectPath;
        static String bitmapPath;
        static String filePath;

        Bitmap emptyBitmap;
        Bitmap colorBitmap;
        String shortData;
        byte[] fullData;
    }
}
