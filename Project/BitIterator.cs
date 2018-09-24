
namespace Steganography
{
    class BitIterator
    {
        public BitIterator()
        {
            Index = InitialIndex;
        }

        public BitIterator( int index )
        {
            Index = index;
        }

        public int Index { get; set; }


        public void SetLastIndex()
        {
            Index = LastIndex;
        }


        public int GetAndDecrementIndex()
        {
            return Index--;
        }

        public void Reset()
        {
            Index = InitialIndex;
        }

        private readonly int InitialIndex = -1;
        public readonly int LastIndex = 7;
    }
}

