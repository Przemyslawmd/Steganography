
using System.Windows;

namespace Steganography
{
    public partial class WindowSettings : Window
    {
        public WindowSettings()
        {
            InitializeComponent();
            IsCompression.IsChecked = Settings.Compression;
            IsEncryption.IsChecked = Settings.Encryption;
            Password.Text = Settings.Password;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionAccept( object sender, RoutedEventArgs e )
        {
            Settings.Compression = (bool) IsCompression.IsChecked;
            Settings.Encryption = (bool) IsEncryption.IsChecked;
            Settings.Password = Password.Text;
            Close();
        }
    }
}
