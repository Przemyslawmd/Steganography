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

        public NodeCompress( int count, Node left, Node right )
        {
            Count = count;
            Left = left;
            Right = right;
        }        
        
        public int Count { get; set; }

        public NodeCompress Parent { get; set; }
    }   
}
