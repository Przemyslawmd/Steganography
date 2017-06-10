using System;

namespace Stegan
{
    class NodeDecompress : Node
    {
        public NodeDecompress( byte byteValue, bool isLeaf ) : base( isLeaf )
        {
            this.ByteValue = byteValue;
        }        

        public NodeDecompress Left { get; set; }

        public NodeDecompress Right { get; set; }
    }
}

