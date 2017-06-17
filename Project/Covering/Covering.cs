
using System;
using System.Drawing;
using System.Collections.Generic;

namespace Stegan
{     
    class Covering : BaseCover
    {        
        /***********************************************************************************************/
        /* COVER DATA INSIDE AN IMAGE ******************************************************************/
        
        public void CoverData( Bitmap Image, List<byte> dataToCover, Boolean isCompress ) 
        {
            Color color;
            byteCount = dataToCover.Count;
            dataToCover.Reverse();
            stack = new Stack<byte>( dataToCover );

            // Save data size to be covered                                                    
            // Number of bytes to be covered is stored in six pixels ( 18 bits )                                   
            bitNumber = 17;            
            
            for ( int x = 0; x < DataSizePixel; x++ )
            {	
                color = Image.GetPixel( x, 0 );
                red = ChangeColorCoveringSize( color.R );
                green = ChangeColorCoveringSize( color.G );
                blue = ChangeColorCoveringSize( color.B );        
                Image.SetPixel( x, 0, Color.FromArgb( red, green, blue ) );
            }            
            
            // Save information whether data is compressed                         
            color = Image.GetPixel( CompressPixel, 0 );
            red = ( isCompress ) ? ( color.R | Mask1 ) : ( color.R & Mask0 );          
            Image.SetPixel( CompressPixel, 0, Color.FromArgb( red, color.G, color.B ));
            
            // Cover data starting from a second row of a bitmap
            // Variable bitNumber has initial value less than zero to pop a byte from a stack
            bitNumber = -1;
	
            for ( int y = 1; y < Image.Height; y++ )
            {
                for ( int x = 0; x < Image.Width; x++ )
                {
                    color = Image.GetPixel(x, y);                             
                                        
                    if ( CheckBitNumber() == false )
                        return;

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
        
        /***********************************************************************************************/
        /* CHECK BIT NUMBER ****************************************************************************/
        // Check bit number in being covered byte

        private bool CheckBitNumber()
        {
            if ( bitNumber < 0 )
            {
                // All bytes have been covered
                if ( stack.Count == 0 )
                    return false;

                byteValue = stack.Pop();
                bitNumber = LastBit;
            }
            return true;
        }

        /***********************************************************************************************/
        /* CHANGE RGB COMPONENT WHILE COVERING DATA SIZE ***********************************************/

        private int ChangeColorCoveringSize( byte componentRGB )
        {
            if ((( byteCount >> bitNumber-- ) % 2 ) == 0 )
                return componentRGB & Mask0;
            
            return componentRGB | Mask1;           
        }

        /**********************************************************************************************/
        /* CHANGE RGB COMPONENT WHILE COVERING DATA ***************************************************/

        private int ChangeColorCoveringData( byte componentRGB )
        {
            if ( (( byteValue >> bitNumber-- ) % 2 ) == 0 ) 
                return componentRGB & Mask0;
            
            return componentRGB | Mask1;
        }

        /**********************************************************************************************/
        /**********************************************************************************************/

        // Data to be covered is arranged into stack
        Stack<byte> stack;
    }
}

