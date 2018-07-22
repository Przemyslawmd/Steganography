
using System.Drawing;
using System.Collections.Generic;

namespace Steganography
{
    class Uncovering : BaseCover
    {        
        public List< byte > UncoverData( Bitmap Image, out bool CompressFlag )
        {
            Color color;
            bitIterator = new BitIterator( 0 );

            for ( int x = 0; x < DataSizePixel; x++ )
            {
                color = Image.GetPixel( x, 0 );
                calculateBytesCount( color.R );
                calculateBytesCount( color.G );
                calculateBytesCount( color.B );
            }

            bytesCount >>= 1;

            List< byte > buffer = new List< byte >( bytesCount );
            CompressFlag = (( Image.GetPixel( CompressPixel, 0 ).R % 2 ) == 1 ) ? true : false;                      

            for ( int y = 1; y < Image.Height; y++ )
            {
                for ( int x = 0; x < Image.Width; x++ )
                {
                    color = Image.GetPixel( x, y );

                    if ( UncoverDataFromPixel( color.R, buffer ) == UncoverState.Completed )
                    {
                        return buffer;
                    }

                    if ( UncoverDataFromPixel( color.G, buffer ) == UncoverState.Completed )
                    {
                        return buffer;
                    }

                    if ( UncoverDataFromPixel( color.B, buffer ) == UncoverState.Completed )
                    {
                        return buffer;
                    }
                }
            }

            return buffer;
        }        
        
        /**************************************************************************************/
        /**************************************************************************************/

        private UncoverState UncoverDataFromPixel( byte componentRGB, List< byte > buffer )
        {
            if (( componentRGB % 2 ) == 1 )
            {
                byteValue |= MaskOne;
            }

            if ( ++bitIterator.Index > bitIterator.LastIndex )
            {
                buffer.Add( byteValue );

                if ( buffer.Count == bytesCount )
                {
                    return UncoverState.Completed;
                }

                byteValue = 0;
                bitIterator.Index = 0;
            }
            else
            {
                byteValue <<= 1;
            }

            return UncoverState.Uncompleted;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void calculateBytesCount( byte componentRGB )
        {
            bytesCount |= ( componentRGB & MaskOne );
            bytesCount <<= 1;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private BitIterator bitIterator;

        private enum UncoverState { Completed, Uncompleted };
    }
}

