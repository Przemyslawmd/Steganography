
using System;
using System.Collections.Generic;


namespace Steganography
{
    public enum Result
    {
        OK,
        ERROR_DECRYPTION_ALIGNMENT,
        IMPROPER_DATA_IN_PICTURE,
        NOT_ENOUGH_BITMAP_WIDTH,
        TOO_MANY_DATA
    };
    
    class Messages
    {
        public String GetMessageText( Result result )
        {
            return messages[result];
        }

        
        private readonly Dictionary<Result, string> messages = new Dictionary<Result, string>()
        {
            [Result.ERROR_DECRYPTION_ALIGNMENT] = "Size of data to be decrypted must be divided by 16.",
            [Result.TOO_MANY_DATA]              = "Too many data to be hidden into a loaded picture.",
            [Result.NOT_ENOUGH_BITMAP_WIDTH]    = "Width of a picture must contains at least seven pixels.",
            [Result.IMPROPER_DATA_IN_PICTURE]   = "No hidden data in a picture."
        };
    }
}

