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
            dataCount = 0;
            bitNumber = 0;

            // Getting a size of covered data
            for ( int x = 0; x < 6; x++ )
            {
                color = Image.GetPixel( x, 0 );

                if ( (color.R % 2) == 1 )
                    dataCount |= MASK_1;
                dataCount <<= 1;

                if ( (color.G % 2) == 1 )
                    dataCount |= MASK_1;
                dataCount <<= 1;

                if ( (color.B % 2) == 1 )
                    dataCount |= MASK_1;

                if ( x != 5 ) dataCount <<= 1;
            }

            byte[] DataBuffer = new byte[dataCount];

                      
            CompressFlag = ((Image.GetPixel( COMPRESS_PIXEL, 0 ).R % 2) == 1) ? true : false;

            byteCover = 0;

            // Uncovering data
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
        /* UNCOVERS DATA FROM PIXEL **************************************************************************************************/

        private bool UncoverDataFromPixel( byte componentRGB, byte[] buffer )
        {
            if ( (componentRGB % 2) == 1 )
                byteCover |= MASK_1;
            bitNumber++;

            if ( bitNumber == (LAST_BIT + 1) )
            {
                buffer[byteNumber++] = byteCover;
                if ( byteNumber == dataCount )
                    return false;
                byteCover = 0;
                bitNumber = 0;
            }
            else
            {
                byteCover <<= 1;
            }
            return true;
        }

        /*****************************************************************************************************************************/
        /*****************************************************************************************************************************/

        private int bitNumber;              // Number of bite in byte
        private readonly int LAST_BIT = 7;
        private int dataCount;              // Size of data to be covered
        private byte byteCover;             // Byte to be covered or unvovered, it depends on method
        private Color color;
    }
}
