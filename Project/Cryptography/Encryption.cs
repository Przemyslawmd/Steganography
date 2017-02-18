using System;
using System.Collections.Generic;
using System.Text;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Tests")]

namespace Cryptography
{
    class Encryption : BaseCryptography
    {
        public List<byte> Encrypt( byte[] source, String password )
        {
            key = Key.CreateKey( password );

            byte[] data = AlignData( source );

            for ( int i = 0; i < data.Length; i += 16 )
                EncryptBlockData( data, i ); 
                        
            return null;
        }

        /***********************************************************************************************/
        /* ENCRYPT ONE BLOCK DATA **********************************************************************/

        private void EncryptBlockData( byte[] data, int blockShift )
        {
            AddRoundKey( 0, data, blockShift, key );

            for ( int roundNumber = 1; roundNumber < roundCount - 1; roundNumber++ )
            {
                SubBytes( data, blockShift + roundNumber );
                ShiftRows( data, blockShift + roundNumber );
                MixColumns( data, blockShift + roundNumber );
                AddRoundKey( roundNumber, data, blockShift, key );
            }

            // Last round without MixColumns 
            SubBytes( data, blockShift + roundCount - 1 );
            ShiftRows( data, blockShift + roundCount - 1 );
            AddRoundKey( roundCount - 1, data, blockShift, key );
        }

        /* ALIGN DATA *********************************************************************************/
        /* Add additional bytes for data to be divided by block size **********************************/

        private byte[] AlignData( byte[] source )
        {
            int beginSize = source.Length;
            int alignment = 16 - ( beginSize % ( keyLength / 8 ));            
            Array.Resize( ref source, beginSize + alignment );

            for ( int i = 0; i < alignment - 1; i++ )
                source[beginSize + i] = ( 0x00 );

            source[beginSize + alignment - 1] = (byte)alignment;
            return source;
        }

        /************************************************************************************************/
        /* SHIFT ROWS IN MATRIX STATE *******************************************************************/

        private void ShiftRows( byte[] data, int shift )
        {
            byte temp;
            
            // Second row
            temp = data[shift + 4];
            data[shift + 4] = data[shift + 5];
            data[shift + 5] = data[shift + 6];
            data[shift + 6] = data[shift + 7];
            data[shift + 7] = temp;

            // Third row - swap two pairs of columns 
            data[shift + 8] += data[shift + 10];
            data[shift + 10] = (byte)( data[shift + 8] - data[shift + 10] );
            data[shift + 8] -= data[shift + 10];

            data[shift + 9] += data[shift + 11];
            data[shift + 11] = (byte)( data[shift + 9] - data[shift +11] );
            data[shift + 9] -= data[shift + 11];

            // Fourth row
            temp = data[shift + 15];
            data[shift + 15] = data[shift + 14];
            data[shift + 14] = data[shift + 13];
            data[shift + 13] = data[shift + 12];
            data[shift + 12] = temp;
        }

        /************************************************************************************************/
        /* SUBBYTES TRANSORMATION ***********************************************************************/

        private void SubBytes( byte[] data, int shift )
        {
            for ( int i = 0; i < 16; i++ )
                data[shift + i] = BaseCryptography.GetSbox( data[shift + i] );
        }


        private void MixColumns( byte[] data, int shift )
        {
            //data[shift + 0] = 
        }


        /*************************************************************************************************/
        /* MULTIPLY BY 2 IN GF(2^8) **********************************************************************/

        private byte MultiplyInGF( byte data )
        {
            bool flag = (( data & 0x80 ) != 0x00 ) ? true : false;
            data <<= 1;

            if ( flag )
                data ^= 0x1b;
            return data;
        }


        private byte[] key;
    }
}
