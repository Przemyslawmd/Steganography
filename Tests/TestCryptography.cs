
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteganographyEncryption;
using System.IO;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class TestsCryptography
    {
        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestAES()
        {
            String password = "3ndnui@uh2";    
            string projectPath = Directory.GetParent( Directory.GetCurrentDirectory() ).Parent.FullName;            
            string filePath = Path.Combine( projectPath, "Resources\\fileToTest.txt" );

            List< byte > data = new List<byte>( File.ReadAllBytes( filePath ) );
            List< byte > dataCopy = new List<byte>( data );
                        
            data = new Encryption().Encrypt( data, password );
            CollectionAssert.AreNotEqual( data, dataCopy );

            List< byte > decompressedData = new Decryption().Decrypt( data, password );
            CollectionAssert.AreEqual( decompressedData, dataCopy );
        } 
                  
        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestAESEncryptBlockData()
        {
            PrivateType key = new PrivateType( typeof( Key ));
            PrivateObject encryption = new PrivateObject( new Encryption() );
                        

            byte[] initialKey = new byte[16] { 0x54, 0x68, 0x61, 0x74,
                                               0x73, 0x20, 0x6D, 0x79,
                                               0x20, 0x4B, 0x75, 0x6E,
                                               0x67, 0x20, 0x46, 0x75 };

            byte[,] blockToEncrypt = new byte[4, 4] {{ 0x54, 0x4F, 0x4E, 0x20 },
                                                    {  0x77, 0x6E, 0x69, 0x54 },
                                                    {  0x6F, 0x65, 0x6E, 0x77 },
                                                    {  0x20, 0x20, 0x65, 0x6F }};

            
            byte[,] blockExpected  = new byte[4, 4] {{ 0x29, 0x57, 0x40, 0x1A },
                                                    {  0xC3, 0x14, 0x22, 0x02 },
                                                    {  0x50, 0x20, 0x99, 0xD7 },
                                                    {  0x5F, 0xF6, 0xB3, 0x3A }};


            byte[][] roundKeys = (byte[][]) key.InvokeStatic( "ExpandKey", initialKey );            
            encryption.Invoke( "EncryptBlockData", blockToEncrypt, roundKeys );
            CollectionAssert.AreEqual( blockToEncrypt, blockExpected );
                        

            initialKey = new byte[16]       { 0x2B, 0x7E, 0x15, 0x16,
                                              0x28, 0xAE, 0xD2, 0xA6,
                                              0xAB, 0xF7, 0x15, 0x88,
                                              0x09, 0xCF, 0x4F, 0x3C };

            blockToEncrypt = new byte[4, 4] {{ 0x32, 0x88, 0x31, 0xE0 },
                                             { 0x43, 0x5A, 0x31, 0x37 },
                                             { 0xF6, 0x30, 0x98, 0x07 },
                                             { 0xA8, 0x8D, 0xA2, 0x34 }};


            blockExpected = new byte[4, 4]  {{ 0x39, 0x02, 0xDC, 0x19 },
                                             { 0x25, 0xDC, 0x11, 0x6A },
                                             { 0x84, 0x09, 0x85, 0x0B },
                                             { 0x1D, 0xFB, 0x97, 0x32 }};
                                    
            roundKeys = (byte[][]) key.InvokeStatic( "ExpandKey", initialKey );
            encryption.Invoke( "EncryptBlockData", blockToEncrypt, roundKeys );
            CollectionAssert.AreEqual( blockToEncrypt, blockExpected );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestAESDecryptBlockData()
        {
            PrivateType key = new PrivateType( typeof( Key ) );
            PrivateObject decryption = new PrivateObject( new Decryption() );


            byte[] initialKey = new byte[16]  { 0x54, 0x68, 0x61, 0x74,
                                                0x73, 0x20, 0x6D, 0x79,
                                                0x20, 0x4B, 0x75, 0x6E,
                                                0x67, 0x20, 0x46, 0x75 };

            byte[,] blockExpected = new byte[4, 4] {{ 0x54, 0x4F, 0x4E, 0x20 },
                                                    { 0x77, 0x6E, 0x69, 0x54 },
                                                    { 0x6F, 0x65, 0x6E, 0x77 },
                                                    { 0x20, 0x20, 0x65, 0x6F }};


            byte[,] blockToDecrypt = new byte[4, 4] {{ 0x29, 0x57, 0x40, 0x1A },
                                                     { 0xC3, 0x14, 0x22, 0x02 },
                                                     { 0x50, 0x20, 0x99, 0xD7 },
                                                     { 0x5F, 0xF6, 0xB3, 0x3A }};

            byte[][] roundKeys = (byte[][])key.InvokeStatic( "ExpandKey", initialKey );
            decryption.Invoke( "DecryptBlockData", blockToDecrypt, roundKeys );
            CollectionAssert.AreEqual( blockToDecrypt, blockExpected );
                        

            initialKey = new byte[16] { 0x2B, 0x7E, 0x15, 0x16,
                                        0x28, 0xAE, 0xD2, 0xA6,
                                        0xAB, 0xF7, 0x15, 0x88,
                                        0x09, 0xCF, 0x4F, 0x3C };

            blockExpected = new byte[4, 4]  {{ 0x32, 0x88, 0x31, 0xE0 },
                                             { 0x43, 0x5A, 0x31, 0x37 },
                                             { 0xF6, 0x30, 0x98, 0x07 },
                                             { 0xA8, 0x8D, 0xA2, 0x34 }};


            blockToDecrypt = new byte[4, 4] {{ 0x39, 0x02, 0xDC, 0x19 },
                                             { 0x25, 0xDC, 0x11, 0x6A },
                                             { 0x84, 0x09, 0x85, 0x0B },
                                             { 0x1D, 0xFB, 0x97, 0x32 }};

            roundKeys = (byte[][]) key.InvokeStatic( "ExpandKey", initialKey );
            decryption.Invoke( "DecryptBlockData", blockToDecrypt, roundKeys );
            CollectionAssert.AreEqual( blockToDecrypt, blockExpected );
        }
        
        /************************************************************************************************************/
        /* TEST DATA ALIGNMENT **************************************************************************************/

        [TestMethod]
        public void TestAESDataAlignment()
        {                                
            PrivateObject obj = new PrivateObject( new Encryption() );
            List<byte> data = new List<byte>();

            for ( int i = 0; i < 5; i++ )
                data.Add( 0x00 );

            obj.Invoke( "AlignData", data );
            Assert.AreEqual( data.Count, 16 );
            Assert.AreEqual( data[data.Count - 1], 11 );
            data.Clear();


            for ( int i = 0; i < 22; i++ )
                data.Add( 0x00 );

            obj.Invoke( "AlignData", data );
            Assert.AreEqual( data.Count, 32 );
            Assert.AreEqual( data[data.Count - 1], 10 );
            data.Clear();


            for ( int i = 0; i < 16; i++ )
                data.Add( 0x00 );

            obj.Invoke( "AlignData", data );
            Assert.AreEqual( data.Count, 32 );
            Assert.AreEqual( data[data.Count - 1], 16 );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestAESKeyExpansion()
        {
            PrivateType type = new PrivateType( typeof( Key ) );                       

            byte[] initialKey = new byte[16] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
                                               0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f };

            byte[][] expectKey = new byte[11][];
            expectKey[0]  = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f };
            expectKey[1]  = new byte[] { 0xd6, 0xaa, 0x74, 0xfd, 0xd2, 0xaf, 0x72, 0xfa, 0xda, 0xa6, 0x78, 0xf1, 0xd6, 0xab, 0x76, 0xfe };
            expectKey[2]  = new byte[] { 0xb6, 0x92, 0xcf, 0x0b, 0x64, 0x3d, 0xbd, 0xf1, 0xbe, 0x9b, 0xc5, 0x00, 0x68, 0x30, 0xb3, 0xfe };
            expectKey[3]  = new byte[] { 0xb6, 0xff, 0x74, 0x4e, 0xd2, 0xc2, 0xc9, 0xbf, 0x6c, 0x59, 0x0C, 0xbf, 0x04, 0x69, 0xbf, 0x41 };
            expectKey[4]  = new byte[] { 0x47, 0xf7, 0xf7, 0xbc, 0x95, 0x35, 0x3e, 0x03, 0xf9, 0x6C, 0x32, 0xbc, 0xfd, 0x05, 0x8d, 0xfd };
            expectKey[5]  = new byte[] { 0x3c, 0xaa, 0xa3, 0xe8, 0xa9, 0x9f, 0x9d, 0xeb, 0x50, 0xf3, 0xaf, 0x57, 0xad, 0xf6, 0x22, 0xaa };
            expectKey[6]  = new byte[] { 0x5e, 0x39, 0x0f, 0x7d, 0xf7, 0xa6, 0x92, 0x96, 0xa7, 0x55, 0x3d, 0xC1, 0x0a, 0xa3, 0x1f, 0x6b };
            expectKey[7]  = new byte[] { 0x14, 0xf9, 0x70, 0x1a, 0xe3, 0x5f, 0xe2, 0x8c, 0x44, 0x0a, 0xdf, 0x4d, 0x4e, 0xa9, 0xc0, 0x26 };
            expectKey[8]  = new byte[] { 0x47, 0x43, 0x87, 0x35, 0xa4, 0x1c, 0x65, 0xb9, 0xe0, 0x16, 0xba, 0xf4, 0xae, 0xbf, 0x7a, 0xd2 };
            expectKey[9]  = new byte[] { 0x54, 0x99, 0x32, 0xd1, 0xf0, 0x85, 0x57, 0x68, 0x10, 0x93, 0xed, 0x9C, 0xbe, 0x2c, 0x97, 0x4e };
            expectKey[10] = new byte[] { 0x13, 0x11, 0x1d, 0x7f, 0xe3, 0x94, 0x4a, 0x17, 0xf3, 0x07, 0xa7, 0x8b, 0x4d, 0x2b, 0x30, 0xc5 };
                        
            byte[][] roundKeys = (byte[][]) type.InvokeStatic( "ExpandKey", initialKey );
                        
            for ( int i = 0; i < 11; i++ )
            {
                CollectionAssert.AreEqual( roundKeys[i], expectKey[i] );
            }

            initialKey = new byte[16] { 0x0f, 0x15, 0x71, 0xc9, 0x47, 0xd9, 0xe8, 0x59, 0x0c, 0xb7, 0xad, 0xd6, 0xaf, 0x7f, 0x67, 0x98 };

            expectKey[0]  = new byte[] { 0x0f, 0x15, 0x71, 0xc9, 0x47, 0xd9, 0xe8, 0x59, 0x0c, 0xb7, 0xad, 0xd6, 0xaf, 0x7f, 0x67, 0x98 };
            expectKey[1]  = new byte[] { 0xdc, 0x90, 0x37, 0xb0, 0x9b, 0x49, 0xdf, 0xe9, 0x97, 0xfe, 0x72, 0x3f, 0x38, 0x81, 0x15, 0xa7 };
            expectKey[2]  = new byte[] { 0xd2, 0xc9, 0x6b, 0xb7, 0x49, 0x80, 0xb4, 0x5e, 0xde, 0x7e, 0xc6, 0x61, 0xe6, 0xff, 0xd3, 0xc6 };
            expectKey[3]  = new byte[] { 0xc0, 0xaf, 0xdf, 0x39, 0x89, 0x2f, 0x6b, 0x67, 0x57, 0x51, 0xad, 0x06, 0xb1, 0xae, 0x7e, 0xc0 };
            expectKey[4]  = new byte[] { 0x2c, 0x5c, 0x65, 0xf1, 0xa5, 0x73, 0x0e, 0x96, 0xf2, 0x22, 0xa3, 0x90, 0x43, 0x8c, 0xdd, 0x50 };
            expectKey[5]  = new byte[] { 0x58, 0x9d, 0x36, 0xeb, 0xfd, 0xee, 0x38, 0x7d, 0x0f, 0xcc, 0x9b, 0xed, 0x4c, 0x40, 0x46, 0xbd };
            expectKey[6]  = new byte[] { 0x71, 0xc7, 0x4c, 0xc2, 0x8c, 0x29, 0x74, 0xbf, 0x83, 0xe5, 0xef, 0x52, 0xcf, 0xa5, 0xa9, 0xef };
            expectKey[7]  = new byte[] { 0x37, 0x14, 0x93, 0x48, 0xbb, 0x3d, 0xe7, 0xf7, 0x38, 0xd8, 0x08, 0xa5, 0xf7, 0x7d, 0xa1, 0x4a };
            expectKey[8]  = new byte[] { 0x48, 0x26, 0x45, 0x20, 0xf3, 0x1b, 0xa2, 0xd7, 0xcb, 0xc3, 0xaa, 0x72, 0x3c, 0xbe, 0x0b, 0x38 };
            expectKey[9]  = new byte[] { 0xfd, 0x0d, 0x42, 0xcb, 0x0e, 0x16, 0xe0, 0x1c, 0xc5, 0xd5, 0x4a, 0x6e, 0xf9, 0x6b, 0x41, 0x56 };
            expectKey[10] = new byte[] { 0xb4, 0x8e, 0xf3, 0x52, 0xba, 0x98, 0x13, 0x4e, 0x7f, 0x4d, 0x59, 0x20, 0x86, 0x26, 0x18, 0x76 };
            
            roundKeys = (byte[][])type.InvokeStatic( "ExpandKey", initialKey );

            for ( int i = 0; i < 11; i++ )
            {
                CollectionAssert.AreEqual( roundKeys[i], expectKey[i] );
            }
            
            initialKey = new byte[16] { 0x54, 0x68, 0x61, 0x74, 0x73, 0x20, 0x6D, 0x79, 0x20, 0x4B, 0x75, 0x6E, 0x67, 0x20, 0x46, 0x75};

            expectKey[0] = new byte[] { 0x54, 0x68, 0x61, 0x74, 0x73, 0x20, 0x6D, 0x79, 0x20, 0x4B, 0x75, 0x6E, 0x67, 0x20, 0x46, 0x75 };
            expectKey[1] = new byte[] { 0xE2, 0x32, 0xFC, 0xF1, 0x91, 0x12, 0x91, 0x88, 0xB1, 0x59, 0xE4, 0xE6, 0xD6, 0x79, 0xA2, 0x93 };
            expectKey[2] = new byte[] { 0x56, 0x08, 0x20, 0x07, 0xC7, 0x1A, 0xB1, 0x8F, 0x76, 0x43, 0x55, 0x69, 0xA0, 0x3A, 0xF7, 0xFA };
            expectKey[3] = new byte[] { 0xD2, 0x60, 0x0D, 0xE7, 0x15, 0x7A, 0xBC, 0x68, 0x63, 0x39, 0xE9, 0x01, 0xC3, 0x03, 0x1E, 0xFB };
            expectKey[4] = new byte[] { 0xA1, 0x12, 0x02, 0xC9, 0xB4, 0x68, 0xBE, 0xA1, 0xD7, 0x51, 0x57, 0xA0, 0x14, 0x52, 0x49, 0x5B };
            expectKey[5] = new byte[] { 0xB1, 0x29, 0x3B, 0x33, 0x05, 0x41, 0x85, 0x92, 0xD2, 0x10, 0xD2, 0x32, 0xC6, 0x42, 0x9B, 0x69 };
            expectKey[6] = new byte[] { 0xBD, 0x3D, 0xC2, 0x87, 0xB8, 0x7C, 0x47, 0x15, 0x6A, 0x6C, 0x95, 0x27, 0xAC, 0x2E, 0x0E, 0x4E };
            expectKey[7] = new byte[] { 0xCC, 0x96, 0xED, 0x16, 0x74, 0xEA, 0xAA, 0x03, 0x1E, 0x86, 0x3F, 0x24, 0xB2, 0xA8, 0x31, 0x6A };
            expectKey[8] = new byte[] { 0x8E, 0x51, 0xEF, 0x21, 0xFA, 0xBB, 0x45, 0x22, 0xE4, 0x3D, 0x7A, 0x06, 0x56, 0x95, 0x4B, 0x6C };
            expectKey[9] = new byte[] { 0xBF, 0xE2, 0xBF, 0x90, 0x45, 0x59, 0xFA, 0xB2, 0xA1, 0x64, 0x80, 0xB4, 0xF7, 0xF1, 0xCB, 0xD8 };
            expectKey[10] = new byte[] { 0x28, 0xFD, 0xDE, 0xF8, 0x6D, 0xA4, 0x24, 0x4A, 0xCC, 0xC0, 0xA4, 0xFE, 0x3B, 0x31, 0x6F, 0x26 };

            roundKeys = (byte[][])type.InvokeStatic( "ExpandKey", initialKey );

            for ( int i = 0; i < 11; i++ )
            {
                CollectionAssert.AreEqual( roundKeys[i], expectKey[i] );
            }
        }
        
        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestAESAddRoundKey()
        {
            byte[,] initialBlock = new byte[4, 4]  {{ 0x04, 0xE0, 0x48, 0x28 },
                                                    { 0x66, 0xCB, 0xF8, 0x06 },
                                                    { 0x81, 0x19, 0xD3, 0x26 },
                                                    { 0xE5, 0x9A, 0x7A, 0x4C }};
           
            byte[,] expectedBlock = new byte[4, 4] {{ 0xA4, 0x68, 0x6B, 0x02 },
                                                    { 0x9C, 0x9F, 0x5B, 0x6A },
                                                    { 0x7F, 0x35, 0xEA, 0x50 },
                                                    { 0xF2, 0x2B, 0x43, 0x49 }};

            byte[] key = new byte[16]               { 0xA0, 0xFA, 0xFE, 0x17,
                                                      0x88, 0x54, 0x2C, 0xB1,
                                                      0x23, 0xA3, 0x39, 0x39,
                                                      0x2A, 0x6C, 0x76, 0x05 };

            PrivateObject type = new PrivateObject( new BaseCryptography() );                 
            type.Invoke( "AddRoundKey", initialBlock, key );            
            CollectionAssert.AreEqual( initialBlock, expectedBlock );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestAESSubBytes()
        {
            byte[,] initialState = new byte[4, 4]  {{ 0x19, 0xa0, 0x9a, 0xe9 },
                                                    { 0x3d, 0xf4, 0xc6, 0xf8 }, 
                                                    { 0xe3, 0xe2, 0x8d, 0x48 }, 
                                                    { 0xbe, 0x2b, 0x2a, 0x08 }};

            byte[,] expectedState = new byte[4, 4] {{ 0xd4, 0xe0, 0xb8, 0x1e },
                                                    { 0x27, 0xbf, 0xb4, 0x41 },
                                                    { 0x11, 0x98, 0x5d, 0x52 },
                                                    { 0xae, 0xf1, 0xe5, 0x30 }};            
           
            PrivateObject type = new PrivateObject( new Encryption() );            
            type.Invoke( "SubBytes", initialState );
            CollectionAssert.AreEqual( initialState, expectedState );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestAESInvSubBytes()
        {
            byte[,] initialState = new byte[4, 4]  {{ 0xac, 0xef, 0x13, 0x45 },
                                                    { 0x73, 0xc1, 0xb5, 0x23 },
                                                    { 0xcf, 0x11, 0xd6, 0x5a },
                                                    { 0x7b, 0xdf, 0xb5, 0xb8 }};

            byte[,] expectedState = new byte[4, 4] {{ 0xaa, 0x61, 0x82, 0x68 },
                                                    { 0x8f, 0xdd, 0xd2, 0x32 },
                                                    { 0x5f, 0xe3, 0x4a, 0x46 },
                                                    { 0x03, 0xef, 0xd2, 0x9a }};

            PrivateObject type = new PrivateObject( new Decryption() );
            type.Invoke( "InvSubBytes", initialState );
            CollectionAssert.AreEqual( initialState, expectedState );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestAESShiftRows()
        {
            byte[,] initialData = new byte[4, 4]   {{ 0x49, 0x45, 0x7f, 0x77 },
                                                    { 0xde, 0xdb, 0x39, 0x02 },
                                                    { 0xd2, 0x96, 0x87, 0x53 },
                                                    { 0x89, 0xf1, 0x1a, 0x3b }};

            byte[,] expectedData = new byte[4, 4]  {{ 0x49, 0x45, 0x7f, 0x77 },
                                                    { 0xdb, 0x39, 0x02, 0xde },
                                                    { 0x87, 0x53, 0xd2, 0x96 },
                                                    { 0x3b, 0x89, 0xf1, 0x1a }};

            PrivateObject type = new PrivateObject( new Encryption() );           
            type.Invoke( "ShiftRows", initialData );
            CollectionAssert.AreEqual( initialData, expectedData );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestAESInvShiftRows()
        {
            byte[,] initialData = new byte[4, 4]   {{ 0x87, 0xf2, 0x4d, 0x97 },
                                                    { 0x6e, 0x4c, 0x90, 0xec },
                                                    { 0x46, 0xe7, 0x4a, 0xc3 },
                                                    { 0xa6, 0x8c, 0xd8, 0x95 }};

            byte[,] expectedData = new byte[4, 4]  {{ 0x87, 0xf2, 0x4d, 0x97 },
                                                    { 0xec, 0x6e, 0x4c, 0x90 },
                                                    { 0x4a, 0xc3, 0x46, 0xe7 },
                                                    { 0x8c, 0xd8, 0x95, 0xa6 }};

            PrivateObject type = new PrivateObject( new Decryption() );
            type.Invoke( "InvShiftRows", initialData );
            CollectionAssert.AreEqual( initialData, expectedData );
       }   
        
       /***************************************************************************************/
       /***************************************************************************************/

       [TestMethod]
       public void TestAESMixColumns()
        {
            byte[,] sourceData   = new byte[4, 4] {{ 0x2b, 0xe2, 0x25, 0x42 },
                                                   { 0x7e, 0x2f, 0x28, 0xb0 },
                                                   { 0x70, 0xd0, 0xfc, 0x28 },
                                                   { 0x62, 0x89, 0x34, 0xf8 }};

            byte[,] expectedData = new byte[4, 4] {{ 0xC6, 0xF7, 0xFA, 0x9F },
                                                   { 0x25, 0x5E, 0x5E, 0xB9 },
                                                   { 0x13, 0xF6, 0xB2, 0xB1 },
                                                   { 0xB7, 0xCB, 0xD3, 0xB5 }};

            PrivateObject type = new PrivateObject( new Encryption() );            
            type.Invoke( "MixColumns", sourceData );
            CollectionAssert.AreEqual( sourceData, expectedData );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestAESInvMixColumns()
        {
            byte[,] sourceData = new byte[4, 4] {{ 0x04, 0xe0, 0x48, 0x28 },
                                                 { 0x66, 0xcb, 0xf8, 0x06 },
                                                 { 0x81, 0x19, 0xd3, 0x26 },
                                                 { 0xe5, 0x9a, 0x7a, 0x4c }};

            byte[,] expectedData = new byte[4, 4] {{ 0xd4, 0xe0, 0xb8, 0x1e },
                                                   { 0xbf, 0xb4, 0x41, 0x27 },
                                                   { 0x5d, 0x52, 0x11, 0x98 },
                                                   { 0x30, 0xae, 0xf1, 0xe5 }};

            PrivateObject type = new PrivateObject( new Decryption() );
            type.Invoke( "InvMixColumns", sourceData );
            CollectionAssert.AreEqual( sourceData, expectedData );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        [TestMethod]
        public void TestAESMultiply()
        {
            PrivateObject type = new PrivateObject( new Encryption() );

            byte result = (byte) type.Invoke( "Multiply", (byte)0xd4, (byte)0x02 );
            byte expected = 0xb3;
            Assert.AreEqual( result, expected );
                        
            result = (byte) type.Invoke( "Multiply", (byte)0xa3, (byte)0x02 );
            expected = 0x5d;
            Assert.AreEqual( result, expected );

            result = (byte) type.Invoke( "Multiply", (byte)0xbf, (byte)0x03 );
            expected = 0xda;
            Assert.AreEqual( result, expected );

            result = (byte) type.Invoke( "Multiply", (byte)0x57, (byte)0x13 );
            expected = 0xFE;
            Assert.AreEqual( result, expected );
        }       
    }
}

