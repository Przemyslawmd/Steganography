
using System.Collections.Generic;
using System.Linq;

namespace Steganography.Huffman
{
    class HuffmanTree
    {
        public NodeCompress BuildTreeCompression( List< byte > sourceData )
        {
            List< NodeCompress > nodes = CreateNodes( sourceData );
            nodes = nodes.OrderBy( x => x.Count ).ToList();
            BuildTree( nodes );
            return nodes.ElementAt( 0 );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        public Node BuildTreeDecompression( Dictionary< byte, List< bool >> codes )
        {
            Node root = new Node( 0 );

            foreach ( KeyValuePair< byte, List< bool >> code in codes )
            {
                Node node = root;
                foreach ( bool token in code.Value.Take( code.Value.Count - 1 ))
                {
                    if ( token is false )
                    {
                        if ( node.Left is null )
                        {
                            node.Left = new Node( 0 );
                        }
                        node = node.Left;
                    }
                    else
                    {
                        if ( node.Right is null )
                        {
                            node.Right = new Node( 0 );
                        }
                        node = node.Right;
                    }
                }

                if ( code.Value.Last() is false )
                {
                    node.Left = new Node( code.Key );
                }
                else
                {
                    node.Right = new Node( code.Key );
                }
            }
            return root;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private List< NodeCompress > CreateNodes( List< byte > source )
        {
            var nodes = new List< NodeCompress >();
            nodes.Add( new NodeCompress( 1, source[0] ));
            bool isFound = false;

            foreach ( byte symbol in source.Skip( 1 ))
            {
                foreach ( NodeCompress node in nodes )
                {
                    if ( symbol == node.ByteValue )
                    {
                        node.Count++;
                        isFound = true;
                        break;
                    }
                    if ( symbol < node.ByteValue )
                    {
                        int index = nodes.IndexOf( node );
                        nodes.Insert( index, new NodeCompress( 1, symbol ) );
                        isFound = true;
                        break;
                    }
                }

                if ( isFound == false )
                {
                    nodes.Add( new NodeCompress( 1, symbol ) );
                }

                isFound = false;
            }
            return nodes;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void BuildTree( List< NodeCompress > nodes )
        {
            NodeCompress internalNode;

            while ( nodes.Count >= 2 )
            {
                internalNode = new NodeCompress( nodes[0].Count + nodes[1].Count, nodes[0], nodes[1] );
                nodes[0].Parent = internalNode;
                nodes[1].Parent = internalNode;
                nodes.RemoveRange( 0, 2 );
                InsertInternalNode( internalNode, nodes );
            }
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void InsertInternalNode( NodeCompress newNode, List< NodeCompress > nodes )
        {
            int index = nodes.FindIndex( node => node.Count >= newNode.Count );
            
            if ( index >= 0 )
            {
                nodes.Insert( index, newNode );
            }
            else
            {
                nodes.Add( newNode );
            }
        }
    }
}

