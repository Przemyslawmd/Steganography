
using System;
using System.Collections.Generic;

namespace SteganographyCompression
{
    class HuffmanCode
    {
        public HuffmanCode()
        {
            tokens = new Stack< byte >();
            length = 0;
        }

        public HuffmanCode( byte symbol, Stack< byte > code, byte length )
        {
            this.symbol = symbol;
            this.tokens = new Stack< byte >( code );
            this.length = length;
        }

        public byte symbol { get; }

        public Stack< byte > tokens { get; }

        public byte length { get; set; }
    }
}

