
namespace Stegan
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

        public static string GetPassword()
        {
            return encryptionPassword;
        }

        public static void SetPassword( string password )
        {
            encryptionPassword = password;
        }

        private static bool isCompression;
        private static bool isEncryption;
        private static string encryptionPassword;
    }
}
