
namespace Steganography
{
    class BitIterator
    {
        public BitIterator()
        {
            Index = 0;
        }

        public int Index { get; private set; }

        public int DecrementAndGetIndex()
        {
            Index = ( Index == 0 ) ? LastIndex : Index - 1;
            return Index;
        }

        public void IncrementIndex()
        {
            Index = ( Index == LastIndex ) ? 0 : Index + 1;
        }

        public void Reset()
        {
            Index = 0;
        }

        private readonly int LastIndex = 7;
    }
}

