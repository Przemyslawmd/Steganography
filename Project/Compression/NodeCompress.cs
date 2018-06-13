
namespace SteganographyCompression
{
    class NodeCompress : Node
    {
        public NodeCompress( int count, byte byteValue ) : base( byteValue )
        {
            Count = count;
        }

        public NodeCompress( int count, Node left, Node right ) : base( left, right )
        {
            Count = count;
        }        
        
        public int Count { get; set; }

        public NodeCompress Parent { private get; set; }
    }   
}
