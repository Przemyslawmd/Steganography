using System;

namespace Stegan
{
    class Node
    {
        public Node() { }

        public Node( bool isLeaf )
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

        protected bool isLeaf;
        protected byte byteValue;
    }
}
