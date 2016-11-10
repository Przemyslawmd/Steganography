using System;

namespace Stegan
{
    class BaseCover
    {
        protected int bitNumber;            // Number of bit in a byte 
        protected int byteCount;            // Count of bytes in a stream of data to be covered / uncovered                        
        protected int byteNumber;           // Number of byte in a stream of data to be covered / uncovered
        protected byte byteValue;           
        protected int red;
        protected int green;
        protected int blue;
        protected readonly byte MASK_1 = 1;
        protected readonly byte MASK_0 = 0;
        protected readonly int COMPRESS_PIXEL = 6;          // Number of a pixel which stores an information about compresion      
        protected readonly int DATA_SIZE_PIXEL = 6;         // Count of pixels to store information about a size of data                         
        protected readonly int LAST_BIT = 7;
    }
}
