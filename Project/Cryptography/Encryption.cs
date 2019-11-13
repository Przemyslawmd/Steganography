
using System;
using System.Collections.Generic;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Tests")]

namespace SteganographyEncryption
{
    class Encryption
    {
        public List< byte > Encrypt( List< byte > dataToEncrypt, String password )
        {
            byte[,] state = new byte[4, 4];
            byte[][] key = new Key().CreateKeys( password );
            int alignment = AlignData( dataToEncrypt );

            dataToEncrypt.Reverse();
            var stack = new Stack< byte >( dataToEncrypt );
            var dataEncrypted = new List< byte >();

            for ( int i = 0; i < dataToEncrypt.Count; i += 16 )
            {
                utils.InputIntoState( stack , state );
                EncryptBlockData( state, key );
                utils.StateIntoOutput( dataEncrypted, state );
            }

            dataEncrypted.Add( (byte) alignment );
            return dataEncrypted;
        }             
        
        /**************************************************************************************/
        /**************************************************************************************/

        private void EncryptBlockData( byte[,] state, byte[][] key )
        {
            utils.AddRoundKey( state, key[0] );
                                 
            for ( int round = 1; round < NumOfRounds - 1; round++ )
            {
                SubBytes( state );
                ShiftRows( state );
                MixColumns( state );                
                utils.AddRoundKey( state, key[round] );
            } 
            
            SubBytes( state );
            ShiftRows( state );            
            utils.AddRoundKey( state, key[NumOfRounds - 1] );
        }

        /**************************************************************************************/
        /**************************************************************************************/
        // Add additional bytes because data to be encrypted must be divided by block size (16) 

        private int AlignData( List< byte > source )
        {
            int alignment = 16 - ( source.Count % 16 );

            for ( int i = 0; i < alignment - 1; i++ )
            {
                source.Add( 0x00 );
            }

            source.Add( (byte) alignment );
            return alignment;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ShiftRows( byte[,] state )
        {            
            byte temp = state[1, 0];
            state[1, 0] = state[1, 1];
            state[1, 1] = state[1, 2];
            state[1, 2] = state[1, 3];
            state[1, 3] = temp;

            state[2, 0] += state[2, 2];
            state[2, 2] = (byte) ( state[2, 0] - state[2, 2] );
            state[2, 0] -= state[2, 2];

            state[2, 1] += state[2, 3];
            state[2, 3] = (byte) ( state[2, 1] - state[2, 3] );
            state[2, 1] -= state[2, 3];

            temp = state[3, 3];
            state[3, 3] = state[3, 2];
            state[3, 2] = state[3, 1];
            state[3, 1] = state[3, 0];
            state[3, 0] = temp;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void SubBytes( byte[,] state )
        {
            utils.GetGeneralSbox( Sbox.GetSbox, state );            
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void MixColumns( byte[,] state )
        {
            byte byte_0, byte_1, byte_2, byte_3;
            
            for ( int i = 0; i < 4; i++ )
            {
                byte_0 = state[0, i];
                byte_1 = state[1, i];
                byte_2 = state[2, i];
                byte_3 = state[3, i];
                
                state[0, i] = (byte)( utils.Multiply( byte_0, 2 ) ^ utils.Multiply( byte_1, 3 ) ^ byte_2 ^ byte_3 );

                state[1, i] = (byte)( byte_0 ^ utils.Multiply( byte_1, 2 ) ^ utils.Multiply( byte_2, 3 ) ^ byte_3 );

                state[2, i] = (byte)( byte_0 ^ byte_1 ^ utils.Multiply( byte_2, 2 ) ^ utils.Multiply( byte_3, 3 ));

                state[3, i] = (byte)( utils.Multiply( byte_0, 3 ) ^ byte_1 ^ byte_2 ^ utils.Multiply( byte_3, 2 ));
            }
        }
        
        /**************************************************************************************/
        /**************************************************************************************/

        private readonly Utils utils = new Utils();
        private readonly int NumOfRounds = 11;
    }
}

