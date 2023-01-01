
using System;
using System.Collections.Generic;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Tests")]

namespace Steganography.Huffman
{
    class Compression
    {
        public List< byte > MakeCompressedStream( List< byte > source )
        {
            NodeCompress root = new HuffmanTree().BuildTreeCompression( source );
            var codes = new HuffmanCodesGenerator().CreateCodesDictionary( root );
            var dataCompressed = Compress( source, codes );

            var dataFinal = new List< byte >( BitConverter.GetBytes( source.Count ));
            var dataWithCodes = CreateCodesDictionaryStream(codes);
            dataFinal.AddRange( dataWithCodes );
            dataFinal.AddRange( dataCompressed );
            return dataFinal;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private List< byte > Compress( List< byte > source, Dictionary< byte, List< bool >> codes )
        {
            byte compressedDataPortion = 0;
            BitIterator bitIterator = new BitIterator();
            var compressedData = new List< byte >();

            foreach ( byte value in source )
            {
                foreach ( bool token in codes[value] )
                {
                    compressedDataPortion <<= 1;
                    if ( token )
                    {
                        compressedDataPortion += 1;
                    }

                    bitIterator.IncrementIndex();

                    if ( bitIterator.IsInitialIndex() )
                    {
                        compressedData.Add( compressedDataPortion );
                        compressedDataPortion = 0;
                    }
                }
            }     
            
            if ( bitIterator.IsInitialIndex() is false )
            {
                compressedDataPortion <<= ( Constants.BitsInByte - bitIterator.Index );
                compressedData.Add( compressedDataPortion );
            }
            return compressedData;
        }

        /**************************************************************************************/
        /**************************************************************************************/
                    
        private List< byte > CreateCodesDictionaryStream( Dictionary< byte, List< bool >> codes )
        {
            var codesStream = new List< byte >();

            foreach ( KeyValuePair< byte, List< bool >> code in codes )
            {
                codesStream.Add( code.Key );
                codesStream.Add( (byte) code.Value.Count );
            }

            byte codesStreamPortion = 0;
            BitIterator bitIterator = new BitIterator();

            foreach ( KeyValuePair< byte, List< bool >> code in codes )
            {
                foreach ( bool token in code.Value )
                {
                    codesStreamPortion <<= bitIterator.Index > 0 ? 1 : 0;

                    if ( token )
                    {
                        codesStreamPortion += 1;
                    }

                    bitIterator.IncrementIndex();

                    if ( bitIterator.IsInitialIndex() )
                    {
                        codesStream.Add( codesStreamPortion );
                        codesStreamPortion = 0;
                    }
                }
            }

            if ( bitIterator.IsInitialIndex() == false )
            {
                codesStreamPortion <<= ( Constants.BitsInByte - bitIterator.Index );
                codesStream.Add( codesStreamPortion );
            }

            int codesCount = ( codes.Count == 256 ) ? 0 : codes.Count;
            codesStream.Insert( 0, (byte) codesCount );
            codesStream.InsertRange( 0, BitConverter.GetBytes( codesStream.Count ));
            return codesStream;
        }
    }
}

