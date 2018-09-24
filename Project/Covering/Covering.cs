
using System;
using System.Drawing;
using System.Collections.Generic;

namespace Steganography
{     
    class Covering
    {        
        public void CoverData( Bitmap Image, List< byte > inputStream, Boolean isCompress ) 
        {
            inputStream.Reverse();
            constData = new CoveringConst();
            bitIterator = new BitIterator();

            bytesToCover = new Stack< byte >(  constData.CoverMark );
            IteratePictureAndCoverData( Image, 0, constData.DataSizePixel, 0, 1 );

            bytesToCover = CreateByteStackFromNumber( inputStream.Count );
            // Three bytes are intented foran  input stream size, therefore last byte is unnecessary
            bytesToCover.Pop();
            bitIterator.Reset();
            IteratePictureAndCoverData( Image, 0, constData.DataSizePixel, constData.SecondRow, 2 );

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
            Color color;
            int red, green, blue;

            for ( int y = startY; y < stopY; y++ )
            {
                for ( int x = startX; x < stopX; x++ )
                {
                    color = image.GetPixel( x, y );

                    if ( AllBytesCompleted() )
                    {
                        return;
                    }

                    red = AdjustRGBComponent( color.R, currentProcessedByte );           
                               
                    if ( AllBytesCompleted() )
                    {
                        image.SetPixel( x, y, Color.FromArgb( red, color.G, color.B ));
                        return;
                    }

                    green = AdjustRGBComponent( color.G, currentProcessedByte );         
                                      
                    if ( AllBytesCompleted() )
                    {
                        image.SetPixel( x, y, Color.FromArgb( red, green, color.B ));
                        return;
                    }

                    blue = AdjustRGBComponent( color.B, currentProcessedByte );                    
                    image.SetPixel( x, y, Color.FromArgb( red, green, blue ));                 
                }            
            }            
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private bool AllBytesCompleted()
        {
            if ( bitIterator.Index < 0 )
            {
                if ( bytesToCover.Count == 0 )
                {
                    return true;
                }

                currentProcessedByte = bytesToCover.Pop();
                bitIterator.SetLastIndex();
            }
            return false;
        }
        
        /**************************************************************************************/
        /**************************************************************************************/

        private int AdjustRGBComponent( byte componentRGB, int value )
        {
            if ((( value >> bitIterator.GetAndDecrementIndex() ) % 2 ) == 0 )
            {
                return componentRGB & MaskZero;
            }
            
            return componentRGB | constData.MaskOne;
        }

        /**************************************************************************************/
        /**************************************************************************************/
        
        private Stack< byte > CreateByteStackFromNumber( int number )  
        {
            Stack< byte > stack = new Stack< byte >();

            for ( int i = 0; i < 4; i++ )
            {
                stack.Push( (byte) ( number >> ( i * 8 )));
            }

            return stack;
        }
        
        /**************************************************************************************/
        /**************************************************************************************/

        private Stack< byte > bytesToCover;
        private const byte MaskZero = 0xFE;
        private CoveringConst constData;
        private BitIterator bitIterator;
        private byte currentProcessedByte;
    }
}

