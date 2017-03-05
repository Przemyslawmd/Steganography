using System;
using System.Security.Cryptography;
using System.Text;

namespace Cryptography
{
    class Key
    {
        public static byte[][] CreateKeys( String password )
        {
            const int Iterations = 200;
            byte[] Salt = new byte[] { 4, 32, 3, 112, 34, 11, 45, 26, 4, 34 };
            byte[] basicKey = new Rfc2898DeriveBytes( password, Salt, Iterations ).GetBytes( 16 );

            return ExpandKey( basicKey );
        }

        /**************************************************************************************************/
        /**************************************************************************************************/
        
        private static byte[][] ExpandKey( byte[] initialKey )
        {            
            Word[] words = new Word[NumOfWords];            
            Word tempWord;                                   
            
            // At the beginning keys are calculated as parts of four-bytes words
                        
            // Key for the first round
            for ( int word = 0; word < 4; word++ )
                words[word] = new Word( initialKey[word * 4], initialKey[word * 4 + 1], 
                                        initialKey[word * 4 + 2], initialKey[word * 4 + 3] );

            // Keys for the rest rounds
            for ( int word = 4; word < NumOfWords; word++ )
            {
                // An action for each part of expanded key to calculate its first word
                if ( word % 4 == 0 )
                {
                    tempWord = new Word( words[word - 1] );
                    CalculateTemporaryWord( word / 4, tempWord );
                    words[word] = words[word - 4].XorOuter( tempWord );
                }
                else
                    words[word] = words[word - 4].XorOuter( words[word - 1] );                
            }
            
            // Create two-dimensional jagged array and move there keys
            byte[][] roundKeys = new byte[NumOfRounds][];

            for ( int i = 0; i < NumOfRounds; i++ )
                roundKeys[i] = new byte[16]; 

            for ( int word = 0, j = 0; word < NumOfWords; word++ )
            {
                if ( word % 4 == 0 )
                    j = 0;

                roundKeys[word / 4][j++] = words[word].value1;
                roundKeys[word / 4][j++] = words[word].value2;
                roundKeys[word / 4][j++] = words[word].value3;
                roundKeys[word / 4][j++] = words[word].value4;                
            }

            return roundKeys;
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

        // Number of rounds with initial round
        static readonly int NumOfRounds = 11;
        // Number of words of keys for all rounds 
        static readonly int NumOfWords = 44;
    }
}


