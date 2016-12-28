using System;
using System.Security.Cryptography;
using System.Text;

namespace Cryptography
{
    class Key
    {
        public static byte[] CreateKey( String password )
        {            
            byte[] basicKey = new Rfc2898DeriveBytes( password, salt, iterations ).GetBytes( keyBytes );
            return ExpandKey( basicKey );
        }


        private static byte[] ExpandKey( byte[] key )
        {
            byte[] expandedKey = new byte[176];

            Array.Copy( key, 0, expandedKey, 0, keyBytes );

            return expandedKey;
        }

        static readonly byte[] salt = new byte[] { 4, 32, 3, 112, 34, 11, 45, 26, 4, 34 };
        static readonly int iterations = 200;
        static readonly int keyBytes = 16;
    }
}


