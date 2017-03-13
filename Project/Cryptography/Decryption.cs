using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptography
{
    class Decryption : BaseCryptography
    {
        public void Decrypt( byte[] dataToDecrypt, String password )
        {
            byte[][] key = Key.CreateKeys( password );
            byte[,] state = new byte[4, 4];

            if ( dataToDecrypt.Length % 16 != 0 )
                return;

            for ( int i = 0; i < dataToDecrypt.Length; i += 16 )
            {
                InputIntoState( dataToDecrypt, i, state );
                DecryptBlockData( state, key );
                StateIntoOutput( dataToDecrypt, i, state );
            }            
        }

        /***********************************************************************************************/
        /* DECRYPT ONE BLOCK DATA **********************************************************************/

        private void DecryptBlockData( byte[,] state, byte[][] key )
        {
            AddRoundKey( state, key[NumOfRounds - 1] );

            for ( int round = NumOfRounds - 2; round > 0; round-- )
            {
                InvShiftRows( state );
                InvSubBytes( state );
                AddRoundKey( state, key[round] );
                InvMixColumns( state );                
            }

            // Last round without InvMixColumns 
            InvShiftRows( state );
            InvSubBytes( state );
            AddRoundKey( state, key[0] );
        }

        /***************************************************************************************************************/
        /* INVERSE SHIFT ROWS ******************************************************************************************/

        private void InvShiftRows( byte[,] state )
        {
            // Second row
            byte temp = state[1, 3];
            state[1, 3] = state[1, 2];
            state[1, 2] = state[1, 1];
            state[1, 1] = state[1, 0];
            state[1, 0] = temp;

            // Third row - swap two pairs of columns 
            state[2, 0] += state[2, 2];
            state[2, 2] = (byte)(state[2, 0] - state[2, 2]);
            state[2, 0] -= state[2, 2];

            state[2, 1] += state[2, 3];
            state[2, 3] = (byte)(state[2, 1] - state[2, 3]);
            state[2, 1] -= state[2, 3];

            // Fourth row
            temp = state[3, 0];
            state[3, 0] = state[3, 1];
            state[3, 1] = state[3, 2];
            state[3, 2] = state[3, 3];
            state[3, 3] = temp;
        }

        /************************************************************************************************/
        /* INVERSE SUBBYTES TRANSORMATION ***************************************************************/

        private void InvSubBytes( byte[,] state )
        {
            GetBox( BaseCryptography.GetInvSbox, state );            
        }

        private void InvMixColumns( byte[,] state )
        {

        }
    }
}
