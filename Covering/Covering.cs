using System;
using System.Drawing;

namespace Stegan
{     
    class Covering : BaseCover
    {        
        /***************************************************************************************************************************/
        /* COVER DATA IN A IMAGE ***************************************************************************************************/
        
        public void CoverData( ref Bitmap Image, byte[] dataToCover, Boolean isCompress ) 
        {
            Color color;
            byteCount = dataToCover.Length;

            // Saving data size to be covered                                                    
            // Number of bytes to be covered is stored in 6 pixels, therefore 18 bites is intended for data size                                   
            bitNumber = 17;            
            
            for (int x = 0; x < DATA_SIZE_PIXEL; x++)
            {	
                color = Image.GetPixel(x, 0);
                red = ChangeColorCoveringSize( color.R );
                green = ChangeColorCoveringSize( color.G );
                blue = ChangeColorCoveringSize( color.B );        
                Image.SetPixel( x, 0, Color.FromArgb( red, green, blue ) );
            }            
            
            // Saving information whether data is compressed                         
            color = Image.GetPixel( COMPRESS_PIXEL, 0 );
            red = ( isCompress ) ? ( color.R | MASK_1 ) : ( color.R & MASK_0 );          
            Image.SetPixel( COMPRESS_PIXEL, 0, Color.FromArgb( red, color.G, color.B));
            
            // Covering data : it starts from a second row of a bitmap                   
            // Value of bitNumber starts with value less than zero in order to get a byte at the beginning
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
            return;
        }
        
        /*****************************************************************************************************************************/
        /* CHECK A BIT NUMBER IN COVERING BYTE ***************************************************************************************/

        private bool CheckBitNumber( byte[] dataToCover )
        {
            if ( bitNumber < 0 )
            {
                if ( byteNumber == byteCount )                                    
                    return false;
                
                byteValue = dataToCover[byteNumber++];
                bitNumber = LAST_BIT;
            }
            return true;
        }

        /*****************************************************************************************************************************/
        /* CHANGE CHOOSEN RGB COMPONENT WHILE COVERING DATA SIZE *********************************************************************/

        private int ChangeColorCoveringSize( byte componentRGB )
        {
            if ((( byteCount >> bitNumber-- ) % 2 ) == 0)
                return componentRGB & MASK_0;
            else
                return componentRGB | MASK_1;           
        }

        /*****************************************************************************************************************************/
        /* CHANGE CHOOSEN RGB COMPONENT WHILE COVERING DATA **************************************************************************/

        private int ChangeColorCoveringData( byte componentRGB )
        {
            if ( (( byteValue >> bitNumber--) % 2) == 0 )
                return componentRGB & MASK_0;
            else
                return componentRGB | MASK_1;
        }              
    }
}
