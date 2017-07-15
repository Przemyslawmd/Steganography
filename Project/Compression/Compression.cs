
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Tests")]

namespace Stegan
{
    class Compression
    {
        /************************************************************************************************/
        /* COMPRESS DATA ********************************************************************************/
        // Public method to start compression                
        // It returns compressed data merged with Huffman codes

        public List<byte> Compress( List<byte> source )
        {
            int originalSize = source.Count;
            NodeCompress root = new HuffmanTree().BuildTree( source );
            codes = new HuffmanCodes().CreateCodesDictionary( root );
            StartCompress( source );

            // Final data at the beginning is filled with codes
            List<byte> finalData = InsertCodes();
            finalData.AddRange( BitConverter.GetBytes( originalSize ));
            finalData.AddRange( compressedData );
            return finalData;
        }

        /************************************************************************************************/
        /* START COMPRESS *******************************************************************************/
        // Main method for compression, returns compressed data

        private void StartCompress( List<byte> source )
        {
            int bitShift = 0;
            byte temp = 0;
            compressedData = new List<byte>();
            
            foreach ( byte value in source )
            {
                foreach ( char token in codes[value] )
                {
                    if ( bitShift == BitsInByte )
                    {
                        compressedData.Add( temp );
                        temp = 0;
                        bitShift = 0;
                    }
                    
                    temp <<= 1;
                    if ( token == '1' )
                        temp += 1;
                    bitShift++;
                }
            }     
            
            // Some data remains, there is a need to add an alignment
            if ( bitShift != 0 )           
            {
                temp <<= ( BitsInByte - bitShift );
                compressedData.Add( temp );
            }
        }

        /**************************************************************************************************/
        /* INSERT CODES ***********************************************************************************/
        // Get codes from dictionary and merge it with compressed data
                    
        private List<byte> InsertCodes()
        {
            List<byte> codesStream = new List<byte>();

            foreach ( KeyValuePair<byte, List<char>> code in codes )
            {
                codesStream.Add( code.Key );
                codesStream.Add( (byte)code.Value.Count );
            }

            byte temp = 0;
            int shift = 0;

            foreach ( KeyValuePair<byte, List<char>> code in codes )
            {
                foreach ( char token in code.Value )
                {
                    if ( ++shift > 1 )
                        temp <<= 1;

                    if ( token == '1' )
                        temp += 1;

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
                codesStream.Insert( 4, 0 );
            else
                codesStream.Insert( 4, (byte)codes.Count );

            return codesStream;
        }

        /********************************************************************************************************/
        /********************************************************************************************************/

        private List<byte> compressedData;
        private Dictionary<byte, List<char>> codes;
        readonly int BitsInByte = 8;
    }
}
