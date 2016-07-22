using System;
using System.Drawing;

namespace Steganografia
{
    class Graphic
    {
        /*************************************************************************************************/
        /* GETS BYTES FROM AN IMAGE **********************************************************************/

        public byte[] GetBytesFromGraphic( Bitmap image )
        {
            byte[] bytesFromImage = new byte[( image.Width * image.Height * 3 ) + 2];
            bytesFromImage[0] = (byte)image.Width;
            bytesFromImage[1] = (byte)image.Height;

            // Two first bytes are intented to store image width and height, as it's seen above
            // therefore bytes from graphic start from third element in an array
            int index = 2;
            
            for ( x = 0; x < image.Width; x++ )
            {
                for ( y = 0; y < image.Height; y++ )
                {
                    color = image.GetPixel( x, y );
                    bytesFromImage[index++] = color.R;
                    bytesFromImage[index++] = color.G;
                    bytesFromImage[index++] = color.B;                    
                }
            }
            return bytesFromImage;
        }

        /***********************************************************************************************/
        /* MAKES AN IMAGE FROM BYTES *******************************************************************/

        public Bitmap GetImage( byte[] bytes ) 
        {
            byte red;
            byte green;
            byte blue;
            width = (int)bytes[0];
            height = (int)bytes[1];
           
            // There is no hidden graphic, return 'garbage' graphic, it means an image without any sense
            if (( width * height * 3 + 2 ) != bytes.Length )
            {
                width = (int)Math.Sqrt(( double )bytes.Length / 3 );
                height = width;
            }
            
            discoveredImage = new Bitmap( width, height );

            int index = 2;
            for ( x = 0; x < width; x++ )
            {
                for ( y = 0; y < height; y++ )
                {
                    red = bytes[index++];                    
                    green = bytes[index++];                    
                    blue = bytes[index++];                    
                    color = Color.FromArgb( red, green, blue );
                    discoveredImage.SetPixel( x, y, color );
                }
            }
            return discoveredImage;
        }

        /*********************************************************************************************************************************************/
        /*********************************************************************************************************************************************/

        private Bitmap discoveredImage;        
        private int width;
        private int height;
        private int x;
        private int y;
        private Color color;    
    }
}
