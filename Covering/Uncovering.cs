using System;
using System.Drawing;

namespace Stegan
{
    class Uncovering : BaseCover
    {        
        /**********************************************************************************************************************************/
        /* UNCOVER DATA FROM AN IMAGE *****************************************************************************************************/

        public byte[] UncoverData( Bitmap Image, ref Boolean CompressFlag )
        {
            Color color;
            
            // Get a size of covered data
            for ( int x = 0; x < DATA_SIZE_PIXEL; x++ )
            {
                color = Image.GetPixel( x, 0 );

                if (( color.R % 2) == 1 )
                    byteCount |= MASK_1;
                byteCount <<= 1;

                if (( color.G % 2) == 1 )
                    byteCount |= MASK_1;
                byteCount <<= 1;

                if (( color.B % 2) == 1 )
                    byteCount |= MASK_1;

                if ( x != ( DATA_SIZE_PIXEL - 1 ))
                    byteCount <<= 1;
            }

            byte[] DataBuffer = new byte[byteCount];                      
            CompressFlag = (( Image.GetPixel( COMPRESS_PIXEL, 0 ).R % 2 ) == 1 ) ? true : false;                      

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

        private bool UncoverDataFromPixel( byte componentRGB, byte[] buffer )
        {
            if ( (componentRGB % 2) == 1 )
                byteValue |= MASK_1;
            bitNumber++;

            if ( bitNumber == ( LAST_BIT + 1 ))
            {
                buffer[byteNumber++] = byteValue;
                if ( byteNumber == byteCount )
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
