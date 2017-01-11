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
            byte[] key = Key.CreateKey( password );

            AlignData( source );

            AddRoundKey( 0, source, key );

            for ( int i = 1; i < roundCount - 1; i++ )
            {
                SubBytes();
                ShiftRows();
                MixColumns();
                AddRoundKey( i, source, key );
            }

            // Last round without MixColumns 
            SubBytes();
            ShiftRows();
            AddRoundKey( roundCount - 1, source, key );
            
            return null;
        }

        private void EncryptBlockData( int index )
        {




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


        private void SubBytes()
        {

        }


        private void MixColumns()
        {

        }
    }
}
