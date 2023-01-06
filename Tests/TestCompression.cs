﻿
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
        public void CompressWithoutDictionaryCodes()
        {
            var dataToCompress = new List< byte >( System.Text.Encoding.Unicode.GetBytes( "AxC2cc&422Avdfr" ));
            NodeCompress root = new HuffmanTree().BuildTreeCompression( dataToCompress );
            Dictionary< byte, List< bool >> codes = new HuffmanCodesGenerator().CreateCodesDictionary( root );

            PrivateObject objectCompression = new PrivateObject( new Compression() );

            var dataCompressed = ( List< byte > ) objectCompression.Invoke( "Compress", dataToCompress, codes );

            var expectedData = new List< byte >{ 0x97, 0x6f, 0x47, 0x1c, 0xf9, 0xf5, 0x75, 
                                                 0xf1, 0xc7, 0x2e, 0xce, 0x9e, 0xee, 0xfc };

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
        public void BuildCompressionTree()
        {
            PrivateObject obj = new PrivateObject( new HuffmanTree() );

            var nodes = new List< NodeCompress >
            {
                new NodeCompress( 1, 0x57 ),
                new NodeCompress( 2, 0x41 ),
                new NodeCompress( 2, 0x56 ),
                new NodeCompress( 3, 0x65 ),
                new NodeCompress( 3, 0x6f ),
                new NodeCompress( 4, 0x72 ),
                new NodeCompress( 5, 0x64 ),
                new NodeCompress( 5, 0x6c ),
                new NodeCompress( 8, 0x4f ),
                new NodeCompress( 10, 0x4b )

            };

            obj.Invoke( "BuildTree", nodes );
            NodeCompress root = nodes[0];

            Assert.IsNotNull( root.Left.Left.Left );
            Assert.IsTrue( CheckLeaf( (NodeCompress) root.Left.Left.Left, 0x72, 4 ));

            Assert.IsNotNull( root.Left.Left.Right.Left );
            Assert.IsTrue( CheckLeaf( (NodeCompress) root.Left.Left.Right.Left, 0x56, 2 ));

            Assert.IsNotNull( root.Left.Left.Right.Right.Left );
            Assert.IsTrue( CheckLeaf( (NodeCompress) root.Left.Left.Right.Right.Left, 0x57, 1 ));

            Assert.IsNotNull( root.Left.Left.Right.Right.Right );
            Assert.IsTrue( CheckLeaf( (NodeCompress) root.Left.Left.Right.Right.Right, 0x41, 2 ));

            Assert.IsNotNull( root.Left.Right.Left );
            Assert.IsTrue( CheckLeaf( (NodeCompress) root.Left.Right.Left, 0x64, 5 ));

            Assert.IsNotNull( root.Left.Right.Right );
            Assert.IsTrue( CheckLeaf( (NodeCompress) root.Left.Right.Right, 0x6c, 5 ));

            Assert.IsNotNull( root.Right.Left );
            Assert.IsTrue( CheckLeaf( (NodeCompress) root.Right.Left, 0x4b, 10 ));

            Assert.IsNotNull( root.Right.Right.Left.Left );
            Assert.IsTrue( CheckLeaf( (NodeCompress) root.Right.Right.Left.Left, 0x65, 3 ));

            Assert.IsNotNull( root.Right.Right.Left.Right );
            Assert.IsTrue( CheckLeaf( (NodeCompress) root.Right.Right.Left.Right, 0x6f, 3 ));

            Assert.IsNotNull( root.Right.Right.Right );
            Assert.IsTrue( CheckLeaf( (NodeCompress) root.Right.Right.Right, 0x4f, 8 ));
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void GenerateCodes()
        {
            PrivateObject obj = new PrivateObject( new HuffmanTree() );

            var nodes = new List< NodeCompress >
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
            CollectionAssert.AreEqual( code, new List< bool > { true, false, true } );
            codesDictionary.TryGetValue( 0x13, out code );
            CollectionAssert.AreEqual( code, new List< bool > { true, true, false } );
            codesDictionary.TryGetValue( 0x14, out code );
            CollectionAssert.AreEqual( code, new List< bool > { true, true, true } );
            codesDictionary.TryGetValue( 0x11, out code );
            CollectionAssert.AreEqual( code, new List< bool > { true, false, false, true } );
            codesDictionary.TryGetValue( 0x10, out code );
            CollectionAssert.AreEqual( code, new List< bool > { true, false, false, false } );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private bool CheckNode( NodeCompress node, byte value, int count )
        {
            return node.ByteValue == value && node.Count == count;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private bool CheckLeaf( NodeCompress node, byte value, int count )
        {
            return CheckNode( node, value, count ) && node.IsLeaf();
        }
    }
}

