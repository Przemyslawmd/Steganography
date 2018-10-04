
using System;
using System.Collections.Generic;
using Steganography;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Tests")]

namespace SteganographyCompression
{
    class Compression
    {
        public List< byte > Compress( List<byte> source )
        {
            int originalSize = source.Count;
            NodeCompress root = new HuffmanTree().BuildTreeCompression( source );
            codes = new HuffmanCodesGenerator().CreateCodesDictionary( root );
            List< byte > compressedData = StartCompress( source );

            // data to be returned contains codes at the beginning
            List< byte > finalData = CreateCodesData();
            finalData.AddRange( BitConverter.GetBytes( originalSize ));
            finalData.AddRange( compressedData );
            return finalData;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private List< byte > StartCompress( List< byte > source )
        {
            byte temp = 0;
            bitIterator = new BitIterator();
            List< byte > compressedData = new List< byte >();

            foreach ( byte value in source )
            {
                foreach ( bool token in codes[value] )
                {
                    temp <<= 1;
                    if ( token )
                    {
                        temp += 1;
                    }

                    bitIterator.IncrementIndex();

                    if ( bitIterator.IsInitialIndex() )
                    {
                        compressedData.Add( temp );
                        temp = 0;
                    }
                }
            }     
            
            // Some data remains, add an alignment
            if ( bitIterator.IsInitialIndex() == false )
            {
                temp <<= ( BitsInByte - bitIterator.Index );
                compressedData.Add( temp );
            }
            return compressedData;
        }

        /**************************************************************************************/
        /**************************************************************************************/
                    
        private List< byte > CreateCodesData()
        {
            List< byte > codesStream = new List< byte >();

            foreach ( KeyValuePair< byte, List< bool >> code in codes )
            {
                codesStream.Add( code.Key );
                codesStream.Add( (byte) code.Value.Count );
            }

            byte temp = 0;
            bitIterator = new BitIterator();

            foreach ( KeyValuePair<byte, List< bool >> code in codes )
            {
                foreach ( bool token in code.Value )
                {
                    if ( bitIterator.Index > 0 )
                    {
                        temp <<= 1;
                    }

                    if ( token )
                    {
                        temp += 1;
                    }

                    bitIterator.IncrementIndex();

                    if ( bitIterator.IsInitialIndex() )
                    {
                        codesStream.Add( temp );
                        temp = 0;
                    }
                }
            }

            if ( bitIterator.IsInitialIndex() == false )
            {
                temp <<= ( BitsInByte - bitIterator.Index );
                codesStream.Add( temp );
            }

            // Size is being increased with 5 bytes because of two statements below
            int codesStreamSize = codesStream.Count + 5;

            codesStream.InsertRange( 0, BitConverter.GetBytes( codesStreamSize ));
            int codesCount = ( codes.Count == 256 ) ? 0 : codes.Count;
            codesStream.Insert( 4, (byte) codesCount );

            return codesStream;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private Dictionary< byte, List< bool >> codes;
        private BitIterator bitIterator;
        private readonly int BitsInByte = 8;

    }
}

