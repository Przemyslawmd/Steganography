using System;
using System.Collections.Generic;
using System.Text;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Tests")]

namespace Cryptography
{
    class Encryption : BaseCryptography
    {
        public List<byte> Encrypt( List<byte> source )
        {
            AlignData( source );

            AddRoundKey( 0 );

            for ( int i = 1; i < roundCount - 1; i++ )
            {
                SubBytes();
                ShiftRows();
                MixColumns();
                AddRoundKey( i );
            }

            // Last round without MixColumns 
            SubBytes();
            ShiftRows();
            AddRoundKey(roundCount - 1);
            
            return null;
        }

        /* ALIGN DATA ********************************************************************************/
        /* Add additional byte for data to be divided by block size                                  */

        private void AlignData( List<byte> source )
        {
            int alignment = 16 - ( source.Count % ( keyLength / 8 ));

            for ( int i = 0; i < alignment - 1; i++ )
                source.Add( 0x00 );

            source.Add( (byte)alignment );
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
