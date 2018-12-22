
using System;
using System.Collections.Generic;

namespace Steganography
{
    class Messages
    {
        public enum MessageCode
        {
            OK,
            NO_PASSWORD,
            ERROR_DECRYPTION_ALIGNMENT,
            ERROR_DECOMPRESSION,
            TOO_MANY_DATA,
            NOT_ENOUGH_BITMAP_WIDTH,
            IMPROPER_DATA_IN_PICTURE
        };

        public Messages()
        {
            messages = new Dictionary< MessageCode, string >();

            messages.Add( MessageCode.NO_PASSWORD,                  "Encryption is choosen, but password is not provided." );
            messages.Add( MessageCode.ERROR_DECRYPTION_ALIGNMENT,   "Improper data to be decrypted, " +
                                                                    "size of data to be decrypted must be divided by 16." );
            messages.Add( MessageCode.ERROR_DECOMPRESSION,          "An error while data decompression." );
            messages.Add( MessageCode.TOO_MANY_DATA,                "Too many data to be hidden into a loaded procture." );
            messages.Add( MessageCode.NOT_ENOUGH_BITMAP_WIDTH,      "Width of picture must contains at least seven pixels.");
            messages.Add( MessageCode.IMPROPER_DATA_IN_PICTURE,     "No hidden data in a picture.");
        }

        public String GetMessageText( MessageCode code )
        {
            return messages[code];
        }

        Dictionary< MessageCode, String > messages;
    }
}

