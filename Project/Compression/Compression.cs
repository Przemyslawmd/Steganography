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
        // Returns compressed data merged with Huffman codes

        public List<byte> Compress( List<byte> source )
        {
            int sizeBeforeCompress = source.Count;

            NodeCompress root = new HuffmanTree().BuildTree( source.ToArray() );
            codes = new HuffmanCodes().CreateCodesDictionary( root );

            compressedData = StartCompress( source.ToArray() );
            List<byte> codesData = InsertCodes();

            codesData.AddRange( BitConverter.GetBytes( sizeBeforeCompress ) );
            codesData.AddRange( compressedData );
            return codesData;
        }

        /************************************************************************************************/
        /* START COMPRESS *******************************************************************************/
        // Main method for compression, returns compressed data

        private List<byte> StartCompress( byte[] source )
        {
            int bitShift = 0;
            byte temp = 0;
            char[] code = null;
            compressedData = new List<byte>();
            
            for (int i = 0; i < source.Length; i++)
            {
                // Get code for byte 
                code = codes[source[i]].ToCharArray();
               
                for (int j = 0; j < code.Length; j++)
                {
                    if ( bitShift == BitsInByte )
                    {
                        compressedData.Add( temp );
                        temp = 0;
                        bitShift = 0;
                    }
                    
                    temp <<= 1;
                    if ( code[j] == '1' ) 
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
            
            return compressedData;
        }

        /**************************************************************************************************/
        /* INSERT CODES ***********************************************************************************/
        // Get codes from dictionary and merge it with compressed data
                    
        private List<byte> InsertCodes()
        {
            List<byte> codesData = new List<byte>();
            byte[] keys = new byte[codes.Count];
            int temp;
            char[] tempCode;

            // In first four bytes there is an information about size of Dictionary
            temp = codes.Count;
            for ( int i = 0, j = 24; i < 4; i++, j -= BitsInByte )
                codesData.Add(( byte )( temp >> j ));
                        
            codes.Keys.CopyTo( keys, 0 );            
            
            for ( int i = 0; i < codes.Count; i++ )
            {
                codesData.Add(keys[i]);
                tempCode = codes[keys[i]].ToCharArray();
                temp = 0;

                for ( int k = 0; k < tempCode.Length - 1; k++ )
                {
                    if ( tempCode[k] == '1' )
                        temp += 1;
                    temp <<= 1;
                }

                if ( tempCode[tempCode.Length - 1] == '1' )
                    temp += 1;

                for ( int k = 0, j = 24; k < 4; k++, j -= BitsInByte )
                    codesData.Add(( byte )( temp >> j ));
            }
            return codesData;
        }

        /********************************************************************************************************/
        /********************************************************************************************************/

        private List<byte> compressedData;     
        private Dictionary<byte, string> codes;    // final codes : byte values with bit sequences  
        readonly int BitsInByte = 8;     
    }
}
