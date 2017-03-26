using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Compression;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Tests
{
    [TestClass]
    public class TestCompression
    {
        /********************************************************************************************/
        /* MAIN TEST FOR COMPRESSION ****************************************************************/

        [TestMethod]
        public void TestMainForCompression()
        {                       
            string projectPath = Directory.GetParent( Directory.GetCurrentDirectory() ).Parent.FullName;
            string filePath = Path.Combine( projectPath, "Resources\\fileToTestCompression.txt" );
            byte[] data = File.ReadAllBytes( filePath );
                        
            byte[] dataCompressed = new Compress().CompressData( data );
            CollectionAssert.AreNotEqual( data, dataCompressed );
            Assert.IsTrue( dataCompressed.Length < data.Length );
            
            byte[] dataDecompressed = new Decompress().decompressData( dataCompressed );
            CollectionAssert.AreEqual( data, dataDecompressed );
        }

        /********************************************************************************************/
        /* TEST CREATING NODES USED TO BUILD HUFFMAN TREE *******************************************/

        [TestMethod]
        public void TestCompressionCreatingNodes()
        {
            PrivateObject obj = new PrivateObject( new HuffmanTree() );

            byte[] data = new byte[10] { 0x12, 0xAA, 0xCA, 0xCA, 0xDA, 0x10, 0x00, 0x00, 0x12, 0x34 };

            nodes = (List<NodeCompress>)(obj.Invoke( "CreateNodes", data ));
            nodes = nodes.OrderBy( x => x.Count ).ToList();

            Assert.AreEqual( nodes.Count, 7 );
            Assert.AreEqual( nodes[0].ByteValue, 0x10 );
            Assert.AreEqual( nodes[3].ByteValue, 0xDA );
            Assert.AreEqual( nodes[5].ByteValue, 0x12 );
        }

        /*********************************************************************************************/
        /* TEST BUILDING HUFFMAN TREE ****************************************************************/

        [TestMethod]
        public void TestCompressionBuildingTree()
        {
            PrivateObject obj = new PrivateObject( new HuffmanTree() );

            // Create a list of NodeCompress objects sorted by count member class 
            nodes = new List<NodeCompress>();
            nodes.Add( new NodeCompress( 1, 0x10 ) );
            nodes.Add( new NodeCompress( 1, 0x11 ) );
            nodes.Add( new NodeCompress( 2, 0x12 ) );
            nodes.Add( new NodeCompress( 2, 0x13 ) );
            nodes.Add( new NodeCompress( 3, 0x14 ) );

            obj.Invoke( "BuildTree", nodes );

            NodeCompress root = nodes[0];

            NodeCompress node = root.Left.Left;
            Assert.AreEqual( node.ByteValue, 0x12 );
            Assert.AreEqual( node.Count, 2 );

            node = root.Right.Left;
            Assert.AreEqual( node.Leaf, false );
            Assert.AreEqual( node.Count, 2 );

            node = root.Right.Left.Right;
            Assert.AreEqual( node.Leaf, true );
            Assert.AreEqual( node.Count, 1 );
            Assert.AreEqual( node.ByteValue, 0x11 );
        }

        /*********************************************************************************************/
        /* TEST CREATING HUFFMAN CODES ***************************************************************/

        [TestMethod]
        public void TestCompressionGeneratingCodes()
        {
            NodeCompress root = nodes[0];
            Dictionary<byte, String> codes = new HuffmanCodes().CreateCodesDictionary( root );
            String code;

            codes.TryGetValue( 0x12, out code );
            Assert.AreEqual( code, "100" );
            codes.TryGetValue( 0x13, out code );
            Assert.AreEqual( code, "101" );
            codes.TryGetValue( 0x14, out code );
            Assert.AreNotEqual( code, "100" );
            codes.TryGetValue( 0x11, out code );
            Assert.AreEqual( code, "1101" );
            codes.TryGetValue( 0x10, out code );
            Assert.AreEqual( code, "1100" );
        }
        
        /********************************************************************************************/
        /********************************************************************************************/

        static List<NodeCompress> nodes;
    }
}
