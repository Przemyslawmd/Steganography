
using System;
using System.Collections.Generic;

namespace Stegan
{
    class Code
    {
        /********************************************************************************/
        /* CONSTRUCTORS  ****************************************************************/

        public Code()
        {
            tokens = new List<byte>();
        }

        public Code( Code code )
        {
            this.lenght = code.lenght;
            tokens = new List<byte>( code.tokens );
        }

        /********************************************************************************/
        /* GETTERS **********************************************************************/

        public int GetLength()
        {
            return lenght;
        }

        public List<byte> GetTokens()
        {
            return tokens;
        }

        /********************************************************************************/
        /* ADD TOKEN ********************************************************************/

        public void Add( bool value )
        {
            lenght++;
            int position = lenght % 8;
                        
            if ( position == 1 )
                tokens.Add( 0x00 );

            if ( value )
                tokens[lenght / 8] |= maskOr[8 - position];
            else
                tokens[lenght / 8] &= maskAnd[8 - position];            
        }

        /********************************************************************************/
        /* REMOVE TOKEN *****************************************************************/

        public void Remove( )
        {
            if ( lenght % 8 == 1 )
                tokens.RemoveAt( lenght / 8 );

            lenght--;
        }

        /********************************************************************************/
        /********************************************************************************/

        private List<byte> tokens;

        private int lenght;

        // Mask used to set an indicated bit to 1 value 
        private static byte[] maskOr =
        {
            0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80    
        };

        // Mask used to set an indicated bit to 0 value
        private static byte[] maskAnd =
        {
            0xFE, 0xFD, 0xFB, 0xF7, 0xEF, 0xDF, 0xBF, 0x7F    
        };
    }
}
