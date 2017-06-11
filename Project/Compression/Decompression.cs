using System;
using System.Collections.Generic;

namespace Stegan
{
    class Decompression
    {
        /**********************************************************************************/
        /* DECOMPRESS DATA  ***************************************************************/
        // Public Method to start decompression

        public List<byte> Decompress( List<byte> source )
        {
            int[] DataCount = new int[1];            
            sourceData = source.ToArray();
            GetCodesFromSource( source  );

            Buffer.BlockCopy( sourceData, dataIndex + 1, DataCount, 0, 4 );
            dataIndex += 4;
            BuildTree(); 
                      
            return Decode( DataCount[0] );    
        }

        /**********************************************************************************/
        /* GET CODES FROM SOURCE **********************************************************/
        // Get Huffman codes from compressed data
        // Huffman codes are merged with compressed data after compression

        private void GetCodesFromSource( List<byte> data )
        {
            // Get number of codes from source data, this value is stored in first four bytes            
            int codesCount = 0;
            for ( int i = 0; i < 3; i++ )
            {
                codesCount += (int)data[i];                
                codesCount <<= 8;
            }
            codesCount += (int)data[3];

            byteValue = new List<byte>( codesCount );
            codeValue = new List<char[]>( codesCount ); 
                        
            byte tempByte;
            int index = 4;
            int codeInt = 0;
            List<char> code = new List<char>();
            
            for (int i = 0; i < codesCount; i++)
            {                
                tempByte = data[index++];

                // Get code as an integer
                for ( int j = 0; j < 3; j++ )
                {
                    codeInt += (int)data[index++];
                    codeInt <<= 8;
                }
                
                codeInt += (int)data[index];
                if (i != ( codesCount - 1 ))
                    index++;
                
                // Change code from an integer into an array of char                
                while ( codeInt != 0 )
                {
                    if (( codeInt % 2 ) == 0 )
                        code.Insert( 0, '0' );
                    else
                        code.Insert( 0, '1' );
                    codeInt >>= 1;
                }

                byteValue.Add( tempByte );
                codeValue.Add( code.ToArray() );               
                code.Clear();
            }

            dataIndex = index;            
        }

        /********************************************************************************/
        /* BUILD TREE *******************************************************************/

        private void BuildTree()
        {
            root = new Node( 0, false );
            Node node;
            
            List<byte> tempByte = new List<byte>();
            char[] tempCode = null;                     

            for ( int i = 0; i < byteValue.Count; i++ )
            {
                node = root;
                tempCode = codeValue[i];

                // Traverse chars in code in exception of first char - each code begins with '1'
                for ( int j = 1; j < tempCode.Length - 1; j++ )
                {
                    if ( tempCode[j] == '0' )
                    {
                        if ( node.Left == null )
                            node.Left = new Node( 0, false );
                        node = node.Left;
                    }

                    else if ( tempCode[j] == '1' )
                    {
                        if ( node.Right == null )
                            node.Right = new Node( 0, false );
                        node = node.Right;
                    }
                }

                // Check last char in code - add leaves
                if ( tempCode[tempCode.Length - 1] == '0' )                
                    node.Left = new Node( byteValue[i], true );
                else if ( tempCode[tempCode.Length - 1] == '1' )                
                    node.Right = new Node( byteValue[i], true );
             }           
        }

        /*********************************************************************************/
        /* DECODE ************************************************************************/
        // Decompress data - change codes into byte values

        private List<byte> Decode( int DataCount )
        {                        
            List<byte> decompressedData = new List<byte>();
            byte[] mask = { 128, 64, 32, 16, 8, 4, 2, 1 };
                        
            Node node = null;
            int Count = 0;

            // Get byte from data to be decompressed
            for ( int i = dataIndex + 1; i < sourceData.Length; i++ )            
            {
                // Check each bite in a byte and traverse tree
                for ( int j = 0; j < 8; j++ )
                {                                        
                    if ( ( mask[j] & sourceData[i] ) == 0 )
                    {
                        node = node.Left;                                                                 
                    }         
                    else
                    {                        
                        if ( node == null )
                        {                                
                            node = root;
                            continue;
                        }
                        node = node.Right;                                                                  
                    }

                    if ( node.isLeaf() )
                    {
                        decompressedData.Add( node.ByteValue );
                        if ( ++Count == DataCount )
                            return decompressedData;
                        node = null;
                    }
                }
            }                       
            return decompressedData;
        } 
        
        /**************************************************************************************/
        /**************************************************************************************/

        private Node root;
        private List<byte> byteValue;
        private List<char[]> codeValue;        
        private byte[] sourceData;          // data to be decompressed              
        private int dataIndex;              // index in sourceData
    }
}
