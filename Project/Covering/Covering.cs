
using System.Drawing;
using System.Collections.Generic;


namespace Steganography
{     
    class Covering
    {
        public void CoverData( Bitmap bitmap, List<byte> inputStream, bool isCompress ) 
        {
            var bytesToCover = new Stack<byte>( Constants.CoverMark );
            Utils.BitmapRange range = new Utils.BitmapRange( 0, Constants.CountOfPixelsForDataSize, 0, 1 );
            IteratePictureAndCoverData( bitmap, range, bytesToCover );

            inputStream.Reverse();
            bytesToCover = new Utils().CreateByteStackFromInteger( inputStream.Count );
            bytesToCover.Pop();
            range = new Utils.BitmapRange( 0, Constants.CountOfPixelsForDataSize, 1, 2 );
            IteratePictureAndCoverData( bitmap, range, bytesToCover );

            Color color = bitmap.GetPixel( Constants.CompressionPixel, 1 );
            int red = ( isCompress ) ? ( color.R | Constants.MaskOne ) : ( color.R & MaskZero );
            bitmap.SetPixel( Constants.CompressionPixel, 1, Color.FromArgb( red, color.G, color.B ));

            bytesToCover = new Stack<byte>( inputStream );
            range = new Utils.BitmapRange( 0, bitmap.Width, 2, bitmap.Height ); 
            IteratePictureAndCoverData( bitmap, range, bytesToCover );
        }
        
        /**************************************************************************************/
        /**************************************************************************************/

        private void IteratePictureAndCoverData( Bitmap bitmap, Utils.BitmapRange range, Stack<byte> bytesToCover )
        {
            bitIterator.Reset();

            for ( int y = range.StartY; y < range.StopY; y++ )
            {
                for ( int x = range.StartX; x < range.StopX; x++ )
                {
                    Color color = bitmap.GetPixel( x, y );

                    if ( AllBytesCompleted( bytesToCover ) )
                    {
                        return;
                    }

                    int red = AdjustRGBComponent( color.R, currentByte );
                    if ( AllBytesCompleted( bytesToCover ) )
                    {
                        bitmap.SetPixel( x, y, Color.FromArgb( red, color.G, color.B ));
                        return;
                    }

                    int green = AdjustRGBComponent( color.G, currentByte );
                    if ( AllBytesCompleted( bytesToCover ) )
                    {
                        bitmap.SetPixel( x, y, Color.FromArgb( red, green, color.B ));
                        return;
                    }

                    int blue = AdjustRGBComponent( color.B, currentByte );
                    bitmap.SetPixel( x, y, Color.FromArgb( red, green, blue ));
                }
            }
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private bool AllBytesCompleted( Stack<byte> bytesToCover )
        {
            if ( bitIterator.Index == 0 )
            {
                if ( bytesToCover.Count == 0 )
                {
                    return true;
                }

                currentByte = bytesToCover.Pop();
            }
            return false;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private int AdjustRGBComponent( byte componentRGB, int value )
        {
            bitIterator.Decrement();
            if ((( value >> bitIterator.Index ) % 2 ) == 0 )
            {
                return componentRGB & MaskZero;
            }

            return componentRGB | Constants.MaskOne;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private const byte MaskZero = 0xFE;
        private readonly BitIterator bitIterator = new BitIterator();
        private byte currentByte;
    }
}

