
using System;
using Stegan;

namespace Cryptography
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

