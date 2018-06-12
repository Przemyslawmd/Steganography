
using System.Windows;

namespace Steganography
{
    public partial class WindowSettings : Window
    {
        public WindowSettings()
        {
            InitializeComponent();
            IsCompression.IsChecked = Settings.Encryption;
            IsEncryption.IsChecked = Settings.Compression;
            Password.Text = Settings.Password;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionAccept( object sender, RoutedEventArgs e )
        {
            Settings.Encryption = (bool) IsCompression.IsChecked;
            Settings.Compression = (bool) IsEncryption.IsChecked;
            Settings.Password = Password.Text;
            Close();
        }
    }
}
