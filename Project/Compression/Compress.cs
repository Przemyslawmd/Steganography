using System;
using System.Collections.Generic;
using System.Linq;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Tests")]

namespace Compression
{
    class Compress
    {
        /************************************************************************************************/
        /* COMPRESS DATA ********************************************************************************/
        // Public method to start compression                
        // Returns compressed data merged with Huffman codes
        public byte[] CompressData( byte[] source )
        {
            int sizeBeforeCompress = source.Length;            

            NodeCompress root = new HuffmanTree().BuildTree( source );
            codes = new HuffmanCodes().CreateCodesDictionary( root );

            source = StartCompress( source );                    
            InsertCodes( );

            byte[] header = codesData.ToArray();    
            byte[] finalData = new byte[source.Length + header.Length + 4];           
            Buffer.BlockCopy( header, 0, finalData, 0, header.Length );
            Buffer.BlockCopy( new int[1] { sizeBeforeCompress }, 0, finalData, header.Length, 4 );
            Buffer.BlockCopy( source, 0, finalData, header.Length + 4, source.Length );
           
            return finalData;           
        }

        /************************************************************************************************/
        /* START COMPRESS *******************************************************************************/
        // Main method for compression                
        // Returns compressed data

        private byte[] StartCompress( byte[] source )
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
            
            return compressedData.ToArray();
        }

        /**************************************************************************************************/
        /* INSERT CODES ***********************************************************************************/
        // Gets codes from dictionary and merges it with compressed data 
                    
        private void InsertCodes()
        {
            codesData = new List<byte>();
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
        }

        /********************************************************************************************************/
        /********************************************************************************************************/

        private List<byte> codesData;              // data with codes that are to be merged with compressed data
        private List<byte> compressedData;     
        private Dictionary<byte, String> codes;    // final codes : byte values with bit sequences  
        readonly int BitsInByte = 8;     
    }
}
