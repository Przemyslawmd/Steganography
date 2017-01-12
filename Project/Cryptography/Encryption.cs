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
                SubBytes( data, blockShift, roundNumber );
                ShiftRows();
                MixColumns();
                AddRoundKey( roundNumber, data, blockShift, key );
            }

            // Last round without MixColumns 
            SubBytes( data, blockShift, roundCount - 1 );
            ShiftRows();
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

        private void ShiftRows()
        {

        }

        /************************************************************************************************/
        /* SUBBYTES TRANSORMATION ***********************************************************************/

        private void SubBytes( byte[] data, int blockShift, int roundShift )
        {
            for ( int i = 0; i < 16; i++ )
                data[blockShift + roundShift + i] = BaseCryptography.GetSbox( data[blockShift + roundShift + i] );
        }


        private void MixColumns()
        {

        }
        
        private byte[] key;
    }
}
