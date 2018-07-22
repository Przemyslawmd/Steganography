
namespace Steganography
{
    class BitIterator
    {
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


        public readonly int LastIndex = 7;
    }
}

