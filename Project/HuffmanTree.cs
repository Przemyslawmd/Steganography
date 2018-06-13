
using System;
using System.Collections.Generic;
using System.Linq;

namespace Steganography
{
    class HuffmanTree
    {
        public NodeCompress BuildTreeCompression( List< byte > sourceData )
        {
            List< NodeCompress > nodes = CreateNodes( sourceData );
            nodes = nodes.OrderBy( x => x.Count ).ToList();
            BuildTree( nodes );
            return nodes[0];
        }

        /**************************************************************************************/
        /**************************************************************************************/

        public Node BuildTreeDecompression( Dictionary< byte, List< char >> codes )
        {
            Node root = new Node( 0 );
            Node node;

            foreach ( KeyValuePair< byte, List< char >> code in codes )
            {
                node = root;

                // Traverse chars in code in exception of first and last char - each code begins with char '1'
                foreach ( char token in code.Value.Skip( 1 ).Take( code.Value.Count - 2 ) )
                {
                    if ( token == '0' )
                    {
                        if ( node.Left == null )
                        {
                            node.Left = new Node( 0 );
                        }
                        node = node.Left;
                    }
                    else
                    {
                        if ( node.Right == null )
                        {
                            node.Right = new Node( 0 );
                        }
                        node = node.Right;
                    }
                }

                // Check last char in code - add leaf
                if ( code.Value.Last() == '0' )
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

        private List<NodeCompress> CreateNodes( List< byte > sourceData )
        {
            List< NodeCompress > nodes = new List< NodeCompress >();
            nodes.Add( new NodeCompress( 1, sourceData[0] ));
            Boolean isFound = false;
            int index;

            foreach ( byte symbol in sourceData.Skip( 1 ))
            {
                index = 0;
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
                        nodes.Insert( index, new NodeCompress( 1, symbol ));
                        isFound = true;
                        break;
                    }
                    index++;
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

        private void BuildTree( List< NodeCompress > listNodes )
        {
            NodeCompress newNode;

            while ( listNodes.Count >= 2 )
            {
                newNode = new NodeCompress( listNodes[0].Count + listNodes[1].Count, listNodes[0], listNodes[1] );
                listNodes[0].Parent = newNode;
                listNodes[1].Parent = newNode;
                listNodes.RemoveAt( 0 );
                listNodes.RemoveAt( 0 );
                InsertNodeIntoTree( newNode, listNodes );
            }
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void InsertNodeIntoTree( NodeCompress newNode, List<NodeCompress> listNodes )
        {
            Boolean isInserted = false;

            for ( int i = 0; i < listNodes.Count; i++ )
            {
                if ( listNodes[i].Count > newNode.Count )
                {
                    listNodes.Insert( i, newNode );
                    isInserted = true;
                    break;
                }
            }

            if ( isInserted == false )
            {
                listNodes.Add( newNode );
            }
        }
    }
}

