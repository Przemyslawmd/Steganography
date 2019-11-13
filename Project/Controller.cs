
using Steganography.Cryptography;
using System.Collections.Generic;
using System.Drawing;

namespace Steganography
{
    class Controller
    {
        public static Result CoverData( List< byte > data, Bitmap bitmap )
        {
            if ( bitmap.Width < 7 )
            {
                return Result.NOT_ENOUGH_BITMAP_WIDTH;
            }

            if ( Settings.Compression )
            {
                data = new Compression().MakeCompressedStream( data );
            }

            if ( Settings.Encryption )
            {
                data = new Encryption().Encrypt( data, Settings.Password );
            }

            if (( data.Count * ConstValues.BitsInByte ) > (( bitmap.Height - 1 ) * bitmap.Width ))
            {
                return Result.TOO_MANY_DATA;
            }
           
            new Covering().CoverData( bitmap, data, Settings.Compression );
            return Result.OK;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        public static List< byte > UncoverData( Bitmap bitmap, ref Result result )
        {
            bool compression = false;
            var data = new Uncovering().UncoverData( bitmap, ref compression, ref result );
            if ( data is null )
            {
                return null;
            }

            if ( Settings.Encryption )
            {
                data = new Decryption().Decrypt( data, Settings.Password, ref result );
                if ( data is null )
                {
                    return null;
                }
            }

            return ( compression ) ? new Decompression().Decompress( data, ref result ) : data;
        }
    }    
}

