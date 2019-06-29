
using System;
using System.Collections.Generic;

namespace Steganography
{
    class Messages
    {
        public enum MessageCode
        {
            OK,
            ERROR_DECRYPTION_ALIGNMENT,
            TOO_MANY_DATA,
            NOT_ENOUGH_BITMAP_WIDTH,
            IMPROPER_DATA_IN_PICTURE
        };

        public Messages()
        {
            messages = new Dictionary< MessageCode, string >();

            messages.Add( MessageCode.ERROR_DECRYPTION_ALIGNMENT,   "Size of data to be decrypted must be divided by 16." );
            messages.Add( MessageCode.TOO_MANY_DATA,                "Too many data to be hidden into a loaded picture." );
            messages.Add( MessageCode.NOT_ENOUGH_BITMAP_WIDTH,      "Width of a picture must contains at least seven pixels.");
            messages.Add( MessageCode.IMPROPER_DATA_IN_PICTURE,     "No hidden data in a picture.");
        }

        public String GetMessageText( MessageCode code )
        {
            return messages[code];
        }

        Dictionary< MessageCode, String > messages;
    }
}

