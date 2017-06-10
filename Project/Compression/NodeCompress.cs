using System;

namespace Stegan
{
    class NodeCompress : Node
    {
        public NodeCompress( int count, byte byteValue ) : base( true )
        {
            this.ByteValue = byteValue;
            this.Count = count;
            this.Leaf = true;
        }

        public NodeCompress( int count, NodeCompress left, NodeCompress right ) : base( false )
        {
            this.Count = count;
            this.Left = left;
            this.Right = right;
        }        
        
        public int Count { get; set; }

        public NodeCompress Left { get; set; }

        public NodeCompress Right { get; set; }

        public NodeCompress Parent { get; set; }
    }   
}
