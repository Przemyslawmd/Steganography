
using System;
using System.Collections.Generic;
using Steganography;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Tests")]

namespace SteganographyCompression
{
    class Compression
    {
        public List< byte > Compress( List< byte > source )
        {
            List< byte > resultStream = new List< byte >( BitConverter.GetBytes( source.Count ));

            NodeCompress root = new HuffmanTree().BuildTreeCompression( source );
            codes = new HuffmanCodesGenerator().CreateCodesDictionary( root );
            List< byte > compressedData = StartCompress( source );

            resultStream.AddRange( CreateCodesDictionaryStream() );
            resultStream.AddRange( compressedData );
            return resultStream;
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
                    
        private List< byte > CreateCodesDictionaryStream()
        {
            List< byte > codesStream = new List< byte >();

            foreach ( KeyValuePair< byte, List< bool >> code in codes )
            {
                codesStream.Add( code.Key );
                codesStream.Add( (byte) code.Value.Count );
            }

            byte temp = 0;
            bitIterator = new BitIterator();

            foreach ( KeyValuePair< byte, List< bool >> code in codes )
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

            int codesCount = ( codes.Count == 256 ) ? 0 : codes.Count;
            codesStream.Insert( 0, (byte) codesCount );
            codesStream.InsertRange( 0, BitConverter.GetBytes( codesStream.Count ));

            return codesStream;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private Dictionary< byte, List< bool >> codes;
        private BitIterator bitIterator;
        private readonly int BitsInByte = 8;
    }
}

