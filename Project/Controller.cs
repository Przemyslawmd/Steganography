
using SteganographyCompression;
using SteganographyEncryption;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Steganography
{
    class Controller
    {
        public static Messages.MessageCode CoverData( List< byte > data, Bitmap bitmap )
        {
            if ( Settings.Encryption )
            {
                string password = Settings.Password;

                if ( password.Equals( "" ) )
                {
                    return Messages.MessageCode.NO_PASSWORD;
                }

                data = new Encryption().Encrypt( data, password );
            }
            
            if ( Settings.Compression )
            {
                data = new Compression().MakeCompressedStream( data );
            }
            
            if ( bitmap.Width < 7 )
            {
                return Messages.MessageCode.TOO_LESS_WIDTH;
            }

            int BitsInByte = 8;

            // Condition without a first row of bitmap because it's intented for metadata

            if ( ( data.Count * BitsInByte ) > ( ( bitmap.Height - 1 ) * bitmap.Width ))
            {
                return Messages.MessageCode.TOO_MANY_DATA;
            }
           
            new Covering().CoverData( bitmap, data, Settings.Compression );
            return Messages.MessageCode.OK;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        public static List< byte > UncoverData( Bitmap bitmap, ref Messages.MessageCode code )
        {
            bool compression = false;
            List< byte > data = new Uncovering().UncoverData( bitmap, ref compression, ref code );

            if ( data == null )
            {
                return null;
            }

            if ( compression )
            {
                data = new Decompression().Decompress( data, ref code );
                if ( data == null )
                {
                    return null;
                }
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

