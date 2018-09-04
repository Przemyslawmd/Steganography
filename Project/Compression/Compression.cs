
using System;
using System.Collections.Generic;

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
            int bitShift = 0;
            byte temp = 0;
            List<byte> compressedData = new List< byte >();
            
            foreach ( byte value in source )
            {
                foreach ( bool token in codes[value] )
                {
                    if ( bitShift == BitsInByte )
                    {
                        compressedData.Add( temp );
                        temp = 0;
                        bitShift = 0;
                    }
                    
                    temp <<= 1;
                    if ( token )
                    {
                        temp += 1;
                    }
                    bitShift++;
                }
            }     
            
            // Some data remains, there is a need to add an alignment
            if ( bitShift != 0 )           
            {
                temp <<= ( BitsInByte - bitShift );
                compressedData.Add( temp );
            }
            return compressedData;
        }

        /**************************************************************************************/
        /**************************************************************************************/
                    
        private List<byte> CreateCodesData()
        {
            List<byte> codesStream = new List<byte>();

            foreach ( KeyValuePair< byte, List< bool >> code in codes )
            {
                codesStream.Add( code.Key );
                codesStream.Add( (byte)code.Value.Count );
            }

            byte temp = 0;
            int shift = 0;

            foreach ( KeyValuePair<byte, List< bool >> code in codes )
            {
                foreach ( bool token in code.Value )
                {
                    if ( ++shift > 1 )
                    {
                        temp <<= 1;
                    }

                    if ( token )
                    {
                        temp += 1;
                    }

                    if ( shift == BitsInByte )
                    {
                        codesStream.Add( temp );
                        shift = 0;
                        temp = 0;
                    }
                }
            }

            if ( shift != 0 )
            {
                temp <<= ( BitsInByte - shift );
                codesStream.Add( temp );
            }

            // Size is being increased with 5 bytes because of two statements below
            int codesStreamSize = codesStream.Count + 5;

            codesStream.InsertRange( 0, BitConverter.GetBytes( codesStreamSize ));

            // To store count of codes inside one byte, 0 value indicates that there are 256 codes
            if ( codes.Count == 256 )
            {
                codesStream.Insert( 4, 0 );
            }
            else
            {
                codesStream.Insert( 4, (byte) codes.Count );
            }

            return codesStream;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private Dictionary< byte, List< bool >> codes;
        readonly int BitsInByte = 8;
    }
}

