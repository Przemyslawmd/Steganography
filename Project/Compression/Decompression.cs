
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
            dataCount = BitConverter.ToInt32( source.ToArray(), dataIndex );

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
            int codesSize = BitConverter.ToInt32( data.GetRange(0, 4).ToArray(), 0 );
            int codesCount = (int)data[4];

            Dictionary<byte, List<char>> codes = new Dictionary<byte, List<char>>();
                        
            dataIndex = 5;
            int streamIndex = 5 + codesCount * 2;
            int bitIndex = 1;
            List<char> code;

            for ( int i = 0; i < codesCount; i++ )
            {
                code = GetCodeFromStream( data, data[dataIndex + 1], ref streamIndex, ref bitIndex );
                codes.Add( data[dataIndex], code );
                dataIndex += 2;
            }

            dataIndex = codesSize;
            return codes;
        }

        /**********************************************************************************/
        /* GET ONE CODE FROM SOURCE *******************************************************/

        private List<char> GetCodeFromStream( List<byte> data, int codeLenght, ref int streamIndex, ref int bitIndex )
        {
            List<char> code = new List<char>();

            for ( int i = 0; i < codeLenght; i++ )
            {
                if (( data[streamIndex] & mask[bitIndex] ) != 0 )
                    code.Add( '1' );
                else
                    code.Add( '0' );

                if ( ++bitIndex > 8 )
                {
                    bitIndex = 1;
                    streamIndex++;
                }
            }

            return code;
        }

        /*********************************************************************************/
        /* DECODE ************************************************************************/
        // Decompress data - change codes into byte values

        private List<byte> Decode( List<byte> source, Node root )
        {                        
            List<byte> decompressedData = new List<byte>();
            Node node = null;
            int Count = 0;

            // Get byte from data to be decompressed
            foreach ( byte symbol in source.Skip( dataIndex ))
            {
                // Check each bite in a byte and traverse tree
                for ( int j = 1; j <= 8; j++ )
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

        static byte[] mask = new byte[9] { 0x00, 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
    }
}
