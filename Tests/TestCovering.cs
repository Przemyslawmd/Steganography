using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using Stegan;

namespace Tests
{
    [TestClass]
    public class TestSteganography
    {
        /***********************************************************************************************/
        /* TEST COVERING DATA **************************************************************************/
        [TestMethod]
        public void TestSteganCovering()
        {
            bitmap = new Bitmap( 50, 50 );
            String text = "This text is to be hidden";
            byte[] data = new byte[text.Length * sizeof( char )];
            System.Buffer.BlockCopy( text.ToCharArray(), 0, data, 0, data.Length );          

            new Covering().CoverData( bitmap, data, false );
            
            // Test size of data in first six pixels
            Assert.AreEqual( bitmap.GetPixel( 0, 0 ).R % 2, 0 );
            Assert.AreEqual( bitmap.GetPixel( 0, 0 ).G % 2, 0 );
            Assert.AreEqual( bitmap.GetPixel( 4, 0 ).G % 2, 1 );
            Assert.AreEqual( bitmap.GetPixel( 5, 0 ).G % 2, 1 );
            Assert.AreEqual( bitmap.GetPixel( 5, 0 ).B % 2, 0 );

            // Test compression flag in pixel number six
            Assert.AreEqual( bitmap.GetPixel( 6, 0 ).R % 2, 0 );

            // Test hidden data
            Assert.AreEqual( bitmap.GetPixel( 0, 1 ).B % 2, 0 );
            Assert.AreEqual( bitmap.GetPixel( 1, 1 ).R % 2, 1 );
            Assert.AreEqual( bitmap.GetPixel( 9, 2 ).R % 2, 1 );
            Assert.AreEqual( bitmap.GetPixel( 9, 2 ).B % 2, 1 );
            Assert.AreEqual( bitmap.GetPixel( 2, 3 ).B % 2, 1 );
            Assert.AreEqual( bitmap.GetPixel( 3, 3 ).G % 2, 0 );            
        }

        /*****************************************************************************************************/
        /* TEST UNCOVERING DATA ******************************************************************************/
        [TestMethod]
        public void TestSteganUncovering()
        {
            Boolean compressionFlag = false;
            byte[] data = new Uncovering().UncoverData( bitmap, ref compressionFlag );
            String text = System.Text.Encoding.Unicode.GetString( data );

            Assert.AreNotEqual( text, "This text is to be hidden_" );
            Assert.AreEqual( text, "This text is to be hidden" );
        }

        /*******************************************************************************************************/
        /*******************************************************************************************************/

        static Bitmap bitmap;
    }
}
