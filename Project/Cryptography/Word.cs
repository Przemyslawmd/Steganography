
namespace SteganographyEncryption
{
    class Word
    {
        public Word( byte value_1, byte value_2, byte value_3, byte value_4 )
        {
            this.value_1 = value_1;
            this.value_2 = value_2;
            this.value_3 = value_3;
            this.value_4 = value_4;
        }


        public Word( Word word )
        {
            value_1 = word.value_1;
            value_2 = word.value_2;
            value_3 = word.value_3;
            value_4 = word.value_4;
        }


        public void Rotate()
        {
            byte temp = value_1;
            value_1 = value_2;
            value_2 = value_3;
            value_3 = value_4;
            value_4 = temp;
        }


        public void SubByte()
        {
            value_1 = BaseCryptography.GetSbox( value_1 );
            value_2 = BaseCryptography.GetSbox( value_2 );
            value_3 = BaseCryptography.GetSbox( value_3 );
            value_4 = BaseCryptography.GetSbox( value_4 );
        }


        public void XorInner( Word word )
        {
            value_1 = (byte) ( value_1 ^ word.value_1 );
            value_1 = (byte) ( value_1 ^ word.value_2 );
            value_1 = (byte) ( value_1 ^ word.value_3 );
            value_1 = (byte) ( value_1 ^ word.value_4 );
        }


        public Word XorOuter( Word word )
        {
            return new Word((byte) ( this.value_1 ^ word.value_1 ), 
                            (byte) ( this.value_2 ^ word.value_2 ), 
                            (byte) ( this.value_3 ^ word.value_3 ), 
                            (byte) ( this.value_4 ^ word.value_4 ));
        }


        public byte value_1;
        public byte value_2;
        public byte value_3;
        public byte value_4;
    }
}

