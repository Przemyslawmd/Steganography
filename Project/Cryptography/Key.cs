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
            for ( int indexWord = 0; indexWord < 4; indexWord++ )
                expandedWord[indexWord] = new Word( key[indexWord * 4], key[indexWord * 4 + 1], key[indexWord * 4 + 2], key[indexWord * 4 + 3] );

            // Keys for the rest rounds
            for ( int indexWord = 4; indexWord < 44; indexWord++ )
            {
                // An action for each part of expanded key to calculate its first word
                if ( indexWord % 4 == 0 )
                {
                    tempWord = new Word( expandedWord[indexWord - 1] );
                    CalculateTemporaryWord( indexWord / 4, tempWord );
                    expandedWord[indexWord] = expandedWord[indexWord - 4].XorOuter( tempWord );
                }
                else
                    expandedWord[indexWord] = expandedWord[indexWord - 4].XorOuter( expandedWord[indexWord - 1] );                
            }
            
            byte[] expandedKey = new byte[keyBytesExpanded];

            for ( int indexWord = 0, j = 0; indexWord < 44; indexWord++ )
            {
                expandedKey[j++] = expandedWord[indexWord].value1;
                expandedKey[j++] = expandedWord[indexWord].value2;
                expandedKey[j++] = expandedWord[indexWord].value3;
                expandedKey[j++] = expandedWord[indexWord].value4;
            }
            
            return expandedKey;
        }

        /***************************************************************************************************/
        /***************************************************************************************************/

        private static void CalculateTemporaryWord( int i, Word word )
        {
            word.Rotate();
            word.SubByte();            
            word.XorInner( new Word( rcon[i - 1], 0, 0, 0 ) );
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


