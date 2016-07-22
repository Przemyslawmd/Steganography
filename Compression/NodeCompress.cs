using System;

namespace Compression
{
    class NodeCompress : Node
    {
        public NodeCompress( int count, byte byteValue ) : base( true )
        {
            this.byteValue = byteValue;
            this.count = count;
            this.isLeaf = true;
        }

        public NodeCompress( int count, NodeCompress left, NodeCompress right ) : base( false )
        {
            this.count = count;
            this.left = left;
            this.right = right;            
        }        
        
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        
        public NodeCompress Left
        {
            get { return left; }
            set { left = value; }
        }

        public NodeCompress Right
        {
            get { return right; }
            set { right = value; }
        }

        public NodeCompress Parent
        {
            get { return parent; }
            set { parent = value; } 
        }
        
        public void Increment()
        {
            count++;
        }
        
        int count;        
        NodeCompress left;
        NodeCompress right;
        NodeCompress parent; 
    }   
}
