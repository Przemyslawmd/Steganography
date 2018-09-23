
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
            dataToBeCovered = new Stack< byte >( dataToCover );
            constValues = new CoveringConst();

            // Save data size to be covered                                                    
            // Number of bytes to be covered is stored in six pixels ( 18 bits )                                   
            bitIterator = new BitIterator( 17 );

            for ( int x = 0; x < constValues.DataSizePixel  ; x++ )
            {	
                color = Image.GetPixel( x, constValues.SecondPictureRow );
                red = AdjustRGBComponent( color.R, dataToCover.Count );
                green = AdjustRGBComponent( color.G, dataToCover.Count );
                blue = AdjustRGBComponent( color.B, dataToCover.Count );        
                Image.SetPixel( x, 1, Color.FromArgb( red, green, blue ) );
            }            

            // Save information whether data is compressed                         
            color = Image.GetPixel( constValues.CompressPixel, constValues.SecondPictureRow );
            red = ( isCompress ) ? ( color.R | constValues.MaskOne ) : ( color.R & MaskZero );
            Image.SetPixel( constValues.CompressPixel, constValues.SecondPictureRow, Color.FromArgb( red, color.G, color.B ));

            // Cover data starting from a second row of a bitmap
            // Variable bitNumber has initial value less than zero to pop a byte from a stack
            bitIterator.Index = -1;
	
            for ( int y = 2; y < Image.Height; y++ )
            {
                for ( int x = 0; x < Image.Width; x++ )
                {
                    color = Image.GetPixel( x, y );

                    if ( AllBytesCompleted() )
                    {
                        return;
                    }

                    red = AdjustRGBComponent( color.R, currentProcessedByte );           
                               
                    if ( AllBytesCompleted() )
                    {
                        Image.SetPixel( x, y, Color.FromArgb( red, color.G, color.B ));
                        return;
                    }

                    green = AdjustRGBComponent( color.G, currentProcessedByte );         
                                      
                    if ( AllBytesCompleted() )
                    {
                        Image.SetPixel( x, y, Color.FromArgb( red, green, color.B ));
                        return;
                    }

                    blue = AdjustRGBComponent( color.B, currentProcessedByte );                    
                    Image.SetPixel( x, y, Color.FromArgb( red, green, blue ));                 
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

        /**************************************************************************************/
        /**************************************************************************************/

        private Stack< byte > dataToBeCovered;

        private const byte MaskZero = 0xFE;

        private CoveringConst constValues;
        private BitIterator bitIterator;
        private byte currentProcessedByte;
    }
}

