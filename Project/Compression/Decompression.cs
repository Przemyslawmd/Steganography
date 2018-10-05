
using System.Collections.Generic;
using Steganography;

namespace SteganographyCompression
{
    class Decompression
    {
        public List< byte > Decompress( List< byte > source, ref Messages.MessageCode code )
        {
            IEnumerator< byte > iter = source.GetEnumerator();
            Dictionary< byte, List< bool >> codesDictionary;
            try
            {
                codesDictionary = GetCodesDictionaryFromStream( iter );
            }
            catch ( System.ArgumentException )
            {
                code = Messages.MessageCode.IMPROPER_DATA_IN_PICTURE;
                return null;
            }

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
            bitIterator = new BitIterator();

            foreach ( KeyValuePair< byte, byte > codeSymbol in codesList )
            {
                GetCodeFromStream( iter, code, codeSymbol.Value );
                codesDictionary.Add( codeSymbol.Key, new List< bool >( code ) );
                code.Clear();
            }

            return codesDictionary;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void GetCodeFromStream( IEnumerator< byte > iter, List< bool > code, int codeLenght )
        {
            for ( int i = 0; i < codeLenght; i++ )
            {
                if ( bitIterator.IsInitialIndex() )
                {
                    iter.MoveNext();
                }

                code.Add((( iter.Current >> ( 7 - bitIterator.Index )) % 2 ) != 0 );
                bitIterator.IncrementIndex();
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

        /**************************************************************************************/
        /**************************************************************************************/

        BitIterator bitIterator;
    }
}

