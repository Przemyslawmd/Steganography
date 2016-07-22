using System;

namespace Compression
{
    class Node
    {
        public Node() { }

        public Node( Boolean isLeaf )
        {
            this.isLeaf = isLeaf;
        }

        public byte ByteValue
        {
            get { return byteValue; }
            set { byteValue = value; }
        }

        public Boolean Leaf
        {
            get { return isLeaf; }
        }

        protected Boolean isLeaf;
        protected byte byteValue;
    }
}
