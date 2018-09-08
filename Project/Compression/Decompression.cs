
using System;
using System.Collections.Generic;
using System.Linq;

namespace SteganographyCompression
{
    class Decompression
    {
        public List< byte > Decompress( List< byte > source )
        {
            Dictionary< byte, List< bool >> codesDictionary = GetCodesDictionaryFromStream( source  );
            originalSize = BitConverter.ToInt32( source.ToArray(), streamIndex );
            streamIndex += 4;

            Node root = new HuffmanTree().BuildTreeDecompression( codesDictionary );
            return Decode( source, root );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private Dictionary< byte, List< bool >> GetCodesDictionaryFromStream( List< byte > stream )
        {
            int codesDictionarySize = BitConverter.ToInt32( stream.GetRange( 0, 4 ).ToArray(), 0 );
            int codesCount = ( stream[4] == 0 ) ? 256 : stream[4];
            streamIndex = 5;

            Dictionary < byte, byte > codesList = new Dictionary< byte, byte >();

            for ( int i = 0; i < codesCount; i++ )
            {
                codesList.Add( stream[streamIndex], stream[streamIndex+1] );
                streamIndex += 2;
            }

            Dictionary< byte, List< bool >> codesDictionary = new Dictionary< byte, List< bool >>();

            int bitIndex = 1;
            List< bool > code = new List< bool >();

            foreach ( KeyValuePair< byte, byte > codeSymbol in codesList )
            {
                GetCodeFromStream( code, stream, codeSymbol.Value, ref streamIndex, ref bitIndex );
                codesDictionary.Add( codeSymbol.Key, new List< bool >( code ) );
                code.Clear();
            }

            streamIndex = codesDictionarySize;
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

            foreach ( byte byteValue in source.Skip( streamIndex ))
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
        private int streamIndex;              // It is global index in a source data to be decompressed
    }
}

