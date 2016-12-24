using System;
using System.Collections.Generic;
using System.Linq;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Tests")]

namespace Compression
{
    class Compress
    {
        /**********************************************************************************************************************************/
        /* MAIN COMPRESS METHOD ***********************************************************************************************************/
                
        public byte[] CompressData( byte[] source )
        {
            int sizeBeforeCompress = source.Length;
            nodes = CreateNodes( source );            
            nodes = nodes.OrderBy( x => x.Count ).ToList();            
            BuildTree( nodes );           

            codes = new HuffmanCodes().CreateCodesDictionary( nodes[0] );

            source = StartCompress( source );                    
            InsertCodes();

            byte[] header = codesData.ToArray();    
            byte[] finalData = new byte[source.Length + header.Length + 4];           
            Buffer.BlockCopy( header, 0, finalData, 0, header.Length );
            Buffer.BlockCopy( new int[1] { sizeBeforeCompress }, 0, finalData, header.Length, 4 );
            Buffer.BlockCopy( source, 0, finalData, header.Length + 4, source.Length );
           
            return finalData;           
        }    
       
        /*****************************************************************************************************************************/
        /* CREATE LIST OF NODECOMPRESS OBJECTS ***************************************************************************************/
             
        private List<NodeCompress> CreateNodes( byte[] buffer )
        {
            List<NodeCompress> list = new List< NodeCompress >();
            list.Add( new NodeCompress( 1, buffer[0] ) );            
            Boolean isFound = false;
                       
            for ( int i = 1; i < buffer.Length; i++ )
            {                
                for ( int j = 0; j < list.Count; j++ )
                {                    
                    if ( buffer[i] == list[j].ByteValue )
                    {
                        list[j].Increment();
                        isFound = true;
                        break;                                           
                    }

                    if ( buffer[i] < list[j].ByteValue )
                    {
                        list.Insert( j, new NodeCompress( 1, buffer[i] ) );
                        isFound = true;
                        break;
                    }                   
                }
                
                if ( !isFound )
                    list.Add( new NodeCompress( 1, buffer[i] ) );                    
                
                isFound = false;
            }
            return list;
        }
        
        /***********************************************************************************************************************************/
        /* BUILD HUFMANN TREE **************************************************************************************************************/
        
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
                
        /****************************************************************************************************************************/
        /* MAIN ACTIVITY FOR COMPRESSING ********************************************************************************************/

        private byte[] StartCompress( byte[] source )
        {
            int bitShift = 0;
            byte temp = 0;
            char[] code = null;
            compressedData = new List<byte>();
            
            for (int i = 0; i < source.Length; i++)
            {
                // Get code for byte 
                code = codes[source[i]].ToCharArray();
               
                for (int j = 0; j < code.Length; j++)
                {
                    if ( bitShift == BITS_IN_BYTE )
                    {
                        compressedData.Add( temp );
                        temp = 0;
                        bitShift = 0;
                    }
                    
                    temp <<= 1;
                    if ( code[j] == '1' ) 
                        temp += 1;
                    bitShift++;
                }
            }     
            
            // Some data remains, there is a need to add an alignment
            if ( bitShift != 0 )           
            {
                temp <<= ( BITS_IN_BYTE - bitShift );
                compressedData.Add( temp );
            }
            
            return compressedData.ToArray();
        }

        /**************************************************************************************************/
        /* GET CODES FROM DICTIONARY AND MERGE IT WITH COMPRESSED DATA ************************************/
        
        private void InsertCodes()
        {
            codesData = new List<byte>();
            byte[] keys = new byte[codes.Count];
            int temp;
            char[] tempCode;

            // First four bytes contains info about count of Dictionary
            temp = codes.Count;
            for ( int i = 0, j = 24; i < 4; i++, j -= BITS_IN_BYTE )
                codesData.Add(( byte )( temp >> j ));
                        
            codes.Keys.CopyTo( keys, 0 );            
            
            for ( int i = 0; i < codes.Count; i++ )
            {
                codesData.Add(keys[i]);
                tempCode = codes[keys[i]].ToCharArray();
                temp = 0;

                for ( int k = 0; k < tempCode.Length - 1; k++ )
                {
                    if ( tempCode[k] == '1' )
                        temp += 1;
                    temp <<= 1;
                }

                if ( tempCode[tempCode.Length - 1] == '1' )
                    temp += 1;

                for ( int k = 0, j = 24; k < 4; k++, j -= BITS_IN_BYTE )
                    codesData.Add(( byte )( temp >> j ));
            }           
        }

        /****************************************************************************************************************/
        /****************************************************************************************************************/

        private List<byte> codesData;
        private List<byte> compressedData;     
        private Dictionary<byte, String> codes;     // final codes : byte values with bit sequences      
        private List<NodeCompress> nodes;
        private int BITS_IN_BYTE = 8;     
    }
}
