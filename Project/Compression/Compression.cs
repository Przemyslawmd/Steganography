
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

        private List< byte > Compress( List< byte > source, Dictionary< byte, List< Token >> codes )
        {
            byte compressByte = 0;
            var compressStream = new List< byte >();
            BitIterator bitIterator = new BitIterator();

            foreach ( byte value in source )
            {
                foreach ( Token token in codes[value] )
                {
                    compressByte <<= 1;
                    if ( token == Token.One )
                    {
                        compressByte += 1;
                    }

                    bitIterator.Increment();

                    if ( bitIterator.IsInitial() )
                    {
                        compressStream.Add( compressByte );
                        compressByte = 0;
                    }
                }
            }     
            
            if ( bitIterator.IsInitial() is false )
            {
                compressByte <<= ( Constants.BitsInByte - bitIterator.Index );
                compressStream.Add( compressByte );
            }
            return compressStream;
        }

        /**************************************************************************************/
        /**************************************************************************************/
                    
        private List< byte > CreateCodesStream( Dictionary< byte, List< Token >> codes )
        {
            var codesStream = new List< byte >();
            codesStream.Add( codes.Count == 256 ? (byte) 0 : (byte) codes.Count );

            foreach ( KeyValuePair< byte, List< Token >> code in codes )
            {
                codesStream.Add( code.Key );
                codesStream.Add( (byte) code.Value.Count );
            }

            byte codesStreamByte = 0;
            BitIterator bitIterator = new BitIterator();

            foreach ( KeyValuePair< byte, List< Token >> code in codes )
            {
                foreach ( Token token in code.Value )
                {
                    codesStreamByte <<= bitIterator.Index > 0 ? 1 : 0;

                    if ( token == Token.One )
                    {
                        codesStreamByte += 1;
                    }

                    bitIterator.Increment();

                    if ( bitIterator.IsInitial() )
                    {
                        codesStream.Add( codesStreamByte );
                        codesStreamByte = 0;
                    }
                }
            }

            if ( bitIterator.IsInitial() == false )
            {
                codesStreamByte <<= ( Constants.BitsInByte - bitIterator.Index );
                codesStream.Add( codesStreamByte );
            }
            return codesStream;
        }
    }
}

