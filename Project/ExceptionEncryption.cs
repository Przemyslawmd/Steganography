
using System;
using Steganography;

namespace SteganographyEncryption
{
    class ExceptionEncryption : SystemException
    {
        public ExceptionEncryption( Messages.MessageCode code )
        {
            this.code = code;
        }

        public Messages.MessageCode code;
    }
}

