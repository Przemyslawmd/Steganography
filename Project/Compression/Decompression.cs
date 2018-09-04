
using System;
using System.Collections.Generic;
using System.Linq;

namespace SteganographyCompression
{
    class Decompression
    {
        public List< byte > Decompress( List< byte > source )
        {
            Dictionary< byte, List< bool >> codesDictionary = GetCodesFromSource( source  );
            originalSize = BitConverter.ToInt32( source.ToArray(), dataIndex );
            dataIndex += 4;

            Node root = new HuffmanTree().BuildTreeDecompression( codesDictionary );
            return Decode( source, root );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private Dictionary< byte, List< bool >> GetCodesFromSource( List<byte> data )
        {
            // Get size of codes stream and count of codes and move dataIndex
            int codesDictionarySize = BitConverter.ToInt32( data.GetRange(0, 4).ToArray(), 0 );
            int codesCount = ( data[4] == 0 ) ? 256 : data[4];
            dataIndex = 5;

            Dictionary< byte, List< bool >> codesDictionary = new Dictionary< byte, List< bool >>();

            // Stream index indicates the beginning of bits sequence that represent codes
            int codesIndex = dataIndex + codesCount * 2;
            int bitIndex = 1;
            List< bool > code = new List< bool >();

            for ( int i = 0; i < codesCount; i++ )
            {
                GetCodeFromStream( code, data, data[dataIndex + 1], ref codesIndex, ref bitIndex );
                codesDictionary.Add( data[dataIndex], new List< bool >( code ));
                code.Clear();
                dataIndex += 2;
            }

            // Move data index at the place where codes end
            dataIndex = codesDictionarySize;
            return codesDictionary;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void GetCodeFromStream( List< bool > code, List< byte > stream, int codeLenght, ref int codesIndex, ref int bitIndex )
        {
            for ( int i = 0; i < codeLenght; i++ )
            {
                code.Add((( stream[codesIndex] >> ( 8 - bitIndex )) % 2 ) != 0 );

                if ( ++bitIndex > 8 )
                {
                    bitIndex = 1;
                    codesIndex++;
                }
            }
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private List< byte > Decode( List< byte > source, Node root )
        {                        
            List< byte > decompressedData = new List< byte >();
            Node node = null;

            foreach ( byte byteValue in source.Skip( dataIndex ))
            {
                for ( int bitNumber = 1; bitNumber <= 8; bitNumber++ )
                {
                    if (( byteValue >> ( 8 - bitNumber )) % 2 == 0 )
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
                        
                        // Case where some last left bits were used as an alignment
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
    }
}

