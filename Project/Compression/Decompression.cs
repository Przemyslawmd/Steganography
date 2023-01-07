
using System.Collections.Generic;

namespace Steganography.Huffman
{
    class Decompression
    {
        public List< byte > Decompress( List< byte > source, ref Result result )
        {
            IEnumerator< byte > iter = source.GetEnumerator();
            int dataSizeBeforeCompression = GetIntegerFromStream( iter );
            iter.MoveNext();

            Dictionary< byte, List< Token >> codesDictionary;
            try
            {
                codesDictionary = GetCodesDictionaryFromStream( iter );
            }
            catch ( System.ArgumentException )
            {
                result = Result.IMPROPER_DATA_IN_PICTURE;
                return null;
            }

            Node root = new HuffmanTree().BuildTreeDecompression( codesDictionary );
            return Decode( iter, root, dataSizeBeforeCompression );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private Dictionary< byte, List< Token >> GetCodesDictionaryFromStream( IEnumerator< byte > iter )
        {
            int codesCount = iter.Current == 0 ? 256 : iter.Current;
            var codesList = new Dictionary< byte, byte >();

            for ( int i = 0; i < codesCount; i++ )
            {
                iter.MoveNext();
                byte symbol = iter.Current;
                iter.MoveNext();
                codesList.Add( symbol, iter.Current );
            }

            var codesDictionary = new Dictionary< byte, List< Token >>();
            var code = new List< Token >();
            bitIterator = new BitIterator();

            foreach ( KeyValuePair< byte, byte > codeSymbol in codesList )
            {
                GetCodeFromStream( iter, code, codeSymbol.Value );
                codesDictionary.Add( codeSymbol.Key, new List< Token >( code ));
                code.Clear();
            }

            return codesDictionary;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void GetCodeFromStream( IEnumerator< byte > iter, List< Token > code, int codeLenght ) 
        {
            for ( int i = 0; i < codeLenght; i++ )
            {
                if ( bitIterator.IsInitial() )
                {
                    iter.MoveNext();
                }

                code.Add((( iter.Current >> ( 7 - bitIterator.Index )) % 2 ) != 0 ? Token.One : Token.Zero );
                bitIterator.Increment();
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
            var decompressedData = new List< byte >();
            Node node = root;

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
                        node = node.Right;
                    }

                    if ( node.IsLeaf() )
                    {
                        decompressedData.Add( node.ByteValue );
                        
                        if ( decompressedData.Count == dataSizeBeforeCompression )
                        {
                            return decompressedData;
                        }
                        node = root;
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

