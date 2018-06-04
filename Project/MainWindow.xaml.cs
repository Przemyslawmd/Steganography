
using System;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Steganography
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void OpenGraphicFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = LoadGraphic.Header.ToString();
            open.Filter = "Graphical file|*.jpg; *.jpeg; *.gif; *.bmp; *.png";

            if ( open.ShowDialog() == false )
            {
                MessageBox.Show(" Loading graphic file failed" );
                return;
            }

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri( open.FileName );
            bitmap.EndInit();

            if ( bitmap.Width > imageBorder.Width || bitmap.Height > imageBorder.Height )
            {
                imageControl.Stretch = Stretch.Uniform;
            }
            else
            {
                imageControl.Stretch = Stretch.None;
            }

            imageControl.Source = bitmap;
        }
    }
}

