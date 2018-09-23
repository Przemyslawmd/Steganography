﻿
using System.Drawing;
using System.Collections.Generic;

namespace Steganography
{
    class Uncovering
    {        
        public List< byte > UncoverData( Bitmap Image, out bool CompressFlag, ref Messages.MessageCode code )
        {
            Color color;
            bitIterator = new BitIterator( 0 );
            constValues = new CoveringConst();

            List< byte > buffer = new List< byte >( 2 );
            countDataToProcessed = 2;
            for ( int x = 0; x < constValues.DataSizePixel; x++ )
            {
                color = Image.GetPixel( x, constValues.FirstRow );

                if ( UncoverDataFromPixel( color.R, buffer ) == UncoverState.Completed )
                {
                        break;
                }

                if ( UncoverDataFromPixel( color.G, buffer ) == UncoverState.Completed )
                {
                        break;
                }

                if ( UncoverDataFromPixel( color.B, buffer ) == UncoverState.Completed )
                {
                        break;
                }
            }

            if ( buffer[0] != constValues.CoverMark[1] && buffer[1] != constValues.CoverMark[0] )
            {
                CompressFlag = false;
                code = Messages.MessageCode.IMPROPER_DATA_IN_PICTURE;
                return null;
            }

            byteValue = 0;
            bitIterator = new BitIterator( 0 );
            countDataToProcessed = 0;
            for ( int x = 0; x < constValues.DataSizePixel; x++ )
            {
                color = Image.GetPixel( x, constValues.SecondRow );
                calculateBytesCount( color.R );
                calculateBytesCount( color.G );
                calculateBytesCount( color.B );
            }

            countDataToProcessed >>= 1;

            buffer = new List< byte >( countDataToProcessed );
            CompressFlag = (( Image.GetPixel( constValues.CompressPixel, constValues.SecondRow ).R % 2 ) == 1 ) ? true : false;

            for ( int y = 2; y < Image.Height; y++ )
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
                byteValue |= constValues.MaskOne;
            }

            if ( ++bitIterator.Index > bitIterator.LastIndex )
            {
                buffer.Add( byteValue );

                if ( buffer.Count == countDataToProcessed )
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
            countDataToProcessed |= ( componentRGB & constValues.MaskOne );
            countDataToProcessed <<= 1;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private CoveringConst constValues;
        private BitIterator bitIterator;
        private int countDataToProcessed;
        private byte byteValue;

        private enum UncoverState { Completed, Uncompleted };
    }
}

