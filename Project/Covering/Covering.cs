
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
            bytesCount = dataToCover.Count;
            dataToCover.Reverse();
            dataToBeCovered = new Stack< byte >( dataToCover );
            constValues = new CoveringConst();

            // Save data size to be covered                                                    
            // Number of bytes to be covered is stored in six pixels ( 18 bits )                                   
            bitIterator = new BitIterator( 17 );

            for ( int x = 0; x < constValues.DataSizePixel; x++ )
            {	
                color = Image.GetPixel( x, 0 );
                red = ChangeColorCoveringSize( color.R );
                green = ChangeColorCoveringSize( color.G );
                blue = ChangeColorCoveringSize( color.B );        
                Image.SetPixel( x, 0, Color.FromArgb( red, green, blue ) );
            }            

            // Save information whether data is compressed                         
            color = Image.GetPixel( constValues.CompressPixel, 0 );
            red = ( isCompress ) ? ( color.R | constValues.MaskOne ) : ( color.R & MaskZero );
            Image.SetPixel( constValues.CompressPixel, 0, Color.FromArgb( red, color.G, color.B ));

            // Cover data starting from a second row of a bitmap
            // Variable bitNumber has initial value less than zero to pop a byte from a stack
            bitIterator.Index = -1;
	
            for ( int y = 1; y < Image.Height; y++ )
            {
                for ( int x = 0; x < Image.Width; x++ )
                {
                    color = Image.GetPixel( x, y );

                    if ( CheckBitNumber() == false )
                    {
                        return;
                    }

                    red = ChangeColorCoveringData( color.R );           
                               
                    if ( CheckBitNumber() == false )
                    {
                        Image.SetPixel( x, y, Color.FromArgb( red, color.G, color.B ) );
                        return;
                    }

                    green = ChangeColorCoveringData( color.G );         
                                      
                    if ( CheckBitNumber() == false )
                    {
                        Image.SetPixel( x, y, Color.FromArgb( red, green, color.B ));
                        return;
                    }

                    blue = ChangeColorCoveringData( color.B );                    
                    Image.SetPixel( x, y, Color.FromArgb( red, green, blue ));                 
                }            
            }            
        }
        
        /**************************************************************************************/
        /**************************************************************************************/

        private bool CheckBitNumber()
        {
            if ( bitIterator.Index < 0 )
            {
                // All bytes have been covered
                if ( dataToBeCovered.Count == 0 )
                {
                    return false;
                }

                byteValue = dataToBeCovered.Pop();
                bitIterator.SetLastIndex();
            }
            return true;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private int ChangeColorCoveringSize( byte componentRGB )
        {
            if ((( bytesCount >> bitIterator.GetAndDecrementIndex() ) % 2 ) == 0 )
            {
                return componentRGB & MaskZero;
            }
            
            return componentRGB | constValues.MaskOne;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private int ChangeColorCoveringData( byte componentRGB )
        {
            if ((( byteValue >> bitIterator.GetAndDecrementIndex() ) % 2 ) == 0 )
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
        private int bytesCount;
        private byte byteValue;
    }
}

