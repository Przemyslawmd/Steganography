using System;
using System.Drawing;

namespace Stegan
{
    /*
     This is main class responsible for covering/uncovering data into/from an image
    */
     
    class Covering
    {        
        /***************************************************************************************************************************/
        /* COVERS DATA IN A IMAGE **************************************************************************************************/
        
        public void CoverData( ref Bitmap Image, byte[] dataToCover, Boolean isCompress ) 
        {         
            byteNumber = 0;	         
            byteCover = 0;
            dataCount = dataToCover.Length;

            // Saving data size                                                       
            // Number of bytes to be covered is stored in 6 pixels, therefore 18 bites is intended for data size                                    

            // Size is stored from last bit of three bytes structure
            bitNumber = 17;            
            
            for (int x = 0; x < 6; x++)
            {	
                color = Image.GetPixel(x, 0);
                red = ChangeColorCoveringSize( color.R );
                green = ChangeColorCoveringSize( color.G );
                blue = ChangeColorCoveringSize( color.B );        
                Image.SetPixel( x, 0, Color.FromArgb( red, green, blue ) );
            }            
            
            // Saving information whwther data is compressed                          
            // This information is included in a red component of seventh pixel in first row  

            color = Image.GetPixel( COMPRESS_PIXEL, 0 );
            red = ( isCompress ) ? ( color.R | MASK_1 ) : ( color.R & MASK_0 );          
            Image.SetPixel( COMPRESS_PIXEL, 0, Color.FromArgb( red, color.G, color.B));
            
            // Covering data                                        
            // it starts from second row of bitmap       
            
            // NumOfBit starts with value less than zero in order to get byte at the beginning
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
        /* CHECKS BIT NUMBER IN COVERING BYTE *****************************************************************************************/

        private bool CheckBitNumber( byte[] dataToCover )
        {
            if ( bitNumber < 0 )
            {
                if ( byteNumber == dataCount )                                    
                    return false;
                
                byteCover = dataToCover[byteNumber++];
                bitNumber = LAST_BIT;
            }
            return true;
        }

        /*****************************************************************************************************************************/
        /* CHANGES CHOOSEN RGB COMPONENT WHILE COVERING DATA SIZE ********************************************************************/

        private int ChangeColorCoveringSize( byte componentRGB )
        {
            if ((( dataCount >> bitNumber-- ) % 2 ) == 0)
                return componentRGB & MASK_0;
            else
                return componentRGB | MASK_1;           
        }

        /*****************************************************************************************************************************/
        /* CHANGES CHOOSEN RGB COMPONENT WHILE COVERING DATA *************************************************************************/

        private int ChangeColorCoveringData( byte componentRGB )
        {
            if ( (( byteCover >> bitNumber--) % 2) == 0 )
                return componentRGB & MASK_0;
            else
                return componentRGB | MASK_1;
        }            
                
        /*****************************************************************************************************************************/
        /*****************************************************************************************************************************/

        private int bitNumber;              // Number of bite in byte
        private readonly int LAST_BIT = 7;
        private int byteNumber;             // Number of byte in an array of data to be covered
        private int dataCount;              // Size of data to be covered
        private byte byteCover;             // Byte to be covered or unvovered, it depends on method
        private Color color;
        private int red;
        private int green;
        private int blue;
        private readonly byte MASK_1 = 1;
        private readonly byte MASK_0 = 0;
        private readonly int COMPRESS_PIXEL = 6; 
    }
}
