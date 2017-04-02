using System;
using System.Drawing;

namespace Stegan
{     
    class Covering : BaseCover
    {        
        /***************************************************************************************************************************/
        /* COVER DATA IN AN IMAGE **************************************************************************************************/
        
        public void CoverData( Bitmap Image, byte[] dataToCover, Boolean isCompress ) 
        {
            Color color;
            byteCount = dataToCover.Length;

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
            Image.SetPixel( CompressPixel, 0, Color.FromArgb( red, color.G, color.B));
            
            // Cover data starting from a second row of a bitmap                   
            // bitNumber variable has initial value less than zero in order to get a byte at the beginning
            bitNumber = -1;
	
            for ( int y = 1; y < Image.Height; y++ )
            {
                for ( int x = 0; x < Image.Width; x++ )
                {
                    color = Image.GetPixel(x, y);                             
                                        
                    if ( CheckBitNumber( dataToCover ) == false )                        
                        return;

                    red = ChangeColorCoveringData( color.R );           
                               
                    if ( CheckBitNumber( dataToCover ) == false )
                    {
                        Image.SetPixel( x, y, Color.FromArgb( red, color.G, color.B ) );
                        return;
                    }

                    green = ChangeColorCoveringData( color.G );         
                                      
                    if ( CheckBitNumber( dataToCover ) == false )
                    {
                        Image.SetPixel( x, y, Color.FromArgb( red, green, color.B ));
                        return;
                    }

                    blue = ChangeColorCoveringData( color.B );                    
                    Image.SetPixel( x, y, Color.FromArgb( red, green, blue ));                 
                }            
            }            
        }
        
        /*****************************************************************************************************************************/
        /* CHECK BIT NUMBER IN BYTE THAT IS BEING COVERED ****************************************************************************/

        private bool CheckBitNumber( byte[] dataToCover )
        {
            if ( bitNumber < 0 )
            {
                if ( byteNumber == byteCount )                                    
                    return false;
                
                byteValue = dataToCover[byteNumber++];
                bitNumber = LastBit;
            }
            return true;
        }

        /*****************************************************************************************************************************/
        /* CHANGE CHOOSEN RGB COMPONENT WHILE COVERING DATA SIZE *********************************************************************/

        private int ChangeColorCoveringSize( byte componentRGB )
        {
            if ((( byteCount >> bitNumber-- ) % 2 ) == 0 )
                return componentRGB & Mask0;
            
            return componentRGB | Mask1;           
        }

        /*****************************************************************************************************************************/
        /* CHANGE CHOOSEN RGB COMPONENT WHILE COVERING DATA **************************************************************************/

        private int ChangeColorCoveringData( byte componentRGB )
        {
            if ( (( byteValue >> bitNumber-- ) % 2 ) == 0 ) 
                return componentRGB & Mask0;
            
            return componentRGB | Mask1;
        }              
    }
}
