
namespace Steganography
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

        public Node Left { get; set; }

        public Node Right { get; set; }

        public byte ByteValue { get; set; }

        public bool IsLeaf()
        {
            return ( Left is null ) && ( Right is null );
        }
    }
}

