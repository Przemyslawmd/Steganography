using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    class ExceptionEncryption : SystemException
    {
        public ExceptionEncryption( String message ) : base( message ) { }       
    }
}
