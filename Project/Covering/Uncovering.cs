using System;
using System.Drawing;
using System.Collections.Generic;

namespace Stegan
{
    class Uncovering : BaseCover
    {        
        /**********************************************************************************************************************************/
        /* UNCOVER DATA FROM AN IMAGE *****************************************************************************************************/

        public List<byte> UncoverData( Bitmap Image, ref Boolean CompressFlag )
        {
            Color color;
            
            // Get a size of covered data
            for ( int x = 0; x < DataSizePixel; x++ )
            {
                color = Image.GetPixel( x, 0 );

                byteCount |= ( color.R & Mask1 );
                byteCount <<= 1;

                byteCount |= ( color.G & Mask1 );
                byteCount <<= 1;

                byteCount |= ( color.B & Mask1 );

                if ( x != ( DataSizePixel - 1 ))
                    byteCount <<= 1;
            }

            List<byte> DataBuffer = new List<byte>( byteCount );
            CompressFlag = (( Image.GetPixel( CompressPixel, 0 ).R % 2 ) == 1 ) ? true : false;                      

            // Uncover data
            for ( int y = 1; y < Image.Height; y++ )
            {
                for ( int x = 0; x < Image.Width; x++ )
                {
                    color = Image.GetPixel( x, y );

                    if ( UncoverDataFromPixel( color.R, DataBuffer ) == false )
                        return DataBuffer;

                    if ( UncoverDataFromPixel( color.G, DataBuffer ) == false )
                        return DataBuffer;

                    if ( UncoverDataFromPixel( color.B, DataBuffer ) == false )
                        return DataBuffer;
                }
            }
            return DataBuffer;
        }        
        
        /*****************************************************************************************************************************/
        /* UNCOVER BIT FROM A PIXEL **************************************************************************************************/

        private bool UncoverDataFromPixel( byte componentRGB, List<byte> buffer )
        {
            if ( (componentRGB % 2) == 1 )
                byteValue |= Mask1;
            bitNumber++;

            if ( bitNumber == ( LastBit + 1 ))
            {
                buffer.Add( byteValue );
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
