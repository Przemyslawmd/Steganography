
using System;
using System.Drawing;
using System.Collections.Generic;

namespace Steganography
{     
    class Covering
    {        
        public void CoverData( Bitmap Image, List< byte > dataToCover, Boolean isCompress ) 
        {
            Color color;
            int red, green, blue;
            dataToCover.Reverse();
            constValues = new CoveringConst();

            bitIterator = new BitIterator( -1 );
            dataToBeCovered = new Stack< byte >(  constValues.CoverMark );
            IteratePictureAndCoverData( Image, 0, constValues.DataSizePixel, 0, 1 );

            // Save data size to be covered                                                    
            /*
            dataToBeCovered = CreateByteStackFromNumber( dataToCover.Count );
            bitIterator.Index = -1;
            IteratePictureAndCoverData( Image, 0, constValues.DataSizePixel, constValues.SecondRow, 2 );
            */

            
            bitIterator = new BitIterator( 23 );
            for ( int x = 0; x < constValues.DataSizePixel  ; x++ )
            {	
                color = Image.GetPixel( x, constValues.SecondRow );
                red = AdjustRGBComponent( color.R, dataToCover.Count );
                green = AdjustRGBComponent( color.G, dataToCover.Count );
                blue = AdjustRGBComponent( color.B, dataToCover.Count );        
                Image.SetPixel( x, 1, Color.FromArgb( red, green, blue ) );
            } 
            

            color = Image.GetPixel( constValues.NumberCompressPixel, constValues.SecondRow );
            red = ( isCompress ) ? ( color.R | constValues.MaskOne ) : ( color.R & MaskZero );
            Image.SetPixel( constValues.NumberCompressPixel, constValues.SecondRow, Color.FromArgb( red, color.G, color.B ));

            dataToBeCovered = new Stack< byte >( dataToCover );
            bitIterator.Index = -1;
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
                if ( dataToBeCovered.Count == 0 )
                {
                    return true;
                }

                currentProcessedByte = dataToBeCovered.Pop();
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
            
            return componentRGB | constValues.MaskOne;
        }

        
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

        private Stack< byte > dataToBeCovered;
        private const byte MaskZero = 0xFE;
        private CoveringConst constValues;
        private BitIterator bitIterator;
        private byte currentProcessedByte;
    }
}

