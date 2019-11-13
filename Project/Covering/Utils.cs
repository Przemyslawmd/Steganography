
using System.Collections.Generic;

namespace Steganography
{
    class Utils
    {
        public Stack< byte > CreateByteStackFromInteger( int number )  
        {
            var stack = new Stack< byte >();

            for ( int i = 0; i < sizeof( int ); i++ )
            {
                stack.Push( (byte) ( number >> ( i * ConstValues.BitsInByte )));
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
                number <<= ConstValues.BitsInByte;
                number += byteValue;
            }
            return number;
        }
    }
}

