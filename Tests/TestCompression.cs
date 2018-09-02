
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Steganography;
using SteganographyCompression;

namespace Tests
{
    [TestClass]
    public class TestCompression
    {
        [TestMethod]
        public void TestCompressionMain()
        {                       
            string projectPath = Directory.GetParent( Directory.GetCurrentDirectory() ).Parent.FullName;
            string filePath = Path.Combine( projectPath, "Resources\\fileToTestCompression.txt" );
            List< byte > dataToCompress = new List< byte >( File.ReadAllBytes( filePath ) );
            List< byte > dataCompressed = new Compression().Compress( dataToCompress );

            CollectionAssert.AreNotEqual( dataToCompress, dataCompressed );
            Assert.IsTrue( dataCompressed.Count < dataToCompress.Count );
            
            List< byte > dataDecompressed = new Decompression().Decompress( dataCompressed );
            CollectionAssert.AreEqual( dataToCompress, dataDecompressed );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestCompressionCreatingNodes()
        {
            PrivateObject obj = new PrivateObject( new HuffmanTree() );
            List< byte > data = new List< byte > { 0x12, 0xAA, 0xCA, 0xCA, 0xDA, 0x10, 0x00, 0x00, 0x12, 0x34 };

            nodes = ( List<NodeCompress> )( obj.Invoke( "CreateNodes", data ));
            nodes = nodes.OrderBy( x => x.Count ).ToList();

            Assert.AreEqual( nodes.Count, 7 );
            Assert.AreEqual( nodes[0].ByteValue, 0x10 );
            Assert.AreEqual( nodes[3].ByteValue, 0xDA );
            Assert.AreEqual( nodes[5].ByteValue, 0x12 );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestCompressionBuildingTree()
        {
            PrivateObject obj = new PrivateObject( new HuffmanTree() );

            // Create a list of NodeCompress objects sorted by count
            nodes = new List< NodeCompress >();
            nodes.Add( new NodeCompress( 1, 0x10 ) );
            nodes.Add( new NodeCompress( 1, 0x11 ) );
            nodes.Add( new NodeCompress( 2, 0x12 ) );
            nodes.Add( new NodeCompress( 2, 0x13 ) );
            nodes.Add( new NodeCompress( 3, 0x14 ) );

            obj.Invoke( "BuildTree", nodes );

            NodeCompress root = nodes[0];

            Node node = root.Left.Left;
            Assert.AreEqual( node.ByteValue, 0x12 );
            Assert.AreEqual( ((NodeCompress) node).Count, 2 );

            node = root.Right.Left;
            Assert.AreEqual( node.isLeaf(), false );
            Assert.AreEqual( ((NodeCompress) node).Count, 2 );

            node = root.Right.Left.Right;
            Assert.AreEqual( node.isLeaf(), true );
            Assert.AreEqual( ((NodeCompress) node).Count, 1 );
            Assert.AreEqual( node.ByteValue, 0x11 );
        }

        /**************************************************************************************/
        /**************************************************************************************/
        // Dependend on the TestCompressionBuildingTree

        /*
        [TestMethod]
        public void TestCompressionGeneratingCodes()
        {
            NodeCompress root = nodes[0];
            Dictionary< byte, List< char >> codes = new HuffmanCodeGenerator().CreateCodesDictionary( root );
            List< char > code;

            codes.TryGetValue( 0x12, out code );
            CollectionAssert.AreEqual( code, new List< char > { '1', '0', '0' } );
            codes.TryGetValue( 0x13, out code );
            CollectionAssert.AreEqual( code, new List< char > { '1', '0', '1' } );
            codes.TryGetValue( 0x14, out code );
            CollectionAssert.AreNotEqual( code, new List< char > { '1', '0', '0' } );
            codes.TryGetValue( 0x11, out code );
            CollectionAssert.AreEqual( code, new List< char > { '1', '1', '0', '1' } );
            codes.TryGetValue( 0x10, out code );
            CollectionAssert.AreEqual( code, new List< char > { '1', '1', '0', '0' } );
        }
        */
        
        [TestMethod]
        public void TestCompressionGeneratingCodes()
        {
            TestCompressionBuildingTree();

            NodeCompress root = nodes[0];
            List< HuffmanCode > codes = new HuffmanCodeGenerator().CreateCodesDictionary( root );

            HuffmanCode code;
            code = FindCodeBySymbol( codes, 0x12 );
            CollectionAssert.AreEqual( code.tokens, new Stack< byte >( new byte[] { 0x80 } ));
            Assert.AreEqual( code.length, 3 );

            code = FindCodeBySymbol( codes, 0x13 );
            CollectionAssert.AreEqual( code.tokens, new Stack< byte >( new byte[] { 0xA0 } ));
            Assert.AreEqual( code.length, 3 );

            code = FindCodeBySymbol( codes, 0x11 );
            CollectionAssert.AreEqual( code.tokens, new Stack< byte >( new byte[] { 0xD0 } ));
            Assert.AreEqual( code.length, 4 );

            code = FindCodeBySymbol( codes, 0x10 );
            CollectionAssert.AreEqual( code.tokens, new Stack< byte >( new byte[] { 0xC0 } ));
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private HuffmanCode FindCodeBySymbol( List< HuffmanCode > codes, byte symbol )
        {
            foreach ( HuffmanCode code in codes )
            {
                if ( code.symbol == symbol )
                {
                    return code;
                }
            }

            return null;
        }

        static List< NodeCompress > nodes;
    }
}
