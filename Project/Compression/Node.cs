using System;

namespace Stegan
{
    class Node
    {
        public Node() { }

        public Node( bool isLeaf )
        {
            this.Leaf = isLeaf;
        }

        public Node( byte value, bool isLeaf )
        {
            this.ByteValue = value;
            this.Leaf = isLeaf;
        }

        public Node Left { get; set; }

        public Node Right { get; set; }

        public byte ByteValue { get; set; }

        public Boolean Leaf { get; protected set; }
    }
}
