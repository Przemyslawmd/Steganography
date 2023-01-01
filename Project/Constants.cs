
namespace Steganography
{
    public static class Constants
    {
        public static readonly int BitsInByte = 8;
        public static readonly int CompressionPixel = 8;
        public static readonly int CountOfPixelsForDataSize = 8;
        public static readonly byte MaskOne = 0x01;
        public static readonly byte[] CoverMark = { 0x6B, 0xA9 };
    }
}

