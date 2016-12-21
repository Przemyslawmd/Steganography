using System;
using System.Security.Cryptography;
using System.Text;

namespace Cryptography
{
    class Key
    {
        public static byte[] CreateKey( String password )
        {
            return new Rfc2898DeriveBytes( password, salt, iterations ).GetBytes( keyBytes );
        }

        static readonly byte[] salt = new byte[] { 4, 32, 3, 112, 34, 11, 45, 26, 4, 34 };
        static readonly int iterations = 200;
        static readonly int keyBytes = 32;
    }
}


