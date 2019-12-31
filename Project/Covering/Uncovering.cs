﻿
using System.Drawing;
using System.Collections.Generic;

namespace Steganography
{
    class Uncovering
    {
        public List< byte > UncoverData( Bitmap bitmap, ref bool compression, ref Result code )
        {
            if ( CheckCoveringMark( bitmap ) is false )
            {
                code = Result.IMPROPER_DATA_IN_PICTURE;
                return null;
            }
                        
            compression = (( bitmap.GetPixel( ConstValues.CompressionPixel, 1 ).R % 2 ) == 1 ) ? true : false;

            bitIterator.Reset();
            Utils.BitmapRange range = new Utils.BitmapRange(  0, ConstValues.CountOfPixelsForDataSize, 1, 2 ); 
            List< byte > buffer = IteratePictureAndUncoverData( bitmap, range, 0 );
            int bytesToProcess = new Utils().CreateIntegerFromByteList( buffer );
            range = new Utils.BitmapRange(  0, bitmap.Width, 2, bitmap.Height );
            return IteratePictureAndUncoverData( bitmap, range, bytesToProcess );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private List< byte > IteratePictureAndUncoverData( Bitmap bitmap, Utils.BitmapRange range, int bytesToProcess )
        {
            var buffer = new List< byte >( bytesToProcess );

            for ( int y = range.StartY; y < range.StopY; y++ )
            {
                for ( int x = range.StartX; x < range.StopX; x++ )
                {
                    Color color = bitmap.GetPixel( x, y );

                    if ( UncoverDataFromPixel( color.R, buffer, bytesToProcess ) == UncoverState.Completed )
                    {
                        return buffer;
                    }

                    if ( UncoverDataFromPixel( color.G, buffer, bytesToProcess ) == UncoverState.Completed )
                    {
                        return buffer;
                    }

                    if ( UncoverDataFromPixel( color.B, buffer, bytesToProcess ) == UncoverState.Completed )
                    {
                        return buffer;
                    }
                }
            }

            return buffer;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private UncoverState UncoverDataFromPixel( byte componentRGB, List< byte > buffer, int bytesToProcess )
        {
            if (( componentRGB % 2 ) == 1 )
            {
                currentByte |= ConstValues.MaskOne;
            }

            bitIterator.IncrementIndex();

            if ( bitIterator.Index == 0 )
            {
                buffer.Add( currentByte );
                currentByte = 0;

                if ( buffer.Count == bytesToProcess )
                {
                    return UncoverState.Completed;
                }
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
            Utils.BitmapRange range = new Utils.BitmapRange( 0, ConstValues.CountOfPixelsForDataSize, 0, 1 );
            List< byte > buffer = IteratePictureAndUncoverData( bitmap, range, 2 );

            return buffer[0] == ConstValues.CoverMark[1] && buffer[1] == ConstValues.CoverMark[0];
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private readonly BitIterator bitIterator = new BitIterator();
        private byte currentByte = 0;
        private enum UncoverState { Completed, Uncompleted };
    }
}

