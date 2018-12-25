
using System.Collections.Generic;

namespace Steganography
{
    class Containers
    {
        public Stack< byte > CreateByteStackFromInteger( int number )  
        {
            Stack< byte > stack = new Stack< byte >();

            for ( int i = 0; i < 4; i++ )
            {
                stack.Push( (byte) ( number >> ( i * BitsInByte )));
            }

            return stack;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        public int CreateIntegerFromByteList( List< byte > byteList )  
        {
            int number = 0;
            
            foreach ( byte byteValue in byteList )
            {
                number <<= BitsInByte;
                number += byteValue;
            }
            return number;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private readonly int BitsInByte = 8;
    }
}

