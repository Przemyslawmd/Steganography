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
            Word[] expandedWord = new Word[keyWordExpanded];            
            Word tempWord;

            // Key for the first round
            for ( int i = 0; i < 4; i++ )
                expandedWord[i] = new Word( key[i * 4], key[i * 4 + 1], key[i * 4 + 2], key[i * 4 + 3] );                     

            // Keys for the rest rounds
            for ( int i = 4; i < 44; i++ )
            {
                // Copy last word of a previous key into a temporary word
                tempWord = new Word( expandedWord[i - 1] );

                // An action for each part of expanded key to calculate its first word
                if ( i % 4 == 0 )
                    CalculateTemporaryWord( i / 4 - 1, tempWord );

                expandedWord[i] = expandedWord[i - 4].XorOuter( tempWord );                
            }

            // Temporary solution 
            byte[] expandedKey = new byte[keyBytesExpanded];

            for ( int i = 0, j = 0; i < 44; i++ )
            {
                expandedKey[j++] = expandedWord[i].value1;
                expandedKey[j++] = expandedWord[i].value2;
                expandedKey[j++] = expandedWord[i].value3;
                expandedKey[j++] = expandedWord[i].value4;
            }
            
            return expandedKey;
        }


        /***************************************************************************************************/
        /***************************************************************************************************/

        private static void CalculateTemporaryWord( int i, Word word )
        {
            word.Rotate();
            word.SubByte();            
            word.XorInner( new Word( rcon[i], 0, 0, 0 ) );
        }            

        /*****************************************************************************************************/
        /*****************************************************************************************************/

        static byte[] rcon = new byte[10]
        {
            0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36
        };

        static readonly byte[] salt = new byte[] { 4, 32, 3, 112, 34, 11, 45, 26, 4, 34 };
        static readonly int iterations = 200;
        static readonly int keyBytes = 16;
        static readonly int keyBytesExpanded = 176;
        static readonly int keyWordExpanded = 44;
    }
}


