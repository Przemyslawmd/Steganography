using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Compression;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class TestCompression
    {
        /********************************************************************************************/
        /* TEST CREATING NODES USED TO BUILD HUFFMAN TREE *******************************************/
        [TestMethod]
        public void TestOrderedNodesForHuffmanTree()
        {
            PrivateObject obj = new PrivateObject( new Compress() );

            byte[] data = new byte[10] { 0x12, 0xAA, 0xCA, 0xCA, 0xDA, 0x10, 0x00, 0x00, 0x12, 0x34 };

            List<NodeCompress> nodes = ( List<NodeCompress> )( obj.Invoke( "CreateNodes", data ));
            nodes = nodes.OrderBy( x => x.Count ).ToList();

            Assert.AreEqual( nodes.Count, 7 );
            Assert.AreEqual( nodes[0].ByteValue, 0x10 );
            Assert.AreEqual( nodes[3].ByteValue, 0xDA );
            Assert.AreEqual( nodes[5].ByteValue, 0x12 );
        }

        /*********************************************************************************************/
        /* TEST BUILDING HUFFMAN TREE ****************************************************************/
        [TestMethod]
        public void TestBuildingHuffmanTree()
        {
            PrivateObject obj = new PrivateObject( new Compress() );
            
            // Create a list of NodeCompress objects sorted by count member class 
            List<NodeCompress> nodes = new List<NodeCompress>();
            nodes.Add( new NodeCompress( 1, 0x10 ));
            nodes.Add( new NodeCompress( 1, 0x11 ));
            nodes.Add( new NodeCompress( 2, 0x12 ));
            nodes.Add( new NodeCompress( 2, 0x13 ));
            nodes.Add( new NodeCompress( 3, 0x14 ));

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
    }
}
