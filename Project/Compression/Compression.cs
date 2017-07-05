using System;
using System.Collections.Generic;

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
            int sizeBeforeCompress = source.Count;
            NodeCompress root = new HuffmanTree().BuildTree( source );
            codes = new HuffmanCodes().CreateCodesDictionary( root );
            StartCompress( source );

            List<byte> finalData = InsertCodes();
            finalData.AddRange( BitConverter.GetBytes( sizeBeforeCompress ) );
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
            List<char> code = new List<char>();
            compressedData = new List<byte>();
            
            foreach ( byte value in source )
            {
                code = codes[value];

                foreach ( char token in code )
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
            List<byte> codesData = new List<byte>();
            List<char> tempCode = new List<char>();
            int temp;

            // First four bytes contain an information about size of Dictionary
            temp = codes.Count;
            for ( int i = 0, j = 24; i < 4; i++, j -= BitsInByte )
                codesData.Add(( byte )( temp >> j ));
                        
            foreach ( KeyValuePair<byte, List<char>> code in codes )
            {
                codesData.Add( code.Key );
                tempCode = code.Value;
                temp = 0;

                for ( int k = 0; k < tempCode.Count - 1; k++ )
                {
                    if ( tempCode[k] == '1' )
                        temp += 1;
                    temp <<= 1;
                }

                if ( tempCode[tempCode.Count - 1] == '1' )
                    temp += 1;

                for ( int k = 0, j = 24; k < 4; k++, j -= BitsInByte )
                    codesData.Add(( byte )( temp >> j ));
            }
            return codesData;
        }

        /********************************************************************************************************/
        /********************************************************************************************************/

        private List<byte> compressedData;

        // Final codes - byte values linked with sequence of bits
        // Sequence of bits is a string because it may have length more than 64
        private Dictionary<byte, List<char>> codes;

        readonly int BitsInByte = 8;     
    }
}
