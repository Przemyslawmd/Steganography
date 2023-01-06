
namespace Steganography.Huffman
{ 
    class Node
    {
        public Node( Node left, Node right )
        {
            Left = left;
            Right = right;
        }

        public Node( byte value )
        {
            ByteValue = value;
        }

        public Node( int count, byte byteValue )
        {
            ByteValue = byteValue;
            Count = count;
        }

        public Node( int count, Node left, Node right )
        {
            Left = left;
            Right = right;
            Count = count;
        }

        public Node Left { get; set; }

        public Node Right { get; set; }

        public Node Parent { private get; set; }

        public byte ByteValue { get; set; }

        public int Count { get; set; }

        public bool IsLeaf()
        {
            return ( Left is null ) && ( Right is null );
        }
    }
}

