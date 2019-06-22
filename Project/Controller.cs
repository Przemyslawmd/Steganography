
using SteganographyCompression;
using SteganographyEncryption;
using System.Collections.Generic;
using System.Drawing;

namespace Steganography
{
    class Controller
    {
        public static Messages.MessageCode CoverData( List< byte > data, Bitmap bitmap )
        {
            if ( bitmap.Width < 7 )
            {
                return Messages.MessageCode.NOT_ENOUGH_BITMAP_WIDTH;
            }

            if ( Settings.Compression )
            {
                data = new Compression().MakeCompressedStream( data );
            }

            if ( Settings.Encryption )
            {
                if ( Settings.Password.Equals( "" ) )
                {
                    return Messages.MessageCode.NO_PASSWORD;
                }

                data = new Encryption().Encrypt( data, Settings.Password );
            }

            int BitsInByte = 8;
            if (( data.Count * BitsInByte ) > ( ( bitmap.Height - 1 ) * bitmap.Width ))
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

            if ( data is null )
            {
                return null;
            }

            if ( Settings.Encryption )
            {
                if ( Settings.Password.Equals( "" ) )
                {
                    code = Messages.MessageCode.NO_PASSWORD;
                    return null;
                }

                data = new Decryption().Decrypt( data, Settings.Password, ref code );
                if ( data is null )
                {
                    return null;
                }
            }

            return ( compression ) ? new Decompression().Decompress( data, ref code ) : data;
        }
    }    
}

