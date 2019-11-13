
using System;
using System.Security.Cryptography;

namespace Steganography.Cryptography
{
    class Key
    {
        public byte[][] CreateKeys( String password )
        {
            const int Iterations = 200;
            byte[] salt = new byte[] { 4, 32, 3, 112, 34, 11, 45, 26, 4, 34 };
            byte[] initialKey = new Rfc2898DeriveBytes( password, salt, Iterations ).GetBytes( 16 );
            
            return ExpandKey( initialKey );
        }

        /**************************************************************************************/
        /**************************************************************************************/
        
        private byte[][] ExpandKey( byte[] initialKey )
        {
            Word[] words = createKeysAsWords( initialKey );
            return MovedKeysFromWordIntoByteArray( words );
        }

        /**************************************************************************************/
        /**************************************************************************************/


        private Word[] createKeysAsWords( byte[] initialKey )
        {
            int wordsCount = WordsInKey * NumOfRounds; 
            Word[] words = new Word[wordsCount]; 

            for ( int wordNum = 0; wordNum < 4; wordNum++ )
            {
                words[wordNum] = new Word( initialKey[wordNum * WordsInKey], initialKey[wordNum * WordsInKey + 1],
                                           initialKey[wordNum * WordsInKey + 2], initialKey[wordNum * WordsInKey + 3] );
            }

            Word word;
            for ( int wordNum = 4; wordNum < wordsCount; wordNum++ )
            {
                if ( wordNum % 4 == 0 )
                {
                    word = new Word( words[wordNum - 1] );
                    CalculateWord( wordNum / WordsInKey, word );
                    words[wordNum] = words[wordNum - WordsInKey].XorOuter( word );
                }
                else
                {
                    words[wordNum] = words[wordNum - 4].XorOuter( words[wordNum - 1] );
                }
            }

            return words;
        }
        
        /**************************************************************************************/
        /**************************************************************************************/

        private byte[][] MovedKeysFromWordIntoByteArray( Word[] words )
        {
            byte[][] keys = new byte[NumOfRounds][];

            for ( int i = 0; i < NumOfRounds; i++ )
            {
                keys[i] = new byte[16];
            }

            for ( int word = 0, j = 0; word < WordsInKey * NumOfRounds; word++ )
            {
                if ( word % WordsInKey == 0 )
                {
                    j = 0;
                }

                keys[word / WordsInKey][j++] = words[word].byte_1;
                keys[word / WordsInKey][j++] = words[word].byte_2;
                keys[word / WordsInKey][j++] = words[word].byte_3;
                keys[word / WordsInKey][j++] = words[word].byte_4;                
            }

            return keys;
        }
        
        /**************************************************************************************/
        /**************************************************************************************/

        private void CalculateWord( int i, Word word )
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

