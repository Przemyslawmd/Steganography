
namespace Steganography
{
    class CoveringConst
    {
        public readonly byte MaskOne = 0x01;
        public readonly int CompressPixel = 6;          // Number of a pixel which stores an information about compresion
        public readonly int DataSizePixel = 6;          // Count of pixels intented to store a size of data
        public readonly int SecondPictureRow = 1;
        public readonly byte[] CoverMark = { 0xA3, 0xF6 };
    }
}

