
namespace SteganographyCompression
{
    class HuffmanCode
    {
        public HuffmanCode() { }

        public HuffmanCode( byte value, int code, byte codeLength )
        {
            this.value = value;
            this.code = code;
            this.codeLength = codeLength;
        }

        public byte value { get; set; }

        public int code { get; set; }

        public byte codeLength { get; set; }

        public void AddBitToCode( bool bitValue )
        {
            if ( codeLength != 0 )
            {
                code <<= 1;
            }

            code |= ( bitValue ) ? 1 : 0; 
            codeLength++;
        }

        public void RemoveLastBit()
        {
            code >>= 1;
            codeLength--;
        }
    }
}
