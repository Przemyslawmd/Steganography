
namespace SteganographyEncryption
{
    class Word
    {
        public Word( byte value1, byte value2, byte value3, byte value4 )
        {
            this.value1 = value1;
            this.value2 = value2;
            this.value3 = value3;
            this.value4 = value4;
        }


        public Word( Word word )
        {
            value1 = word.value1;
            value2 = word.value2;
            value3 = word.value3;
            value4 = word.value4;
        }


        public void Rotate()
        {
            byte temp = value1;
            value1 = value2;
            value2 = value3;
            value3 = value4;
            value4 = temp;
        }


        public void SubByte()
        {
            value1 = BaseCryptography.GetSbox( value1 );
            value2 = BaseCryptography.GetSbox( value2 );
            value3 = BaseCryptography.GetSbox( value3 );
            value4 = BaseCryptography.GetSbox( value4 );
        }


        public void XorInner( Word word )
        {
            value1 = (byte) ( value1 ^ word.value1 );
            value1 = (byte) ( value1 ^ word.value2 );
            value1 = (byte) ( value1 ^ word.value3 );
            value1 = (byte) ( value1 ^ word.value4 );
        }


        public Word XorOuter( Word word )
        {
            return new Word((byte) ( this.value1 ^ word.value1 ), 
                            (byte) ( this.value2 ^ word.value2 ), 
                            (byte) ( this.value3 ^ word.value3 ), 
                            (byte) ( this.value4 ^ word.value4 ));
        }

        public byte value1;
        public byte value2;
        public byte value3;
        public byte value4;
    }
}

