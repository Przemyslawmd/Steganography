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

        /**************************************************************************************************/
        /**************************************************************************************************/

        private static byte[] ExpandKey( byte[] key )
        {
            byte[] expandedKey = new byte[keyBytesExpanded];
            byte[] tempWord = new byte[4];

            Array.Copy( key, 0, expandedKey, 0, keyBytes );

            // First four word ( 16 bytes ) are filled, take an action for the rest 
            for ( int i = 16; i < keyBytesExpanded; i++ )
            {
                // Copy last word of a previous key into a temporary word
                Array.Copy( expandedKey, i - 4, tempWord, 0, 4 );

                //if ( i % 16 == 0 )


            }


            return expandedKey;
        }

        /***************************************************************************************************/
        /***************************************************************************************************/

        private static void RotateWord( byte[] word )
        {
            byte temp = word[0];
            word[0] = word[1];
            word[1] = word[2];
            word[2] = word[3];
            word[3] = temp;
        }

        static readonly byte[] salt = new byte[] { 4, 32, 3, 112, 34, 11, 45, 26, 4, 34 };
        static readonly int iterations = 200;
        static readonly int keyBytes = 16;
        static readonly int keyBytesExpanded = 176;
    }
}


