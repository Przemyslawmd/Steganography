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
    }
}
