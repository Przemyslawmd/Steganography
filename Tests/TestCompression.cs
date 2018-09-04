
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
        public void TestCompressionShortTextWithoutDictionaryCodes()
        {
            List< byte > dataToCompress = new List< byte >( System.Text.Encoding.Unicode.GetBytes( "AxC2cc&422Avdfr" ));
            NodeCompress root = new HuffmanTree().BuildTreeCompression( dataToCompress );
            Dictionary< byte, List< bool >> codes = new HuffmanCodesGenerator().CreateCodesDictionary( root );

            PrivateObject objectCompression = new PrivateObject( new Compression() );
            objectCompression.SetField( "codes", codes );

            List< byte > dataCompressed = (List< byte >) objectCompression.Invoke( "StartCompress", dataToCompress );

            List< byte > expectedData = new List< byte >{ 0xD5, 0xFD, 0xD5, 0x96, 0xED,
                                                          0xDC, 0x5C, 0xD9, 0x65, 0xAB,
                                                          0xEB, 0xBB, 0xCB, 0xD8 };

            CollectionAssert.AreEqual( dataCompressed, expectedData );
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

        [TestMethod]
        public void TestCompressionGeneratingCodes()
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

        static List< NodeCompress > nodes;
    }
}
