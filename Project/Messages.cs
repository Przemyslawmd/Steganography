
using System;
using System.Collections.Generic;
using System.Text;

namespace Stegan
{
    class Messages
    {
        public enum MessageCode
        {
            NO_PASSWORD,
            ERROR_ENCRYPTION,
            ERROR_DECRYPTION,
            ERROR_DECRYPTION_ALIGNMENT,
            ERROR_COMPRESSION,
            ERROR_DECOMPRESSION,
            TOO_MANY_DATA
        };

        public Messages()
        {
            messages = new Dictionary<MessageCode, string>();

            messages.Add( MessageCode.NO_PASSWORD, "Encryption is choosen, but password is empty." );
            messages.Add( MessageCode.ERROR_ENCRYPTION, "An error while data encryption." );
            messages.Add( MessageCode.ERROR_DECRYPTION, "An error while data decryption." );
            messages.Add( MessageCode.ERROR_DECRYPTION_ALIGNMENT,   "Improper data to be decrypted, " +
                                                                    "size of data to be decrypted must be divided by 16." );
            messages.Add( MessageCode.ERROR_COMPRESSION, "An error while data compression." );
            messages.Add( MessageCode.ERROR_DECOMPRESSION, "An error while data decompression." );
            messages.Add( MessageCode.TOO_MANY_DATA, "Too many data to be hidden into a loaded graphic." );
        }

        public String GetMessageText( MessageCode code )
        {
            return messages[code];
        }

        Dictionary<MessageCode, String> messages;
    }
}
