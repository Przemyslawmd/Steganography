
namespace Steganography
{
    class BitIterator
    {
        public int Index { get; private set; }

        public void DecrementIndex()
        {
            Index = ( Index == 0 ) ? LastIndex : Index - 1;
        }

        public void IncrementIndex()
        {
            Index = ( Index == LastIndex ) ? 0 : Index + 1;
        }

        public void Reset()
        {
            Index = 0;
        }

        public bool IsInitialIndex()
        {
            return Index == 0;
        }

        private readonly int LastIndex = 7;
    }
}

