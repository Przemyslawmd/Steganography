using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stegan
{
    class BaseCover
    {
        protected int byteNumber;             // Number of byte in a stream of data to be covered/uncovered
        protected int red;
        protected int green;
        protected int blue;
        protected readonly byte MASK_1 = 1;
        protected readonly byte MASK_0 = 0;
        protected readonly int COMPRESS_PIXEL = 6;
    }
}
