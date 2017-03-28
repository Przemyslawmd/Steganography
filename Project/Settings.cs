﻿using System;

namespace Steganography
{
    class Settings
    {
        public static bool GetCompressionState()
        {
            return isCompression;
        }

        public static void SetCompressionState( bool state )
        {
            isCompression = state;
        }

        public static bool GetEncryptionState()
        {
            return isEncryption;
        }

        public static void SetEncryptionState( bool state )
        {
            isEncryption = state;
        }

        private static bool isCompression;
        private static bool isEncryption;
    }
}
