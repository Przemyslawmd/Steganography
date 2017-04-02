﻿
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
        protected const byte Mask1 = 1;
        protected const byte Mask0 = 0;
        protected const int CompressPixel = 6;         // Number of a pixel which stores an information about compresion      
        protected const int DataSizePixel = 6;         // Count of pixels intented to store an information about a size of data                         
        protected const int LastBit = 7;
    }
}
