
namespace Steganography
{
    class BitIterator
    {
        public BitIterator()
        {
            Index = 0;
        }

        public int Index { get; set; }

        public int DecrementAndGetIndex()
        {
            Index = ( Index == 0 ) ? LastIndex : Index - 1;
            return Index;
        }

        public void Reset()
        {
            Index = 0;
        }

        public readonly int LastIndex = 7;
    }
}

