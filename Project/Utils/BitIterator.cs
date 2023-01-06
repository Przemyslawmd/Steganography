
namespace Steganography
{
    class BitIterator
    {
        public int Index { get; private set; }

        public void Decrement()
        {
            Index = ( Index == 0 ) ? LastIndex : Index - 1;
        }

        public void Increment()
        {
            Index = ( Index == LastIndex ) ? 0 : Index + 1;
        }

        public void Reset()
        {
            Index = 0;
        }

        public bool IsInitial()
        {
            return Index == 0;
        }

        private readonly int LastIndex = 7;
    }
}

