using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptography
{
    class BaseCryptography
    {
        protected readonly int keyLength = 128;
        protected readonly int blockLength = 128;
        protected readonly int roundCount = 10;

        protected void AddRoundKey( int round )
        {

        }
    }
}
