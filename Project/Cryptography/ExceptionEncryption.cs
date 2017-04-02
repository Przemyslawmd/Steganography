using System;

namespace Cryptography
{
    class ExceptionEncryption : SystemException
    {
        public ExceptionEncryption( String message ) : base( message ) { }       
    }
}
