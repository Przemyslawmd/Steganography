
using System.Drawing;
using System.Collections.Generic;

namespace Steganography
{
    class Uncovering
    {
        public List< byte > UncoverData( Bitmap Image, ref bool compression, ref Result code )
        {
            if ( CheckCoveringMark( Image ) is false )
            {
                code = Result.IMPROPER_DATA_IN_PICTURE;
                return null;
            }

            compression = (( Image.GetPixel( ConstValues.CompressionPixel, ConstValues.SecondRow ).R % 2 ) == 1 ) ? true : false;

            currentByte = 0;
            bitIterator.Reset();
            countDataToProcess = 0;
            List< byte >buffer = IteratePictureAndUncoverData( Image, 0, ConstValues.CountOfPixelsForDataSize, 1, 2 );
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
                currentByte |= ConstValues.MaskOne;
            }

            bitIterator.IncrementIndex();

            if ( bitIterator.Index == 0 )
            {
                buffer.Add( currentByte );

                if ( buffer.Count == countDataToProcess )
                {
                    return UncoverState.Completed;
                }

                currentByte = 0;
            }
            else
            {
                currentByte <<= 1;
            }

            return UncoverState.Uncompleted;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private bool CheckCoveringMark( Bitmap bitmap )
        {
            countDataToProcess = 2;
            List< byte > buffer = IteratePictureAndUncoverData( bitmap, 0, ConstValues.CountOfPixelsForDataSize, 0, 1 );

            return buffer[0] == ConstValues.CoverMark[1] && buffer[1] == ConstValues.CoverMark[0];
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private readonly BitIterator bitIterator = new BitIterator();
        private int countDataToProcess;
        private byte currentByte;

        private enum UncoverState { Completed, Uncompleted };
    }
}

