
using System;
using System.Collections.Generic;
using System.Linq;

namespace SteganographyCompression
{
    class Decompression
    {
        /**************************************************************************************/
        /**************************************************************************************/

        public List< byte > Decompress( List< byte > source )
        {
            Dictionary<byte, List< char >> codes = GetCodesFromSource( source  );
            originalSize = BitConverter.ToInt32( source.ToArray(), dataIndex );
            dataIndex += 4;

            Node root = new HuffmanTree().BuildTreeDecompression( codes );
            return Decode( source, root );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private Dictionary< byte, List< char >> GetCodesFromSource( List<byte> data )
        {
            // Get size of codes stream and count of codes and move dataIndex
            int codesSize = BitConverter.ToInt32( data.GetRange(0, 4).ToArray(), 0 );
            int codesCount = ( (int) data[4] == 0 ) ? 256 : (int) data[4];
            dataIndex = 5;

            Dictionary< byte, List< char >> codes = new Dictionary< byte, List< char >>();

            // Stream index indicates the beginning of bits sequence that represent codes
            int streamIndex = dataIndex + codesCount * 2;
            int bitIndex = 1;
            List< char > code = new List<char>();

            for ( int i = 0; i < codesCount; i++ )
            {
                GetCodeFromStream( code, data, data[dataIndex + 1], ref streamIndex, ref bitIndex );
                codes.Add( data[dataIndex], new List< char >( code ));
                code.Clear();
                dataIndex += 2;
            }

            // Move data index at the place where codes end
            dataIndex = codesSize;
            return codes;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void GetCodeFromStream( List<char> code, List<byte> data, int codeLenght, ref int streamIndex, ref int bitIndex )
        {
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
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private List< byte > Decode( List< byte > source, Node root )
        {                        
            List< byte > decompressedData = new List<byte>();
            Node node = null;

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
                        // Case where some last bits were used as an alignment
                        if ( decompressedData.Count == originalSize )
                        {
                            return decompressedData;
                        }
                        node = null;
                    }
                }
            }

            return decompressedData;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        int originalSize;                   // Size of data before compression
        private int dataIndex;              // It is global index in a source data to be decompressed

        readonly byte[] mask = new byte[9] { 0x00, 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
    }
}

