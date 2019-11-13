
namespace Steganography
{
    class ConstValues
    {
        public static readonly int BitsInByte = 8; 
        public static readonly int CompressionPixelNumber = 8;
        public static readonly int CountOfPixelsForDataSize = 8;
        public static readonly int FirstRow = 0;
        public static readonly int SecondRow = 1;
        public static readonly byte MaskOne = 0x01;
        public static readonly byte[] CoverMark = { 0x6B, 0xA9 };
    }
}

