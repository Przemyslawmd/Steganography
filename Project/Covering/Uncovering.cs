
using System.Drawing;
using System.Collections.Generic;

namespace Steganography
{
    class Uncovering
    {
        public List< byte > UncoverData( Bitmap Image, ref bool compression, ref Messages.MessageCode code )
        {
            bitIterator = new BitIterator();
            constData = new CoveringConst();

            if ( CheckCoveringMark( Image ) == false )
            {
                code = Messages.MessageCode.IMPROPER_DATA_IN_PICTURE;
                return null;
            }

            compression = (( Image.GetPixel( constData.NumberCompressPixel, constData.SecondRow ).R % 2 ) == 1 ) ? true : false;

            byteValue = 0;
            bitIterator.Reset();
            countDataToProcess = 0;
            List< byte >buffer = IteratePictureAndUncoverData( Image, 0, constData.PixelCountForDataSize, 1, 2 );
            countDataToProcess = new Containers().CreateIntegerFromByteList( buffer );

            return IteratePictureAndUncoverData( Image, 0, Image.Width, 2, Image.Height );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private List< byte > IteratePictureAndUncoverData( Bitmap image, int startX, int stopX, int startY, int stopY )
        {
            List< byte > buffer = new List< byte >( countDataToProcess );

            for ( int y = startY; y < stopY; y++ )
            {
                for ( int x = startX; x < stopX; x++ )
                {
                    Color color = image.GetPixel( x, y );

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
                byteValue |= constData.MaskOne;
            }

            bitIterator.IncrementIndex();

            if ( bitIterator.Index == 0 )
            {
                buffer.Add( byteValue );

                if ( buffer.Count == countDataToProcess )
                {
                    return UncoverState.Completed;
                }

                byteValue = 0;
            }
            else
            {
                byteValue <<= 1;
            }

            return UncoverState.Uncompleted;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private bool CheckCoveringMark( Bitmap bitmap )
        {
            countDataToProcess = 2;
            List< byte > buffer = IteratePictureAndUncoverData( bitmap, 0, constData.PixelCountForDataSize, 0, 1 );

            return buffer[0] == constData.CoverMark[1] && buffer[1] == constData.CoverMark[0];
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private CoveringConst constData;
        private BitIterator bitIterator;
        private int countDataToProcess;
        private byte byteValue;

        private enum UncoverState { Completed, Uncompleted };
    }
}

