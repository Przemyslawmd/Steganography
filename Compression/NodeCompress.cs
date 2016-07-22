using System;
using System.Text;

namespace Compression
{
    class NodeCompress
    {
        public NodeCompress( int count, byte byteValue )
        {
            this.byteValue = byteValue;
            this.count = count;
            this.isLeaf = true;
        }

        public NodeCompress( int count, NodeCompress left, NodeCompress right )
        {
            this.count = count;
            this.left = left;
            this.right = right;
            this.isLeaf = false;
        }

        public Boolean IsLeaf
        {
            get { return isLeaf; }
        }
        
        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public byte ByteValue
        {
            get { return byteValue; }
            set { byteValue = value; }
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

        byte byteValue;
        int count;
        bool isLeaf;
        NodeCompress left;
        NodeCompress right;
        NodeCompress parent; 
    }   
}
