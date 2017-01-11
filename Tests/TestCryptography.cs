﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Cryptography;

namespace Tests
{
    [TestClass]
    public class TestsCryptography
    {
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
            byte[] basicKey = new byte[16] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f };

            byte[] expandedKey = new byte[176]
            {
                /* 0 */   0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0D, 0x0E, 0x0f,
                /* 16 */  0xD6, 0xAA, 0x74, 0xFD, 0xD2, 0xAF, 0x72, 0xFA, 0xDA, 0xa6, 0x78, 0xf1, 0xd6, 0xAB, 0x76, 0xFE,
                /* 32 */  0xB6, 0x92, 0xCF, 0x0B, 0x64, 0x3D, 0xBD, 0xF1, 0xBE, 0x9b, 0xc5, 0x00, 0x68, 0x30, 0xB3, 0xFE,
                /* 48 */  0xB6, 0xFF, 0x74, 0x4E, 0xD2, 0xC2, 0xC9, 0xBF, 0x6C, 0x59, 0x0C, 0xbf, 0x04, 0x69, 0xBF, 0x41,
                /* 64 */  0x47, 0xF7, 0xF7, 0xBC, 0x95, 0x35, 0x3E, 0x03, 0xF9, 0x6C, 0x32, 0xbc, 0xfd, 0x05, 0x8D, 0xFD,
                /* 80 */  0x3C, 0xaa, 0xA3, 0xE8, 0xA9, 0x9F, 0x9D, 0xEB, 0x50, 0xf3, 0xaf, 0x57, 0xad, 0xF6, 0x22, 0xAA,
                /* 96 */  0x5E, 0x39, 0x0F, 0x7D, 0xF7, 0xA6, 0x92, 0x96, 0xA7, 0x55, 0x3d, 0xC1, 0x0a, 0xA3, 0x1F, 0x6B,
                /* 112 */ 0x14, 0xF9, 0x70, 0x1A, 0xE3, 0x5F, 0xE2, 0x8C, 0x44, 0x0a, 0xdf, 0x4d, 0x4e, 0xA9, 0xc0, 0x26,
                /* 128 */ 0x47, 0x43, 0x87, 0x35, 0xA4, 0x1C, 0x65, 0xB9, 0xE0, 0x16, 0xba, 0xf4, 0xae, 0xBF, 0x7A, 0xD2,
                /* 144 */ 0x54, 0x99, 0x32, 0xD1, 0xF0, 0x85, 0x57, 0x68, 0x10, 0x93, 0xed, 0x9C, 0xbe, 0x2C, 0x97, 0x4E,
                /* 160 */ 0x13, 0x11, 0x1D, 0x7F, 0xE3, 0x94, 0x4A, 0x17, 0xF3, 0x07, 0xa7, 0x8b, 0x4d, 0x2B, 0x30, 0xC5
            };

            PrivateType type = new PrivateType( typeof( Key ) );
            byte[] key = (byte[])type.InvokeStatic( "ExpandKey", basicKey );            
            CollectionAssert.AreEqual( key, expandedKey );
        }

        /************************************************************************************************************/
        /* TEST ADD ROUND KEY METHOD ********************************************************************************/

        [TestMethod]
        public void TestAESAddRoundKey()
        {
            byte[] sourceData = new byte[16] { 0x04, 0xE0, 0x48, 0x28, 0x66, 0xCB, 0xF8, 0x06, 0x81, 0x19, 0xD3, 0x26, 0xE5, 0x9A, 0x7A, 0x4C };
            byte[] key = new byte[16] { 0xA0, 0x88, 0x23, 0x2A, 0xFA, 0x54, 0xA3, 0x6C, 0xFE, 0x2C, 0x39, 0x76, 0x17, 0xB1, 0x39, 0x05 };
            byte[] expectedData = new byte[16] { 0xA4, 0x68, 0x6B, 0x02, 0x9C, 0x9F, 0x5B, 0x6A, 0x7F, 0x35, 0xEA, 0x50, 0xF2, 0x2B, 0x43, 0x49 };
            
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
            byte[] sourceData = new byte[16] { 0x19, 0xA0, 0x9A, 0xE9, 0x3D, 0xF4, 0xC6, 0xF8, 0xE3, 0xE2, 0x8D, 0x48, 0xBE, 0x2B, 0x2A, 0x08 };
            byte[] expectedData = new byte[16] { 0xD4, 0xE0, 0xB8, 0x1E, 0x27, 0xBF, 0xB4, 0x41, 0x11, 0x98, 0x5D, 0x52, 0xAE, 0xF1, 0xE5, 0x30 };
            
            for ( int i = 0; i < 16; i++ )
                sourceData[i] = BaseCryptography.GetSbox( sourceData[i] );

            CollectionAssert.AreEqual( sourceData, expectedData );
        }
    }
}
