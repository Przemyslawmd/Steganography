﻿
using System.Collections.Generic;

namespace SteganographyCompression
{
    class Decompression
    {
        public List< byte > Decompress( List< byte > source )
        {
            IEnumerator< byte > iter = source.GetEnumerator();
            Dictionary< byte, List< bool >> codesDictionary = GetCodesDictionaryFromStream( iter );
            int dataSizeBeforeCompression = GetIntegerFromStream( iter );

            Node root = new HuffmanTree().BuildTreeDecompression( codesDictionary );
            return Decode( iter, root, dataSizeBeforeCompression );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private Dictionary< byte, List< bool >> GetCodesDictionaryFromStream( IEnumerator< byte > iter )
        {
            int codesDictionarySize = GetIntegerFromStream( iter );
            iter.MoveNext();
            int codesCount = iter.Current == 0 ? 256 : iter.Current;

            Dictionary < byte, byte > codesList = new Dictionary< byte, byte >();

            for ( int i = 0; i < codesCount; i++ )
            {
                iter.MoveNext();
                byte symbol = iter.Current;
                iter.MoveNext();
                codesList.Add( symbol, iter.Current );
            }

            Dictionary< byte, List< bool >> codesDictionary = new Dictionary< byte, List< bool >>();
            List< bool > code = new List< bool >();
            int bitIndex = 1;

            foreach ( KeyValuePair< byte, byte > codeSymbol in codesList )
            {
                GetCodeFromStream( iter, code, codeSymbol.Value, ref bitIndex );
                codesDictionary.Add( codeSymbol.Key, new List< bool >( code ) );
                code.Clear();
            }

            return codesDictionary;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void GetCodeFromStream( IEnumerator< byte > iter, List< bool > code, int codeLenght, ref int bitIndex )
        {
            for ( int i = 0; i < codeLenght; i++ )
            {
                if ( bitIndex == 1 )
                {
                    iter.MoveNext();
                }

                code.Add((( iter.Current >> ( 8 - bitIndex )) % 2 ) != 0 );

                if ( ++bitIndex > 8 )
                {
                    bitIndex = 1;
                }
            }
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private int GetIntegerFromStream( IEnumerator< byte > iter )
        {
            int number = 0;

            for ( int i = 0; i < 3; i++ )
            {
                iter.MoveNext();
                number += iter.Current << ( i * 8 );
            }

            iter.MoveNext();
            return number + iter.Current;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private List< byte > Decode( IEnumerator< byte > iter, Node root, int dataSizeBeforeCompression )
        {                        
            List< byte > decompressedData = new List< byte >();
            Node node = null;

            while ( iter.MoveNext() )
            {
                for ( int bitNumber = 1; bitNumber <= 8; bitNumber++ )
                {
                    if (( iter.Current >> ( 8 - bitNumber )) % 2 == 0 )
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
                        
                        // Some last left bits were used as an alignment
                        if ( decompressedData.Count == dataSizeBeforeCompression )
                        {
                            return decompressedData;
                        }
                        node = null;
                    }
                }
            }

            return decompressedData;
        }
    }
}

