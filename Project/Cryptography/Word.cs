
namespace Steganography.Cryptography
{
    class Word
    {
        public Word( byte byte_1, byte byte_2, byte byte_3, byte byte_4 )
        {
            this.byte_1 = byte_1;
            this.byte_2 = byte_2;
            this.byte_3 = byte_3;
            this.byte_4 = byte_4;
        }


        public Word( Word word )
        {
            byte_1 = word.byte_1;
            byte_2 = word.byte_2;
            byte_3 = word.byte_3;
            byte_4 = word.byte_4;
        }


        public void Rotate()
        {
            byte temp = byte_1;
            byte_1 = byte_2;
            byte_2 = byte_3;
            byte_3 = byte_4;
            byte_4 = temp;
        }


        public void SubByte()
        {
            byte_1 = Sbox.GetSbox( byte_1 );
            byte_2 = Sbox.GetSbox( byte_2 );
            byte_3 = Sbox.GetSbox( byte_3 );
            byte_4 = Sbox.GetSbox( byte_4 );
        }


        public void XorInner( Word word )
        {
            byte_1 = (byte) ( byte_1 ^ word.byte_1 );
            byte_1 = (byte) ( byte_1 ^ word.byte_2 );
            byte_1 = (byte) ( byte_1 ^ word.byte_3 );
            byte_1 = (byte) ( byte_1 ^ word.byte_4 );
        }


        public Word XorOuter( Word word )
        {
            return new Word((byte) ( byte_1 ^ word.byte_1 ), 
                            (byte) ( byte_2 ^ word.byte_2 ), 
                            (byte) ( byte_3 ^ word.byte_3 ), 
                            (byte) ( byte_4 ^ word.byte_4 ));
        }


        public byte byte_1;
        public byte byte_2;
        public byte byte_3;
        public byte byte_4;
    }
}

