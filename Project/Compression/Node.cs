using System;

namespace Stegan
{
    class Node
    {
        public Node() { }

        public Node( byte value, bool isLeaf )
        {
            this.ByteValue = value;
        }

        public Node Left { get; set; }

        public Node Right { get; set; }

        public byte ByteValue { get; set; }

        public bool isLeaf()
        {
            return ( Left == null ) && ( Right == null );
        }
    }
}
