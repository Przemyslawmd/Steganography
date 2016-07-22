using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Linq;
//using Debug;

namespace Compression
{
    class Compress
    {
        /**********************************************************************************************************************************/
        /* PUBLIC INTERFACE OF THIS CLASS *************************************************************************************************/
        /* Class has no other public methods **********************************************************************************************/
        
        public byte[] CompressData( byte[] source )
        {        
            // One element array for Buffer.BlockCopy method 
            int[] DataCount  = new int[1] { source.Length }; 
                        
            CreateNodes( source );            
            nodes.OrderBy( x => x.Count );            
            BuildTree(); 

            // codes and tempCode are used in generateCodes method, but must be declared out this method, because this method is recursive
            codes = new Dictionary<byte, String>();
            tempCode = new List<Char>();
            GenerateCodes( nodes[0], '1' );
            source = StartCompress( source );     
            InsertCodes();

            byte[] header = codesData.ToArray();    
            byte[] finalData = new byte[source.Length + header.Length + 4];           
            Buffer.BlockCopy(header, 0, finalData, 0, header.Length);
            Buffer.BlockCopy(DataCount, 0, finalData, header.Length, 4);            
            Buffer.BlockCopy(source, 0, finalData, header.Length + 4, source.Length);
            
            return finalData;           
        }    
       
        /********************************************************************************************************************************/
        /* CREATE LIST OF NODECOMPRESS OBJECTS ******************************************************************************************/
             
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
            return;
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
        /* INNER METHOD OF BUILD TREE METHOD **********************************************************************************************/

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
            if ( node.Leaf ) 
                codes.Add( node.ByteValue, String.Concat( tempCode ) );
            tempCode.RemoveAt( tempCode.Count - 1 );            
        }        
        
        /****************************************************************************************************************************/
        /* MAIN ACTIVITY FOR COMPRESSING DATA ***************************************************************************************/

        private byte[] StartCompress( byte[] source )
        {
            int bitShift = 0;
            byte temp = 0;
            char[] code = null;
            compressedData = new List<byte>();
            
            for (int i = 0; i < source.Length; i++)
            {
                // Gets code for byte from an uncompressed data
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
            
            // Some data remains, there is a need to add alignment
            if ( bitShift != 0 )           
            {
                temp <<= ( BITS_IN_BYTE - bitShift );
                compressedData.Add( temp );
            }
            
            return compressedData.ToArray();
        }

        /**************************************************************************************************/
        /* GETS CODES FROM DICTIONARY AND MERGE IT WITH COMPRESSED DATA ***********************************/
        
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

            // Bytes values are copied into separate 
            codes.Keys.CopyTo( keys, 0 );
            
            // Codes (bytes values plus codes) are added into data
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

            return; 
        }

        /****************************************************************************************************************/
        /****************************************************************************************************************/

        private List<byte> codesData;
        private List<byte> compressedData;        
        
        // Contains final codes which means byte value bounded with sequence of bits
        private Dictionary<byte, String> codes;
        // tempCode is used while building final codes
        private List<Char> tempCode;        
        private List<NodeCompress> nodes;
        private int BITS_IN_BYTE = 8;     
    }
}
