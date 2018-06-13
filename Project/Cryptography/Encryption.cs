
using System;
using System.Collections.Generic;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Tests")]

namespace SteganographyEncryption
{
    class Encryption : BaseCryptography
    {
        public List< byte > Encrypt( List< byte > dataToEncrypt, String password )
        {
            byte[,] state = new byte[4, 4];
            byte[][] key = Key.CreateKeys( password );
            int alignment = AlignData( dataToEncrypt );

            dataToEncrypt.Reverse();
            Stack< byte > stack = new Stack< byte >( dataToEncrypt );
            List< byte > dataEncrypted = new List< byte >();

            for ( int i = 0; i < dataToEncrypt.Count; i += 16 )
            {
                InputIntoState( stack , state );
                EncryptBlockData( state, key );
                StateIntoOutput( dataEncrypted, state );
            }

            dataEncrypted.Add( (byte) alignment );
            return dataEncrypted;
        }             
        
        /**************************************************************************************/
        /**************************************************************************************/

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
            
            SubBytes( state );
            ShiftRows( state );            
            AddRoundKey( state, key[NumOfRounds - 1] );
        }

        /**************************************************************************************/
        /**************************************************************************************/
        // Add additional bytes because data to be encrypted has to be divided by block size (16) 

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

        /**************************************************************************************/
        /**************************************************************************************/

        private void SubBytes( byte[,] state )
        {
            GetGeneralSbox( GetSbox, state );            
        }

        /**************************************************************************************/
        /**************************************************************************************/

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

