using System;
using System.Collections.Generic;
using System.Linq;

namespace Compression
{
    class Compress
    {
        /**********************************************************************************************************************************/
        /* MAIN COMPRESS METOHD ***********************************************************************************************************/
                
        public byte[] CompressData( byte[] source )
        {
            int dataCount = source.Length;
            CreateNodes( source );            
            nodes.OrderBy( x => x.Count );            
            BuildTree(); 

            // codes and tempCode are used only in generateCodes, but are declared out of this method, because it's recursive method
            codes = new Dictionary<byte, String>();
            tempCode = new List<Char>();
            GenerateCodes( nodes[0], '1' );
            source = StartCompress( source );     
            InsertCodes();

            byte[] header = codesData.ToArray();    
            byte[] finalData = new byte[source.Length + header.Length + 4];           
            Buffer.BlockCopy(header, 0, finalData, 0, header.Length);
            Buffer.BlockCopy( new int[1] { dataCount }, 0, finalData, header.Length, 4 );
            Buffer.BlockCopy( source, 0, finalData, header.Length + 4, source.Length );
            
            return finalData;           
        }    
       
        /*****************************************************************************************************************************/
        /* CREATE LIST OF NODECOMPRESS OBJECTS ***************************************************************************************/
             
        private void CreateNodes( byte[] buffer )
        {
            nodes = new List< NodeCompress >();
            nodes.Add( new NodeCompress( 1, buffer[0] ) );            
            Boolean isFound = false;
                       
            for ( int i = 1; i < buffer.Length; i++ )
            {                
                for ( int j = 0; j < nodes.Count; j++ )
                {                    
                    if ( nodes[j].ByteValue == buffer[i] )
                    {
                        nodes[j].Increment();
                        isFound = true;
                        break;                                           
                    }                    
                }
                
                if (!isFound)
                    nodes.Add( new NodeCompress( 1, buffer[i] ) );                    
                
                isFound = false;
            }            
        }
        
        /***********************************************************************************************************************************/
        /* BUILD HUFMANN TREE **************************************************************************************************************/
        
        private void BuildTree()
        {            
            NodeCompress newNode;
                        
            while ( nodes.Count >= 2 )
            {                
                newNode = new NodeCompress( nodes[0].Count + nodes[1].Count, nodes[0], nodes[1] );                
                nodes[0].Parent = newNode;
                nodes[1].Parent = newNode;
                nodes.RemoveAt( 0 );
                nodes.RemoveAt( 0 );
                InsertNodeIntoTree( newNode );
            }           
        }

        /**********************************************************************************************************************************/
        /* INSERT NODE INTO A TREE ********************************************************************************************************/

        private void InsertNodeIntoTree( NodeCompress newNode )
        {
            Boolean isInserted = false;
            
            for ( int i = 0; i < nodes.Count; i++ )
            {
                if ( nodes[i].Count > newNode.Count )
                {
                    nodes.Insert( i, newNode );
                    isInserted = true;
                    break;
                }
            }

            if ( !isInserted )
                nodes.Add( newNode );
        }

        /******************************************************************************************************************************/
        /* GENERATES CODES TRAVERSING HUFFMAN TREE ************************************************************************************/
                      
        private void GenerateCodes( NodeCompress node, Char token )
        {
            tempCode.Add(token);

            if ( node.Left != null )
                GenerateCodes( node.Left, '0' );
            if ( node.Right != null )
                GenerateCodes( node.Right, '1' );
            else           
                codes.Add( node.ByteValue, String.Concat( tempCode ) );

            tempCode.RemoveAt( tempCode.Count - 1 );            
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
        private List<Char> tempCode;                // using while creating final codes   
        private List<NodeCompress> nodes;
        private int BITS_IN_BYTE = 8;     
    }
}
