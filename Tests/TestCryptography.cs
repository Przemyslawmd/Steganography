﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Cryptography;

namespace Tests
{
    [TestClass]
    public class TestsCryptography
    {

        [TestMethod]
        public void TestAESEncryptBlockData()
        {
            //byte[] dataToEncrypt = new byte[16] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef, 0xfe, 0xdc, 0xba, 0x98, 0x76, 0x54, 0x32, 0x10 };
            byte[] dataToEncrypt = new byte[16] { 0x01, 0x89, 0xfe, 0x76, 0x23, 0xab, 0xdc, 0x54, 0x45, 0xcd, 0xba, 0x32, 0x67, 0xef, 0x98, 0x10 };

            //byte[] expectedData = new byte[16]  { 0xff, 0x0b, 0x84, 0x4a, 0x08, 0x53, 0xbf, 0x7c, 0x69, 0x34, 0xab, 0x43, 0x64, 0x14, 0x8f, 0xb9 };
            byte[] expectedData = new byte[16] { 0xff, 0x08, 0x69, 0x64, 0x0b, 0x53, 0x34, 0x14, 0x84, 0xbf, 0xab, 0x8f, 0x4a, 0x7c, 0x43, 0xb9 };

            byte[] key = new byte[16]           { 0x0f, 0x15, 0x71, 0xc9, 0x47, 0xd9, 0xe8, 0x59, 0x0c, 0xb7, 0xad, 0xd6, 0xaf, 0x7f, 0x67, 0x98 };

            PrivateType typeKey = new PrivateType( typeof( Key ) );
            byte[] expandedKey = (byte[])typeKey.InvokeStatic( "ExpandKey", key );
            
            PrivateObject typeEncryption = new PrivateObject( new Encryption() );            
            typeEncryption.Invoke( "EncryptBlockData", dataToEncrypt, expandedKey );

            Assert.AreEqual( dataToEncrypt[0], expectedData[0] );
        }

        /************************************************************************************************************/
        /* TEST DATA ALIGNMENT **************************************************************************************/

        [TestMethod]
        public void TestAESDataAlignment()
        {                                
            PrivateObject obj = new PrivateObject( new Encryption() );

            byte[] data = new byte[5];  
            data = ( byte[] )obj.Invoke( "AlignData", data );

            Assert.AreEqual( data.Length, 16 );
            Assert.AreEqual( data[data.Length - 1], 11 );
                        
            data = new byte[22];
            data = ( byte[] )obj.Invoke( "AlignData", data );

            Assert.AreEqual( data.Length, 32 );
            Assert.AreEqual( data[data.Length - 1], 10 );

            data = new byte[16] ;
            data = ( byte[] )obj.Invoke( "AlignData", data );

            Assert.AreEqual( data.Length, 32 );
            Assert.AreEqual( data[data.Length - 1], 16 );            
        }

        /************************************************************************************************************/
        /* TEST KEY EXPANSION ***************************************************************************************/

        [TestMethod]
        public void TestAESKeyExpansion()
        {
            byte[] inputKey = new byte[16] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f };

            byte[] expandedKey = new byte[176]
            {
                0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
                0xd6, 0xaa, 0x74, 0xfd, 0xd2, 0xaf, 0x72, 0xfa, 0xda, 0xa6, 0x78, 0xf1, 0xd6, 0xab, 0x76, 0xfe,
                0xb6, 0x92, 0xcf, 0x0b, 0x64, 0x3d, 0xbd, 0xf1, 0xbe, 0x9b, 0xc5, 0x00, 0x68, 0x30, 0xb3, 0xfe,
                0xb6, 0xff, 0x74, 0x4e, 0xd2, 0xc2, 0xc9, 0xbf, 0x6c, 0x59, 0x0C, 0xbf, 0x04, 0x69, 0xbf, 0x41,
                0x47, 0xf7, 0xf7, 0xbc, 0x95, 0x35, 0x3e, 0x03, 0xf9, 0x6C, 0x32, 0xbc, 0xfd, 0x05, 0x8d, 0xfd,
                0x3c, 0xaa, 0xa3, 0xe8, 0xa9, 0x9f, 0x9d, 0xeb, 0x50, 0xf3, 0xaf, 0x57, 0xad, 0xf6, 0x22, 0xaa,
                0x5e, 0x39, 0x0f, 0x7d, 0xf7, 0xa6, 0x92, 0x96, 0xa7, 0x55, 0x3d, 0xC1, 0x0a, 0xa3, 0x1f, 0x6b,
                0x14, 0xf9, 0x70, 0x1a, 0xe3, 0x5f, 0xe2, 0x8c, 0x44, 0x0a, 0xdf, 0x4d, 0x4e, 0xa9, 0xc0, 0x26,
                0x47, 0x43, 0x87, 0x35, 0xa4, 0x1c, 0x65, 0xb9, 0xe0, 0x16, 0xba, 0xf4, 0xae, 0xbf, 0x7a, 0xd2,
                0x54, 0x99, 0x32, 0xd1, 0xf0, 0x85, 0x57, 0x68, 0x10, 0x93, 0xed, 0x9C, 0xbe, 0x2c, 0x97, 0x4e,
                0x13, 0x11, 0x1d, 0x7f, 0xe3, 0x94, 0x4a, 0x17, 0xf3, 0x07, 0xa7, 0x8b, 0x4d, 0x2b, 0x30, 0xc5
            };

