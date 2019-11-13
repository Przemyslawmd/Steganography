
using System;
using System.Collections.Generic;

namespace Steganography.Cryptography
{ 
    class Utils
    {
        public void InputIntoState( Stack< byte > stack, byte[,] state )
        {
            for ( int i = 0; i < stateArraySize; i++ )
            {
                for ( int j = 0; j < stateArraySize; j++ )
                {
                    state[j, i] = stack.Pop();
                }
            }
        }

        /**************************************************************************************/
        /**************************************************************************************/

        public void StateIntoOutput( List< byte > output, byte[,] state )
        {
            for ( int i = 0; i < stateArraySize; i++ )
            {
                for ( int j = 0; j < stateArraySize; j++ )
                {
                    output.Add( state[j, i] );
                }
            }
        }

        /**************************************************************************************/
        /**************************************************************************************/
        
        public void AddRoundKey( byte[,] state, byte[] key )
        {            
            for ( int i = 0; i < stateArraySize; i++ )
            {
                for ( int j = 0; j < stateArraySize; j++ )
                {
                    state[i, j] ^= key[i + j * stateArraySize];
                }
            }           
        }

        /**************************************************************************************/
        /**************************************************************************************/

        public void GetGeneralSbox( Func< byte, byte > func, byte[,] state )
        {
            for ( int i = 0; i < stateArraySize; i++ )
            {
                for ( int j = 0; j < stateArraySize; j++ )
                {
                    state[i, j] = func.Invoke( state[i, j] );
                }
            }            
        }

        /**************************************************************************************/
        /**************************************************************************************/

        public byte Multiply( byte a, byte b )
        {
            byte result = 0; 
                        
            while ( b > 0 )
            {
                if (( b & 0x01 ) != 0x00 )
                {
                    result ^= a;
                }

                if (( a & 0x80 ) != 0x00 )
                {
                    a = (byte) ( a << 1 ^ 0x11b );
                }
                else
                {
                    a <<= 1;
                }
                
                b >>= 1;
            }
            return result;            
        }        

        /**************************************************************************************/
        /**************************************************************************************/

        private readonly int stateArraySize = 4; 
    }
}

