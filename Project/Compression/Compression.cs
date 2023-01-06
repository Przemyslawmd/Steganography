
using System;
using System.Collections.Generic;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Tests")]

namespace Steganography.Huffman
{
    class Compression
    {
        public List< byte > MakeCompressedStream( List< byte > source )
        {
            Node root = new HuffmanTree().BuildTreeCompression( source );
            var codes = new HuffmanCodesGenerator().CreateCodesDictionary( root );
            var compressedData = Compress( source, codes );
            var codesData = CreateCodesStream( codes );

            var finalData = new List< byte >( BitConverter.GetBytes( source.Count ));
            finalData.AddRange( codesData );
            finalData.AddRange( compressedData );
            return finalData;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private List< byte > Compress( List< byte > source, Dictionary< byte, List< bool >> codes )
        {
            byte compressByte = 0;
            var compressStream = new List<byte>();
            BitIterator bitIterator = new BitIterator();

            foreach ( byte value in source )
            {
                foreach ( bool token in codes[value] )
                {
                    compressByte <<= 1;
                    if ( token )
                    {
                        compressByte += 1;
                    }

                    bitIterator.IncrementIndex();

                    if ( bitIterator.IsInitialIndex() )
                    {
                        compressStream.Add( compressByte );
                        compressByte = 0;
                    }
                }
            }     
            
            if ( bitIterator.IsInitialIndex() is false )
            {
                compressByte <<= ( Constants.BitsInByte - bitIterator.Index );
                compressStream.Add( compressByte );
            }
            return compressStream;
        }

        /**************************************************************************************/
        /**************************************************************************************/
                    
        private List< byte > CreateCodesStream( Dictionary< byte, List< bool >> codes )
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

