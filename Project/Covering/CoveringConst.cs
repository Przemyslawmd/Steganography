
namespace Steganography
{
    class CoveringConst
    {
        public readonly byte MaskOne = 0x01;
        public readonly int NumberCompressPixel = 8;
        public readonly int PixelCountForDataSize = 8;
        public readonly int FirstRow = 0;
        public readonly int SecondRow = 1;
        public readonly byte[] CoverMark = { 0x6B, 0xA9 };
    }
}

