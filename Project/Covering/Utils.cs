
using System.Collections.Generic;


namespace Steganography
{
    class Utils
    {
        public Stack<byte> CreateByteStackFromInteger( int number )  
        {
            var stack = new Stack<byte>();

            for ( int i = 0; i < sizeof( int ); i++ )
            {
                stack.Push( (byte) ( number >> ( i * Constants.BitsInByte )));
            }

            return stack;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        public int CreateIntegerFromByteList( List<byte> byteList )
        {
            int number = 0;
            foreach ( byte byteValue in byteList )
            {
                number <<= Constants.BitsInByte;
                number += byteValue;
            }
            return number;
         }

        /**************************************************************************************/
        /**************************************************************************************/

        public struct BitmapRange
        {
            public BitmapRange( int startX, int stopX, int startY, int stopY )
            {
                StartX = startX;
                StartY = startY;
                StopX = stopX;
                StopY = stopY;
            }

            public int StartX { get; }
            public int StartY { get; }
            public int StopX { get; }
            public int StopY { get; }
        }
    }
}

