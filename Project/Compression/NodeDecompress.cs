using System;

namespace Compression
{
    class NodeDecompress : Node
    {
        public NodeDecompress( byte byteValue, Boolean isLeaf ) : base( isLeaf )
        {
            this.byteValue = byteValue;            
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

        NodeDecompress left;
        NodeDecompress right;       
    }
}