            PrivateType type = new PrivateType( typeof( Key ) );                 
            byte[] key = (byte[])type.InvokeStatic( "ExpandKey", inputKey );
            CollectionAssert.AreEqual( key, expandedKey );
            
            
            // An example from Stallings book
            inputKey = new byte[16] { 0x0f, 0x15, 0x71, 0xc9, 0x47, 0xd9, 0xe8, 0x59, 0x0c, 0xb7, 0xad, 0xd6, 0xaf, 0x7f, 0x67, 0x98 };

            expandedKey = new byte[176]
            {
                0x0f, 0x15, 0x71, 0xc9, 0x47, 0xd9, 0xe8, 0x59, 0x0c, 0xb7, 0xad, 0xd6, 0xaf, 0x7f, 0x67, 0x98,
                0xdc, 0x90, 0x37, 0xb0, 0x9b, 0x49, 0xdf, 0xe9, 0x97, 0xfe, 0x72, 0x3f, 0x38, 0x81, 0x15, 0xa7,
                0xd2, 0xc9, 0x6b, 0xb7, 0x49, 0x80, 0xb4, 0x5e, 0xde, 0x7e, 0xc6, 0x61, 0xe6, 0xff, 0xd3, 0xc6,
                0xc0, 0xaf, 0xdf, 0x39, 0x89, 0x2f, 0x6b, 0x67, 0x57, 0x51, 0xad, 0x06, 0xb1, 0xae, 0x7e, 0xc0,
                0x2c, 0x5c, 0x65, 0xf1, 0xa5, 0x73, 0x0e, 0x96, 0xf2, 0x22, 0xa3, 0x90, 0x43, 0x8c, 0xdd, 0x50,
                0x58, 0x9d, 0x36, 0xeb, 0xfd, 0xee, 0x38, 0x7d, 0x0f, 0xcc, 0x9b, 0xed, 0x4c, 0x40, 0x46, 0xbd,
                0x71, 0xc7, 0x4c, 0xc2, 0x8c, 0x29, 0x74, 0xbf, 0x83, 0xe5, 0xef, 0x52, 0xcf, 0xa5, 0xa9, 0xef,
                0x37, 0x14, 0x93, 0x48, 0xbb, 0x3d, 0xe7, 0xf7, 0x38, 0xd8, 0x08, 0xa5, 0xf7, 0x7d, 0xa1, 0x4a,
                0x48, 0x26, 0x45, 0x20, 0xf3, 0x1b, 0xa2, 0xd7, 0xcb, 0xc3, 0xaa, 0x72, 0x3c, 0xbe, 0x0b, 0x38,
                0xfd, 0x0d, 0x42, 0xcb, 0x0e, 0x16, 0xe0, 0x1c, 0xc5, 0xd5, 0x4a, 0x6e, 0xf9, 0x6b, 0x41, 0x56,
                0xb4, 0x8e, 0xf3, 0x52, 0xba, 0x98, 0x13, 0x4e, 0x7f, 0x4d, 0x59, 0x20, 0x86, 0x26, 0x18, 0x76
            };

            key = (byte[])type.InvokeStatic( "ExpandKey", inputKey );                      
            CollectionAssert.AreEqual( key, expandedKey );            
        }
        /************************************************************************************************************/
        /* TEST ADD ROUND KEY METHOD ********************************************************************************/

        [TestMethod]
        public void TestAESAddRoundKey()
        {
            byte[,] sourceData = new byte[4, 4] {   { 0x04, 0xE0, 0x48, 0x28 },
                                                    { 0x66, 0xCB, 0xF8, 0x06 },
                                                    { 0x81, 0x19, 0xD3, 0x26 },
                                                    { 0xE5, 0x9A, 0x7A, 0x4C } };
           
            byte[,] expectedData = new byte[4, 4] { { 0xA4, 0x68, 0x6B, 0x02 },
                                                    { 0x9C, 0x9F, 0x5B, 0x6A },
                                                    { 0x7F, 0x35, 0xEA, 0x50 },
                                                    { 0xF2, 0x2B, 0x43, 0x49 } };

            byte[] key = new byte[16] { 0xA0, 0x88, 0x23, 0x2A, 0xFA, 0x54, 0xA3, 0x6C, 0xFE, 0x2C, 0x39, 0x76, 0x17, 0xB1, 0x39, 0x05 };

            PrivateObject type = new PrivateObject( new BaseCryptography() );
            int roundNumber = 0;            
            
            type.Invoke( "AddRoundKey", roundNumber, sourceData, key );            
            CollectionAssert.AreEqual( sourceData, expectedData );
        }

