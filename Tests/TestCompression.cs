
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Steganography;
using Steganography.Huffman;
using System.Collections.Generic;
using System.IO;

namespace Tests
{
    [TestClass]
    public class TestCompression
    {
        [TestMethod]
        public void FullCompression()
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
        public void CompareRawCompressedStream()
        {
            var dataToCompress = new List< byte >( System.Text.Encoding.Unicode.GetBytes( "AxC2cc&422Avdfr" ));
            Node root = new HuffmanTree().BuildTreeCompression( dataToCompress );
            Dictionary< byte, List< bool >> codes = new HuffmanCodesGenerator().CreateCodesDictionary( root );

            PrivateObject objectCompression = new PrivateObject( new Compression() );
            var dataCompressed = ( List< byte > ) objectCompression.Invoke( "Compress", dataToCompress, codes );
            var expectedData = new List< byte >{ 0x2b, 0x68, 0x89, 0xce, 0xaa, 0xe2, 0x25, 0x65, 0x37, 0x5f };

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

            var nodes = ( List< Node > )obj.Invoke( "CreateNodes", data );

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
        public void BuildCompressionTree()
        {
            PrivateObject obj = new PrivateObject( new HuffmanTree() );

            var nodes = new List< Node >
            {
                new Node( 1, 0x57 ),
                new Node( 2, 0x41 ),
                new Node( 2, 0x56 ),
                new Node( 3, 0x65 ),
                new Node( 3, 0x6f ),
                new Node( 4, 0x72 ),
                new Node( 5, 0x64 ),
                new Node( 5, 0x6c ),
                new Node( 8, 0x4f ),
                new Node( 10, 0x4b )

            };

            obj.Invoke( "BuildTree", nodes );
            Node root = nodes[0];

            Assert.IsNotNull( root.Left.Left.Left );
            Assert.IsTrue( CheckLeaf( root.Left.Left.Left, 0x72, 4 ));

            Assert.IsNotNull( root.Left.Left.Right.Left );
            Assert.IsTrue( CheckLeaf( root.Left.Left.Right.Left, 0x56, 2 ));

            Assert.IsNotNull( root.Left.Left.Right.Right.Left );
            Assert.IsTrue( CheckLeaf( root.Left.Left.Right.Right.Left, 0x57, 1 ));

            Assert.IsNotNull( root.Left.Left.Right.Right.Right );
            Assert.IsTrue( CheckLeaf( root.Left.Left.Right.Right.Right, 0x41, 2 ));

            Assert.IsNotNull( root.Left.Right.Left );
            Assert.IsTrue( CheckLeaf( root.Left.Right.Left, 0x64, 5 ));

            Assert.IsNotNull( root.Left.Right.Right );
            Assert.IsTrue( CheckLeaf( root.Left.Right.Right, 0x6c, 5 ));

            Assert.IsNotNull( root.Right.Left );
            Assert.IsTrue( CheckLeaf( root.Right.Left, 0x4b, 10 ));

            Assert.IsNotNull( root.Right.Right.Left.Left );
            Assert.IsTrue( CheckLeaf( root.Right.Right.Left.Left, 0x65, 3 ));

            Assert.IsNotNull( root.Right.Right.Left.Right );
            Assert.IsTrue( CheckLeaf( root.Right.Right.Left.Right, 0x6f, 3 ));

            Assert.IsNotNull( root.Right.Right.Right );
            Assert.IsTrue( CheckLeaf( root.Right.Right.Right, 0x4f, 8 ));
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void GenerateCodes()
        {
            PrivateObject obj = new PrivateObject( new HuffmanTree() );

            var nodes = new List< Node >
            {
                new Node( 1, 0x10 ),
                new Node( 1, 0x11 ),
                new Node( 2, 0x12 ),
                new Node( 2, 0x13 ),
                new Node( 3, 0x14 )
            };

            obj.Invoke( "BuildTree", nodes );

            Node root = nodes[0];
            Dictionary< byte, List< bool >> codes = new HuffmanCodesGenerator().CreateCodesDictionary( root );
            List< bool > code;

            codes.TryGetValue( 0x12, out code );
            CollectionAssert.AreEqual( code, new List< bool > { false, true } );
            codes.TryGetValue( 0x13, out code );
            CollectionAssert.AreEqual( code, new List< bool > { true, false } );
            codes.TryGetValue( 0x14, out code );
            CollectionAssert.AreEqual( code, new List< bool > { true, true } );
            codes.TryGetValue( 0x11, out code );
            CollectionAssert.AreEqual( code, new List< bool > { false, false, true } );
            codes.TryGetValue( 0x10, out code );
            CollectionAssert.AreEqual( code, new List< bool > { false, false, false } );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private bool CheckNode( Node node, byte value, int count )
        {
            return node.ByteValue == value && node.Count == count;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private bool CheckLeaf( Node node, byte value, int count )
        {
            return CheckNode( node, value, count ) && node.IsLeaf();
        }
    }
}

