
using System;
using System.Collections.Generic;


namespace Steganography.Cryptography
{
    class Decryption
    {
        public List<byte> Decrypt( List<byte> data, String password, ref Result code )
        {
            byte[][] key = new Key().CreateKeys( password );
            byte[,] state = new byte[4, 4];

            int alignment = data[data.Count - 1];
            data.RemoveAt( data.Count - 1 );

            if ( data.Count % 16 != 0 )
            {
                code = Result.ERROR_DECRYPTION_ALIGNMENT;
                return null;
            }

            data.Reverse();
            var stack = new Stack<byte>( data );
            var decryptedData = new List<byte>();

            for ( int i = 0; i < data.Count; i += 16 )
            {
                utils.InputIntoState( stack, state );
                DecryptBlockData( state, key );
                utils.StateIntoOutput( decryptedData, state );
            }

            decryptedData.RemoveRange( decryptedData.Count - alignment, alignment );
            return decryptedData;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void DecryptBlockData( byte[,] state, byte[][] key )
        {
            utils.AddRoundKey( state, key[NumOfRounds - 1] );

            for ( int round = NumOfRounds - 2; round > 0; round-- )
            {
                InvShiftRows( state );
                InvSubBytes( state );
                utils.AddRoundKey( state, key[round] );
                InvMixColumns( state );
            }

            InvShiftRows( state );
            InvSubBytes( state );
            utils.AddRoundKey( state, key[0] );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void InvShiftRows( byte[,] state )
        {
            byte temp = state[1, 3];
            state[1, 3] = state[1, 2];
            state[1, 2] = state[1, 1];
            state[1, 1] = state[1, 0];
            state[1, 0] = temp;

            state[2, 0] += state[2, 2];
            state[2, 2] = (byte) (state[2, 0] - state[2, 2]);
            state[2, 0] -= state[2, 2];

            state[2, 1] += state[2, 3];
            state[2, 3] = (byte) (state[2, 1] - state[2, 3]);
            state[2, 1] -= state[2, 3];

            temp = state[3, 0];
            state[3, 0] = state[3, 1];
            state[3, 1] = state[3, 2];
            state[3, 2] = state[3, 3];
            state[3, 3] = temp;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void InvSubBytes( byte[,] state )
        {
            utils.GetGeneralSbox( Sbox.GetInvSbox, state );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void InvMixColumns( byte[,] state )
        {
            byte byte_0, byte_1, byte_2, byte_3;

            for ( int i = 0; i < 4; i++ )
            {
                byte_0 = state[0, i];
                byte_1 = state[1, i];
                byte_2 = state[2, i];
                byte_3 = state[3, i];

                state[0, i] = (byte)( utils.Multiply( byte_0, 0x0e ) ^ utils.Multiply( byte_1, 0x0b ) ^ 
                                      utils.Multiply( byte_2, 0x0d ) ^ utils.Multiply( byte_3, 0x09 ));

                state[1, i] = (byte)( utils.Multiply( byte_0, 0x09 ) ^ utils.Multiply( byte_1, 0x0e ) ^
                                      utils.Multiply( byte_2, 0x0b ) ^ utils.Multiply( byte_3, 0x0d ));

                state[2, i] = (byte)( utils.Multiply( byte_0, 0x0d ) ^ utils.Multiply( byte_1, 0x09 ) ^
                                      utils.Multiply( byte_2, 0x0e ) ^ utils.Multiply( byte_3, 0x0b ));

                state[3, i] = (byte)( utils.Multiply( byte_0, 0x0b ) ^ utils.Multiply( byte_1, 0x0d ) ^ 
                                      utils.Multiply( byte_2, 0x09 ) ^ utils.Multiply( byte_3, 0x0e ));
            }
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private readonly Utils utils = new Utils();
        private readonly int NumOfRounds = 11;
    }
}

