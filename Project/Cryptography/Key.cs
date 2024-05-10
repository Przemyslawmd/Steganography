
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
            Word[] keyWords = CreateKeysAsWords( initialKey );
            return MovedKeysFromWordIntoByteArray( keyWords );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private Word[] CreateKeysAsWords( byte[] initialKey )
        {
            int keyWordsCount = KeySize * NumOfRounds; 
            Word[] keyWords = new Word[keyWordsCount]; 

            for ( int wordNum = 0; wordNum < 4; wordNum++ )
            {
                keyWords[wordNum] = new Word( initialKey[wordNum * KeySize], initialKey[wordNum * KeySize + 1],
                                              initialKey[wordNum * KeySize + 2], initialKey[wordNum * KeySize + 3] );
            }

            for ( int wordNum = 4; wordNum < keyWordsCount; wordNum++ )
            {
                if ( wordNum % 4 == 0 )
                {
                    Word word = new Word( keyWords[wordNum - 1] );
                    CalculateWord( wordNum / KeySize, word );
                    keyWords[wordNum] = keyWords[wordNum - KeySize].XorOuter( word );
                }
                else
                {
                    keyWords[wordNum] = keyWords[wordNum - 4].XorOuter( keyWords[wordNum - 1] );
                }
            }

            return keyWords;
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

            for ( int word = 0, j = 0; word < KeySize * NumOfRounds; word++ )
            {
                if ( word % KeySize == 0 )
                {
                    j = 0;
                }

                keys[word / KeySize][j++] = words[word].byte_1;
                keys[word / KeySize][j++] = words[word].byte_2;
                keys[word / KeySize][j++] = words[word].byte_3;
                keys[word / KeySize][j++] = words[word].byte_4;                
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

        private readonly byte[] rcon = new byte[10]
        {
            0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36
        };

        private readonly int NumOfRounds = 11;
        private readonly int KeySize = 4;
    }
}

