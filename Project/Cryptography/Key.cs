
using System;
using System.Security.Cryptography;

namespace SteganographyEncryption
{
    class Key
    {
        public byte[][] CreateKeys( String password )
        {
            const int Iterations = 200;
            byte[] Salt = new byte[] { 4, 32, 3, 112, 34, 11, 45, 26, 4, 34 };
            byte[] basicKey = new Rfc2898DeriveBytes( password, Salt, Iterations ).GetBytes( 16 );

            return ExpandKey( basicKey );
        }

        /**************************************************************************************/
        /**************************************************************************************/
        
        private byte[][] ExpandKey( byte[] initialKey )
        {
            int numOfAllWords = WordsInKey * NumOfRounds; 
            Word[] words = new Word[numOfAllWords];            
            Word tempWord;

            createKeysForFirstRound( words, initialKey );
            
            // Keys for the rest rounds
            for ( int word = 4; word < numOfAllWords; word++ )
            {
                // An action for each part of expanded key to calculate its first word
                if ( word % 4 == 0 )
                {
                    tempWord = new Word( words[word - 1] );
                    CalculateTemporaryWord( word / WordsInKey, tempWord );
                    words[word] = words[word - WordsInKey].XorOuter( tempWord );
                }
                else
                {
                    words[word] = words[word - 4].XorOuter( words[word - 1] );
                }
            }
            
            // Create two-dimensional jagged array and move there keys
            byte[][] roundKeys = new byte[NumOfRounds][];

            for ( int i = 0; i < NumOfRounds; i++ )
            {
                roundKeys[i] = new byte[16];
            }

            for ( int word = 0, j = 0; word < numOfAllWords; word++ )
            {
                if ( word % WordsInKey == 0 )
                {
                    j = 0;
                }

                roundKeys[word / WordsInKey][j++] = words[word].value1;
                roundKeys[word / WordsInKey][j++] = words[word].value2;
                roundKeys[word / WordsInKey][j++] = words[word].value3;
                roundKeys[word / WordsInKey][j++] = words[word].value4;                
            }

            return roundKeys;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void createKeysForFirstRound( Word[] words, byte[] initialKey )
        {
            for ( int word = 0; word < 4; word++ )
            {
                words[word] = new Word( initialKey[word * WordsInKey], initialKey[word * WordsInKey + 1],
                                        initialKey[word * WordsInKey + 2], initialKey[word * WordsInKey + 3] );
            }
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void CalculateTemporaryWord( int i, Word word )
        {
            word.Rotate();
            word.SubByte();            
            word.XorInner( new Word( rcon[i - 1], 0, 0, 0 ) );
        }              

        /**************************************************************************************/
        /**************************************************************************************/

        private byte[] rcon = new byte[10]
        {
            0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36
        };


        private readonly int NumOfRounds = 11;
        private readonly int WordsInKey = 4;
    }
}

