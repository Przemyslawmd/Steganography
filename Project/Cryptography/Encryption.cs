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
    }
}
