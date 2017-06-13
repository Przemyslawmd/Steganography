﻿
using System;
using System.Collections.Generic;
using System.Text;
using Cryptography;
using System.Drawing;

namespace Stegan
{
    class Controller
    {
        /**********************************************************************************************/
        /** COVER DATA ********************************************************************************/

        public static bool CoverData( List<byte> data, Bitmap bitmap, Messages.MessageCode code )
        {
            if ( Settings.GetEncryptionState() )
            {
                string password = Settings.GetPassword();

                if ( password.Equals( "" ) )
                {
                    code = Messages.MessageCode.NO_PASSWORD;
                    return false;                    
                }

                try
                {
                    data = new Encryption().Encrypt( data, password );
                }
                catch ( Exception e )
                {
                    code = Messages.MessageCode.ERROR_ENCRYPTION;
                    return false;
                }
            }

            if ( Settings.GetCompressionState() )
            {
                try
                {
                    data = new Compression().Compress( data );
                }
                catch ( Exception e )
                {
                    code = Messages.MessageCode.ERROR_COMPRESSION;
                    return false;
                }
            }

            // This condition must be checked after potential compression
            // Value '8' means number of bites in a byte
            // First row of bitmap is intented to include metadata
            if ( ( data.Count * 8 ) > ( ( bitmap.Height - 1 ) * bitmap.Width ))
            {
                code = Messages.MessageCode.TOO_MANY_DATA;
                return false; 
            }
           
            new Covering().CoverData( bitmap, data, Settings.GetCompressionState() );
            return true;
        }

        /**********************************************************************************************/
        /* UNCOVER DATA *******************************************************************************/

        public static List<byte> UncoverData( Bitmap bitmap, Messages.MessageCode code )
        {
            Boolean flagCompress = false;
            List<byte> data = new List<byte>();

            try
            {
                data = new Uncovering().UncoverData( bitmap, ref flagCompress );
                if ( flagCompress )
                    data = new Decompression().Decompress( data );
            }
            catch ( Exception e )
            {
                code = Messages.MessageCode.ERROR_DECOMPRESSION;
                return null;
            }

            if ( Settings.GetEncryptionState() )
            {
                string password = Settings.GetPassword();

                if ( password.Equals( "" ) )
                {
                    code = Messages.MessageCode.NO_PASSWORD;
                    return null;
                }

                try
                {
                    data = new Decryption().Decrypt( data, password );
                }
                catch ( Exception e )
                {
                    code = Messages.MessageCode.ERROR_DECRYPTION;
                    return null;
                }
            }

            return data;
        }
    }    
}

