
using System;
using System.Drawing;
using System.Collections.Generic;

namespace Stegan
{
    class Uncovering : BaseCover
    {        
        /**********************************************************************************************************************************/
        /* UNCOVER DATA FROM AN IMAGE *****************************************************************************************************/

        public List<byte> UncoverData( Bitmap Image, ref bool CompressFlag )
        {
            Color color;
            
            // Get a size of covered data
            for ( int x = 0; x < DataSizePixel; x++ )
            {
                color = Image.GetPixel( x, 0 );

                byteCount |= ( color.R & MaskOne );
                byteCount <<= 1;

                byteCount |= ( color.G & MaskOne );
                byteCount <<= 1;

                byteCount |= ( color.B & MaskOne );

                if ( x != ( DataSizePixel - 1 ))
                    byteCount <<= 1;
            }

            List<byte> buffer = new List<byte>( byteCount );
            CompressFlag = (( Image.GetPixel( CompressPixel, 0 ).R % 2 ) == 1 ) ? true : false;                      

            // Uncover data
            for ( int y = 1; y < Image.Height; y++ )
            {
                for ( int x = 0; x < Image.Width; x++ )
                {
                    color = Image.GetPixel( x, y );

                    if ( UncoverDataFromPixel( color.R, buffer ) == false )
                        return buffer;

                    if ( UncoverDataFromPixel( color.G, buffer ) == false )
                        return buffer;

                    if ( UncoverDataFromPixel( color.B, buffer ) == false )
                        return buffer;
                }
            }
            return buffer;
        }        
        
        /*****************************************************************************************************************************/
        /* UNCOVER BIT FROM A PIXEL **************************************************************************************************/

        private bool UncoverDataFromPixel( byte componentRGB, List<byte> buffer )
        {
            if (( componentRGB % 2 ) == 1 )
                byteValue |= MaskOne;
            bitNumber++;

            if ( bitNumber == ( LastBit + 1 ))
            {
                buffer.Add( byteValue );

                // All bytes have been uncovered
                if ( buffer.Count == byteCount )
                    return false;

                byteValue = 0;
                bitNumber = 0;
            }
            else
            {
                byteValue <<= 1;
            }
            return true;
        }          
    }
}
