
namespace Stegan
{
    abstract class BaseCover
    {
        protected int bitNumber;                        // Number of bit in a byte
        protected int byteCount;                        // Count of bytes in a stream of data to be covered / uncovered
        protected byte byteValue;           
        protected const byte MaskOne = 0x01;
        protected const int CompressPixel = 6;          // Number of a pixel which stores an information about compresion
        protected const int DataSizePixel = 6;          // Count of pixels intented to store a size of data
        protected const int LastBit = 7;
    }
}
