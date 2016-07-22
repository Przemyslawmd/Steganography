using System;
using System.Text;

namespace Compression
{
    class NodeDecompress
    {
        public NodeDecompress( byte byteValue, Boolean isLeaf )
        {
            this.byteValue = byteValue;
            this.isLeaf = isLeaf;
        }

        public byte ByteValue
        {
            get { return byteValue; }            
        }

        public NodeDecompress Left
        {
            get { return left; }
            set { left = value; }
        }

        public NodeDecompress Right
        {
            get { return right; }
            set { right = value; }
        }

        public Boolean Leaf
        {
            get { return isLeaf; }
        }

        byte byteValue;        
        NodeDecompress left;
        NodeDecompress right;
        Boolean isLeaf;
    }
}
