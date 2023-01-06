
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Steganography;
using Steganography.Huffman;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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
            var dataToCompress = new List< byte >( File.ReadAllBytes( filePath ) );
            List< byte > dataCompressed = new Compression().MakeCompressedStream( dataToCompress );

            CollectionAssert.AreNotEqual( dataToCompress, dataCompressed );
            Assert.IsTrue( dataCompressed.Count < dataToCompress.Count );

            Result result = Result.OK;
            List< byte > dataDecompressed = new Decompression().Decompress( dataCompressed, ref result );

            CollectionAssert.AreEqual( dataToCompress, dataDecompressed );
            Assert.AreEqual( result, Result.OK );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestCompressionShortTextWithoutDictionaryCodes()
        {
            var dataToCompress = new List< byte >( System.Text.Encoding.Unicode.GetBytes( "AxC2cc&422Avdfr" ));
            NodeCompress root = new HuffmanTree().BuildTreeCompression( dataToCompress );
            Dictionary< byte, List< bool >> codes = new HuffmanCodesGenerator().CreateCodesDictionary( root );

            PrivateObject objectCompression = new PrivateObject( new Compression() );

            var dataCompressed = ( List< byte > ) objectCompression.Invoke( "Compress", dataToCompress, codes );

            var expectedData = new List< byte >{ 0xD5, 0xFD, 0xD5, 0x96, 0xED, 0xDC, 0x5C, 
                                                 0xD9, 0x65, 0xAB, 0xEB, 0xBB, 0xCB, 0xD8 };

            CollectionAssert.AreEqual( dataCompressed, expectedData );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void CreateNodes()
        {
            PrivateObject obj = new PrivateObject( new HuffmanTree() );
            var data = new List< byte > { 
                0x65, 0x4f, 0x64, 0x4f, 0x4f, 0x72, 0x64, 0x72, 0x6c, 0x4f, 0x6f, 0x57, 0x4b, 0x72, 0x6f, 
                0x4b, 0x6c, 0x64, 0x65, 0x6c, 0x41, 0x4b, 0x72, 0x4b, 0x4b, 0x65, 0x6c, 0x4b, 0x4b, 0x4f, 
                0x4b,0x4f,  0x56, 0x56, 0x64, 0x4b, 0x41, 0x6c, 0x64, 0x4b, 0x6f, 0x4f, 0x4f };

            var nodes = ( List< NodeCompress > )obj.Invoke( "CreateNodes", data );

            Assert.AreEqual( nodes.Count, 10 );
            Assert.IsTrue( CheckNode( nodes[0], 0x41, 2 ));
            Assert.IsTrue( CheckNode( nodes[1], 0x4b, 10 ));
            Assert.IsTrue( CheckNode( nodes[2], 0x4f, 8 ));
            Assert.IsTrue( CheckNode( nodes[3], 0x56, 2 ));
            Assert.IsTrue( CheckNode( nodes[4], 0x57, 1 ));
            Assert.IsTrue( CheckNode( nodes[5], 0x64, 5 ));
            Assert.IsTrue( CheckNode( nodes[6], 0x65, 3 ));
            Assert.IsTrue( CheckNode( nodes[7], 0x6c, 5 ));
            Assert.IsTrue( CheckNode( nodes[8], 0x6f, 3 ));
            Assert.IsTrue( CheckNode( nodes[9], 0x72, 4 ));
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestCompressionBuildingTree()
        {
            PrivateObject obj = new PrivateObject( new HuffmanTree() );

            List< NodeCompress > nodes = new List< NodeCompress >
            {
                new NodeCompress( 1, 0x10 ),
                new NodeCompress( 1, 0x11 ),
                new NodeCompress( 2, 0x12 ),
                new NodeCompress( 2, 0x13 ),
                new NodeCompress( 3, 0x14 )
            };

            obj.Invoke( "BuildTree", nodes );

            NodeCompress root = nodes[0];

            Node node = root.Left.Left;
            Assert.AreEqual( node.ByteValue, 0x12 );
            Assert.AreEqual( ((NodeCompress) node).Count, 2 );

            node = root.Right.Left;
            Assert.AreEqual( node.IsLeaf(), false );
            Assert.AreEqual( ((NodeCompress) node).Count, 2 );

            node = root.Right.Left.Right;
            Assert.AreEqual( node.IsLeaf(), true );
            Assert.AreEqual( ((NodeCompress) node).Count, 1 );
            Assert.AreEqual( node.ByteValue, 0x11 );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestCompressionGeneratingCodes()
        {
            PrivateObject obj = new PrivateObject( new HuffmanTree() );

            List< NodeCompress > nodes = new List< NodeCompress >
            {
                new NodeCompress( 1, 0x10 ),
                new NodeCompress( 1, 0x11 ),
                new NodeCompress( 2, 0x12 ),
                new NodeCompress( 2, 0x13 ),
                new NodeCompress( 3, 0x14 )
            };

            obj.Invoke( "BuildTree", nodes );

            NodeCompress root = nodes[0];
            Dictionary< byte, List< bool >> codesDictionary = new HuffmanCodesGenerator().CreateCodesDictionary( root );
            List< bool > code;

            codesDictionary.TryGetValue( 0x12, out code );
            CollectionAssert.AreEqual( code, new List< bool > { true, false, false } );
            codesDictionary.TryGetValue( 0x13, out code );
            CollectionAssert.AreEqual( code, new List< bool > { true, false, true } );
            codesDictionary.TryGetValue( 0x14, out code );
            CollectionAssert.AreNotEqual( code, new List< bool > { true, false, false } );
            codesDictionary.TryGetValue( 0x11, out code );
            CollectionAssert.AreEqual( code, new List< bool > { true, true, false, true } );
            codesDictionary.TryGetValue( 0x10, out code );
            CollectionAssert.AreEqual( code, new List< bool > { true, true, false, false } );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private bool CheckNode( NodeCompress node, byte value, int count )
        {
            return node.ByteValue == value && node.Count == count;
        }
    }
}

