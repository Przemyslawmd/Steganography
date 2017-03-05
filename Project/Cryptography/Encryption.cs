using System;
using System.Collections.Generic;
using System.Text;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Tests")]

namespace Cryptography
{
    class Encryption : BaseCryptography
    {
        public void Encrypt( byte[] source, String password )
        {
            byte[][] key = Key.CreateKeys( password );
            byte[] data = AlignData( source );
            byte[,] state = new byte[4, 4];

            for ( int i = 0; i < data.Length; i += 16 )
            {
                InputIntoState( data , i, state );
                EncryptBlockData( state, key );
                StateIntoOutput( data, i, state );
            }                 
        }

        /*************************************************************************************/
        /* CHANGE STREAM INTO TWO DIMENSION ARRAY ********************************************/
        /* Two dimension array seems to be more natural for AES functions than stream ********/
        /* Additional this array is "after transpontation" ***********************************/ 

        private void InputIntoState( byte[] data, int index, byte[,] state )
        {
            for ( int i = 0; i < 4; i++ )
            {
                for ( int j = 0; j < 4; j++ )
                    state[i, j] = data[index + i + j * 4];        
            }            
        }

        /**************************************************************************************/
        /* CHANGE STATE INTO STREAM ***********************************************************/

        private void StateIntoOutput( byte[] data, int index, byte[,] state )
        {
            for ( int i = 0; i < 4; i++ )
            {
                for ( int j = 0; j < 4; j++ )
                    data[index + i * 4 + j] = state[j, i];
            }  
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
            for ( int i = 0; i < 4; i++ )
            {
                for ( int j = 0; j < 4; j++ )
                    state[i, j] = BaseCryptography.GetSbox( state[i, j] );
            }
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
                
                state[0, i] = (byte)( MultiplyBy2( val0 ) ^ MultiplyBy3( val1 ) ^ val2 ^ val3 );

                state[1, i] = (byte)( val0 ^ MultiplyBy2( val1 ) ^ MultiplyBy3( val2 ) ^ val3 );

                state[2, i] = (byte)( val0 ^ val1 ^ MultiplyBy2( val2 ) ^ MultiplyBy3( val3 ));

                state[3, i] = (byte)( MultiplyBy3( val0 ) ^ val1 ^ val2 ^ MultiplyBy2( val3 ));
            }
        }

        /*************************************************************************************************/
        /* MULTIPLY BY 2 IN GF(2^8) **********************************************************************/

        private byte MultiplyBy2( byte data )
        {
            bool flag = (( data & 0x80 ) != 0x00 ) ? true : false;
            data <<= 1;

            if ( flag )
                data ^= 0x1b;
            return data;
        }

        /*************************************************************************************************/
        /* MULTIPLY BY 3 IN GF(2^8) **********************************************************************/

        private byte MultiplyBy3( byte data )
        {
            byte temp = MultiplyBy2( data );
            data ^= temp;
            return data;
        }        
    }
}
