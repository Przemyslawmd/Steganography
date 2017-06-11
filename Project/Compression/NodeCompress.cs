using System;

namespace Stegan
{
    class NodeCompress : Node
    {
        public NodeCompress( int count, byte byteValue )
        {
            ByteValue = byteValue;
            Count = count;
        }

        public NodeCompress( int count, NodeCompress left, NodeCompress right )
        {
            Count = count;
            Left = left;
            Right = right;
        }        
        
        public int Count { get; set; }

        public NodeCompress Left { get; set; }

        public NodeCompress Right { get; set; }

        public NodeCompress Parent { get; set; }

        public bool isLeaf()
        {
            return ( Left == null ) && ( Right == null );
        }
    }   
}
