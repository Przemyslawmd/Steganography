
using System;
using System.Collections.Generic;
using Stegan;

namespace Cryptography
{
    class Decryption : BaseCryptography
    {
        public List<byte> Decrypt( List<byte> data, String password )
        {
            byte[][] key = Key.CreateKeys( password );
            byte[,] state = new byte[4, 4];

            int alignment = data[data.Count - 1];
            data.RemoveAt( data.Count - 1 );

            if ( data.Count % 16 != 0 )
                throw new ExceptionEncryption( Messages.MessageCode.ERROR_DECRYPTION_ALIGNMENT );

            data.Reverse();
            Stack<byte> stack = new Stack<byte>( data );
            List<byte> decryptedData = new List<byte>();

            for ( int i = 0; i < data.Count; i += 16 )
            {
                InputIntoState( stack, state );
                DecryptBlockData( state, key );
                StateIntoOutput( decryptedData, state );
            }

            decryptedData.RemoveRange( decryptedData.Count - alignment, alignment );
            return decryptedData;
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
            GetGeneralSbox( GetInvSbox, state );            
        }

        private void InvMixColumns( byte[,] state )
        {
            byte val0, val1, val2, val3;

            for ( int i = 0; i < 4; i++ )
            {
                val0 = state[0, i];
                val1 = state[1, i];
                val2 = state[2, i];
                val3 = state[3, i];

                state[0, i] = (byte)( Multiply( val0, 0x0e ) ^ Multiply( val1, 0x0b ) ^ Multiply( val2, 0x0d ) ^ Multiply( val3, 0x09 ));

                state[1, i] = (byte)( Multiply( val0, 0x09 ) ^ Multiply( val1, 0x0e ) ^ Multiply( val2, 0x0b ) ^ Multiply( val3, 0x0d ));

                state[2, i] = (byte)( Multiply( val0, 0x0d ) ^ Multiply( val1, 0x09 ) ^ Multiply( val2, 0x0e ) ^ Multiply( val3, 0x0b ));

                state[3, i] = (byte)( Multiply( val0, 0x0b ) ^ Multiply( val1, 0x0d ) ^ Multiply( val2, 0x09 ) ^ Multiply( val3, 0x0e ));
            }
        }
    }
}
