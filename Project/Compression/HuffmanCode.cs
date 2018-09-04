
using System;
using System.Collections.Generic;

namespace SteganographyCompression
{
    class HuffmanCode
    {
        public HuffmanCode()
        {
            tokens = new Stack< byte >();
        }

        public HuffmanCode( HuffmanCode code )
        {
            tokens = new Stack< byte >( code.tokens );
            length = code.length;
        }

        public Stack< byte > tokens { get; set; }

        public byte length { get; set; }
    }
}

