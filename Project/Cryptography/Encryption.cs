using System;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Tests")]

namespace Cryptography
{
    class Encryption : BaseCryptography
    {
        public byte[] Encrypt( byte[] dataToEncrypt, String password )
        {
            byte[][] key = Key.CreateKeys( password );
            dataToEncrypt = AlignData( dataToEncrypt );
            byte[,] state = new byte[4, 4];

            int alignment = dataToEncrypt[dataToEncrypt.Length - 1];

            for ( int i = 0; i < dataToEncrypt.Length; i += 16 )
            {
                InputIntoState( dataToEncrypt , i, state );
                EncryptBlockData( state, key );
                StateIntoOutput( dataToEncrypt, i, state );
            }

            Array.Resize( ref dataToEncrypt, dataToEncrypt.Length + 1 );
            dataToEncrypt[dataToEncrypt.Length - 1] = (byte)alignment;
            return dataToEncrypt;                 
        }             
        
        /***********************************************************************************************/
        /* ENCRYPT ONE BLOCK DATA **********************************************************************/

        private void EncryptBlockData( byte[,] state, byte[][] key )
        {
            AddRoundKey( state, key[0] );
                                 
            for ( int round = 1; round < NumOfRounds - 1; round++ )
            {
                SubBytes( state );
                ShiftRows( state );
                MixColumns( state );                
                AddRoundKey( state, key[round] );
            } 
            
            // Last round without MixColumns 
            SubBytes( state );
            ShiftRows( state );            
            AddRoundKey( state, key[NumOfRounds - 1] );
        }

        /* ALIGN DATA *********************************************************************************/
        /* Add additional bytes for data to be divided by block size **********************************/

        private byte[] AlignData( byte[] source )
        {
            const int KeyLenghtInBytes = 16;
            int beginSize = source.Length;
            int alignment = 16 - ( beginSize % ( KeyLenghtInBytes ));            
            Array.Resize( ref source, beginSize + alignment );

            for ( int i = 0; i < alignment - 1; i++ )
                source[beginSize + i] = ( 0x00 );

            source[beginSize + alignment - 1] = (byte)alignment;
            return source;
        }

        /************************************************************************************************/
        /* SHIFT ROWS IN MATRIX STATE *******************************************************************/

        private void ShiftRows( byte[,] state )
        {            
            // Second row
            byte temp = state[1, 0];
            state[1, 0] = state[1, 1];
            state[1, 1] = state[1, 2];
            state[1, 2] = state[1, 3];
            state[1, 3] = temp;

            // Third row - swap two pairs of columns 
            state[2, 0] += state[2, 2];
            state[2, 2] = (byte)( state[2, 0] - state[2, 2] );
            state[2, 0] -= state[2, 2];

            state[2, 1] += state[2, 3];
            state[2, 3] = (byte)( state[2, 1] - state[2, 3] );
            state[2, 1] -= state[2, 3];

            // Fourth row
            temp = state[3, 3];
            state[3, 3] = state[3, 2];
            state[3, 2] = state[3, 1];
            state[3, 1] = state[3, 0];
            state[3, 0] = temp;
        }

        /************************************************************************************************/
        /* SUBBYTES TRANSORMATION ***********************************************************************/

        private void SubBytes( byte[,] state )
        {
            GetGeneralSbox( GetSbox, state );            
        }

        /*************************************************************************************************/
        /* MIX COLUMNS ***********************************************************************************/

        private void MixColumns( byte[,] state )
        {
            byte val0, val1, val2, val3;
            
            for ( int i = 0; i < 4; i++ )
            {
                val0 = state[0, i];
                val1 = state[1, i];
                val2 = state[2, i];
                val3 = state[3, i];
                
                state[0, i] = (byte)( Multiply( val0, 2 ) ^ Multiply( val1, 3 ) ^ val2 ^ val3 );

                state[1, i] = (byte)( val0 ^ Multiply( val1, 2 ) ^ Multiply( val2, 3 ) ^ val3 );

                state[2, i] = (byte)( val0 ^ val1 ^ Multiply( val2, 2 ) ^ Multiply( val3, 3 ));

                state[3, i] = (byte)( Multiply( val0, 3 ) ^ val1 ^ val2 ^ Multiply( val3, 2 ));
            }
        }        
    }
}
