
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stegan
{
    class Decompression
    {
        /**********************************************************************************/
        /* DECOMPRESS DATA  ***************************************************************/
        // Public Method to start decompression

        public List<byte> Decompress( List<byte> source )
        {
            Dictionary<byte, List<char>> codes = GetCodesFromSource( source  );
            dataCount = BitConverter.ToInt32( source.ToArray(), dataIndex + 1 );

            dataIndex += 4;
            Node root = new HuffmanTree().BuildTreeDecompression( codes );
            return Decode( source, root );
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
                if ( i != ( codesCount - 1 ))
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

        /*********************************************************************************/
        /* DECODE ************************************************************************/
        // Decompress data - change codes into byte values

        private List<byte> Decode( List<byte> source, Node root )
        {                        
            List<byte> decompressedData = new List<byte>();
            byte[] mask = { 128, 64, 32, 16, 8, 4, 2, 1 };
                        
            Node node = null;
            int Count = 0;

            // Get byte from data to be decompressed
            foreach ( byte symbol in source.Skip( dataIndex + 1 ))
            {
                // Check each bite in a byte and traverse tree
                for ( int j = 0; j < 8; j++ )
                {                                        
                    if (( mask[j] & symbol ) == 0 )
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
        private int dataIndex;              // index in sourceData
    }
}
