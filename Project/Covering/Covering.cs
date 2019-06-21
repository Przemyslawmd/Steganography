
using System;
using System.Drawing;
using System.Collections.Generic;

namespace Steganography
{     
    class Covering
    {        
        public void CoverData( Bitmap Image, List< byte > inputStream, Boolean isCompress ) 
        {
            constData = new CoveringConst();
            bitIterator = new BitIterator();

            bytesToCover = new Stack< byte >(  constData.CoverMark );
            IteratePictureAndCoverData( Image, 0, constData.PixelCountForDataSize, 0, 1 );

            inputStream.Reverse();
            bytesToCover = new Containers().CreateByteStackFromInteger( inputStream.Count );
            bytesToCover.Pop();
            bitIterator.Reset();
            IteratePictureAndCoverData( Image, 0, constData.PixelCountForDataSize, constData.SecondRow, 2 );

            Color color = Image.GetPixel( constData.NumberCompressPixel, constData.SecondRow );
            int red = ( isCompress ) ? ( color.R | constData.MaskOne ) : ( color.R & MaskZero );
            Image.SetPixel( constData.NumberCompressPixel, constData.SecondRow, Color.FromArgb( red, color.G, color.B ));

            bytesToCover = new Stack< byte >( inputStream );
            bitIterator.Reset();
            IteratePictureAndCoverData( Image, 0, Image.Width, 2, Image.Height );
        }
        
        /**************************************************************************************/
        /**************************************************************************************/

        private void IteratePictureAndCoverData( Bitmap image, int startX, int stopX, int startY, int stopY )
        {
            for ( int y = startY; y < stopY; y++ )
            {
                for ( int x = startX; x < stopX; x++ )
                {
                    Color color = image.GetPixel( x, y );

                    if ( AllBytesCompleted() )
                    {
                        return;
                    }

                    int red = AdjustRGBComponent( color.R, currentByte );           
                               
                    if ( AllBytesCompleted() )
                    {
                        image.SetPixel( x, y, Color.FromArgb( red, color.G, color.B ));
                        return;
                    }

                    int green = AdjustRGBComponent( color.G, currentByte );         
                                      
                    if ( AllBytesCompleted() )
                    {
                        image.SetPixel( x, y, Color.FromArgb( red, green, color.B ));
                        return;
                    }

                    int blue = AdjustRGBComponent( color.B, currentByte );                    
                    image.SetPixel( x, y, Color.FromArgb( red, green, blue ));                 
                }            
            }            
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private bool AllBytesCompleted()
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
            bitIterator.DecrementIndex();
            if ((( value >> bitIterator.Index ) % 2 ) == 0 )
            {
                return componentRGB & MaskZero;
            }
            
            return componentRGB | constData.MaskOne;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private Stack< byte > bytesToCover;
        private const byte MaskZero = 0xFE;
        private CoveringConst constData;
        private BitIterator bitIterator;
        private byte currentByte;
    }
}