        /************************************************************************************************************/
        /* TEST SUB BYTES TRANSFORMATION ****************************************************************************/

        [TestMethod]
        public void TestAESSubBytes()
        {
            byte[,] sourceData = new byte[4, 4] {   { 0x19, 0xa0, 0x9a, 0xe9 },
                                                    { 0x3d, 0xf4, 0xc6, 0xf8 }, 
                                                    { 0xe3, 0xe2, 0x8d, 0x48 }, 
                                                    { 0xbe, 0x2b, 0x2a, 0x08 } };

            byte[,] expectedData = new byte[4, 4] { { 0xd4, 0xe0, 0xb8, 0x1e },
                                                    { 0x27, 0xbf, 0xb4, 0x41 },
                                                    { 0x11, 0x98, 0x5d, 0x52 },
                                                    { 0xae, 0xf1, 0xe5, 0x30 } };            
           
            PrivateObject type = new PrivateObject( new Encryption() );
            
            type.Invoke( "SubBytes", sourceData );
            CollectionAssert.AreEqual( sourceData, expectedData );
        }

        /***************************************************************************************************************/
        /* TEST SHIFT ROWS *********************************************************************************************/

        [TestMethod]
        public void TestAESShiftRows()
        {
            byte[,] sourceData = new byte[4, 4]   { { 0x49, 0x45, 0x7f, 0x77 },
                                                    { 0xde, 0xdb, 0x39, 0x02 },
                                                    { 0xd2, 0x96, 0x87, 0x53 },
                                                    { 0x89, 0xf1, 0x1a, 0x3b } };

            byte[,] expectedData = new byte[4, 4] { { 0x49, 0x45, 0x7f, 0x77 },
                                                    { 0xdb, 0x39, 0x02, 0xde },
                                                    { 0x87, 0x53, 0xd2, 0x96 },
                                                    { 0x3b, 0x89, 0xf1, 0x1a } };

            PrivateObject type = new PrivateObject( new Encryption() );            
            
            type.Invoke( "ShiftRows", sourceData );
            CollectionAssert.AreEqual( sourceData, expectedData );
        }

        /***************************************************************************************************************/
        /* TEST MIX COLUMNS ********************************************************************************************/

        [TestMethod]
        public void TestAESMixColumns()
        {
            byte[,] sourceData = new byte[4, 4] {   { 0x2b, 0xe2, 0x25, 0x42 },
                                                    { 0x7e, 0x2f, 0x28, 0xb0 },
                                                    { 0x70, 0xd0, 0xfc, 0x28 },
                                                    { 0x62, 0x89, 0x34, 0xf8 } };

            byte[,] expectedData = new byte[4, 4] { { 0xC6, 0xF7, 0xFA, 0x9F },
                                                    { 0x25, 0x5E, 0x5E, 0xB9 },
                                                    { 0x13, 0xF6, 0xB2, 0xB1 },
                                                    { 0xB7, 0xCB, 0xD3, 0xB5 } };

            PrivateObject type = new PrivateObject( new Encryption() );
            
            type.Invoke( "MixColumns", sourceData );
            CollectionAssert.AreEqual( sourceData, expectedData );
        }

        /***************************************************************************************************************/
        /* TEST MULTIPLY BY 2 IN GF(2^8) *******************************************************************************/

        [TestMethod]
        public void TestAESMultiplyBy2()
        {
            byte input = 0xd4;
            byte expected = 0xb3;

            PrivateObject type = new PrivateObject( new Encryption() );
            input = (byte)type.Invoke( "MultiplyBy2", input );
            Assert.AreEqual( input, expected );

            input = 0xa3;
            expected = 0x5d;

            type = new PrivateObject( new Encryption() );
            input = (byte)type.Invoke( "MultiplyBy2", input );
            Assert.AreEqual( input, expected );
        }

        /***************************************************************************************************************/
        /* TEST MULTIPLY BY 3 IN GF(2^8) *******************************************************************************/

        [TestMethod]
        public void TestAESMultiplyBy3()
        {
            byte input = 0xbf;
            byte expected = 0xda;

            PrivateObject type = new PrivateObject( new Encryption() );
            input = (byte)type.Invoke( "MultiplyBy3", input );
            Assert.AreEqual( input, expected );
        }
    }
}
