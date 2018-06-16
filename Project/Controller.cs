
using SteganographyCompression;
using SteganographyEncryption;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Steganography
{
    class Controller
    {
        public static bool CoverData( List< byte > data, Bitmap bitmap, ref Messages.MessageCode code )
        {
            if ( Settings.Encryption )
            {
                string password = Settings.Password;

                if ( password.Equals( "" ) )
                {
                    code = Messages.MessageCode.NO_PASSWORD;
                    return false;                    
                }

                data = new Encryption().Encrypt( data, password );
            }
            
            if ( Settings.Compression )
            {
                data = new Compression().Compress( data );
            }
            
            if ( bitmap.Width < 7 )
            {
                code = Messages.MessageCode.TOO_LESS_WIDTH;
                return false;
            }

            int BitsInByte = 8;

            // Condition without a first row of bitmap because it's intented for metadata

            if ( ( data.Count * BitsInByte ) > ( ( bitmap.Height - 1 ) * bitmap.Width ))
            {
                code = Messages.MessageCode.TOO_MANY_DATA;
                return false; 
            }
           
            new Covering().CoverData( bitmap, data, Settings.Compression );
            return true;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        public static List< byte > UncoverData( Bitmap bitmap, ref Messages.MessageCode code )
        {
            bool compression;
            List< byte > data = new Uncovering().UncoverData( bitmap, out compression );

            if ( compression )
            {
                data = new Decompression().Decompress( data );
            }

            if ( Settings.Encryption )
            {
                string password = Settings.Password;

                if ( password.Equals( "" ) )
                {
                    code = Messages.MessageCode.NO_PASSWORD;
                    return null;
                }

                try
                {
                    data = new Decryption().Decrypt( data, password );
                }
                catch ( ExceptionEncryption exc )
                {
                    code = exc.code;
                    return null;
                }
                catch ( Exception )
                {
                    code = Messages.MessageCode.ERROR_DECRYPTION;
                    return null;
                }
            }

            return data;
        }
    }    
}

