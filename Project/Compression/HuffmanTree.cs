using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compression
{
    class HuffmanTree
    {
        public NodeCompress BuildTree( byte[] sourceData )
        {
            List<NodeCompress> nodes = CreateNodes( sourceData );
            nodes = nodes.OrderBy( x => x.Count ).ToList();
            BuildTree( nodes );
            return nodes[0];
        }

        /*********************************************************************/
        /*********************************************************************/

        private List<NodeCompress> CreateNodes( byte[] sourceData )
        {
            List<NodeCompress> list = new List<NodeCompress>();
            list.Add( new NodeCompress( 1, sourceData[0] ) );
            Boolean isFound = false;

            for ( int i = 1; i < sourceData.Length; i++ )
            {
                for ( int j = 0; j < list.Count; j++ )
                {
                    if ( sourceData[i] == list[j].ByteValue )
                    {
                        list[j].Increment();
                        isFound = true;
                        break;
                    }

                    if ( sourceData[i] < list[j].ByteValue )
                    {
                        list.Insert( j, new NodeCompress( 1, sourceData[i] ) );
                        isFound = true;
                        break;
                    }
                }

                if ( !isFound )
                    list.Add( new NodeCompress( 1, sourceData[i] ) );

                isFound = false;
            }
            return list;
        }

        /*********************************************************************************************/
        /* BUILD HUFFMAN TREE ************************************************************************/

        private void BuildTree( List<NodeCompress> listNodes )
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

        /**********************************************************************************************************************************/
        /* INSERT NODE INTO A TREE ********************************************************************************************************/

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

            if ( !isInserted )
                listNodes.Add( newNode );
        }
    }
}
