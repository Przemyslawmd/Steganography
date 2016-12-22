using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Cryptography;

namespace Tests
{
    [TestClass]
    public class TestsCryptography
    {
        [TestMethod]
        public void TestDataAlignment()
        {                                
            PrivateObject obj = new PrivateObject( new Encryption());

            List<byte> data = new List<byte>( new byte[5] );  
            obj.Invoke( "AlignData", data );

            Assert.AreEqual(data.Count, 16);
            Assert.AreEqual( data[data.Count - 1], 11 );

            data = new List<byte>( new byte[22] );
            obj.Invoke( "AlignData", data );

            Assert.AreEqual( data.Count, 32 );
            Assert.AreEqual( data[data.Count - 1], 10 );

            data = new List<byte>( new byte[16] );
            obj.Invoke( "AlignData", data );

            Assert.AreEqual( data.Count, 32 );
            Assert.AreEqual( data[data.Count - 1], 16 );
        }
    }
}
