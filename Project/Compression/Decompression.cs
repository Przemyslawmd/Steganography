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
            sourceData = source.ToArray();
            Dictionary<byte, List<char>> codes = GetCodesFromSource( source  );
            dataCount = BitConverter.ToInt32( sourceData, dataIndex + 1 );

            dataIndex += 4;
            BuildTree( codes );
                      
            return Decode();
        }

        /**********************************************************************************/
        /* GET CODES FROM SOURCE **********************************************************/
        // Get Huffman codes from compressed data
        // Huffman codes are merged with compressed data after compression

        private Dictionary<byte, List<char>> GetCodesFromSource( List<byte> data )
        {
            // Get number of codes from source data, this value is stored in first four bytes            
            int codesCount = 0;
            for ( int i = 0; i < 3; i++ )
            {
                codesCount += (int)data[i];                
                codesCount <<= 8;
            }
            codesCount += (int)data[3];

            Dictionary<byte, List<char>> codes = new Dictionary<byte, List<char>>();
                        
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

                codes.Add( tempByte, new List<char>( code ));
                code.Clear();
            }

            dataIndex = index;
            return codes;
        }

        /********************************************************************************/
        /* BUILD TREE *******************************************************************/

        private void BuildTree( Dictionary<byte, List<char>> codes )
        {
            root = new Node( 0 );
            Node node;
            
            List<byte> tempByte = new List<byte>();
            List<char> tempCode;

            foreach ( KeyValuePair<byte, List<char>> code in codes )
            {
                node = root;
                tempCode = code.Value;

                // Traverse chars in code in exception of first char - each code begins with '1'
                for ( int j = 1; j < tempCode.Count - 1; j++ )
                {
                    if ( tempCode[j] == '0' )
                    {
                        if ( node.Left == null )
                            node.Left = new Node( 0 );
                        node = node.Left;
                    }

                    else if ( tempCode[j] == '1' )
                    {
                        if ( node.Right == null )
                            node.Right = new Node( 0 );
                        node = node.Right;
                    }
                }

                // Check last char in code - add leaves
                if ( tempCode[tempCode.Count - 1] == '0' )
                    node.Left = new Node( code.Key );
                else if ( tempCode[tempCode.Count - 1] == '1' )
                    node.Right = new Node( code.Key );
             }
        }

        /*********************************************************************************/
        /* DECODE ************************************************************************/
        // Decompress data - change codes into byte values

        private List<byte> Decode()
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
                        if ( ++Count == dataCount )
                            return decompressedData;
                        node = null;
                    }
                }
            }                       
            return decompressedData;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        int dataCount;                      // count of bytes to be decompressed
        private Node root;
        private byte[] sourceData;          // data to be decompressed              
        private int dataIndex;              // index in sourceData
    }
}
