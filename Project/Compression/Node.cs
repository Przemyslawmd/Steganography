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

        public byte ByteValue { get; set; }

        public Boolean Leaf { get; protected set; }
    }
}
