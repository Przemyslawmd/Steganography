
using System;
using System.Drawing;
using System.IO;
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

        private void ActionOpenGraphic(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = LoadGraphic.Header.ToString();
            open.Filter = "Graphical file|*.jpg; *.jpeg; *.gif; *.bmp; *.png";

            if ( open.ShowDialog() == false )
            {
                MessageBox.Show(" Loading graphic file failed" );
                return;
            }

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri( open.FileName );
            bitmapImage.EndInit();

            if ( bitmapImage.Width > imageBorder.ActualWidth || bitmapImage.Height > imageBorder.ActualHeight )
            {
                imageControl.Stretch = Stretch.Uniform;
            }
            else
            {
                imageControl.Stretch = Stretch.None;
            }

            imageControl.Source = bitmapImage;
            ChangeControlState( true, MenuSaveGraphic, MenuRemoveGraphic );
            MenuSaveGraphic.IsEnabled = true;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionSaveGraphic( object sender, RoutedEventArgs e )
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PNG|*.png|BMP|*.bmp";
            saveDialog.Title = "Save Graphic File";

            if ( saveDialog.ShowDialog() == false )
            {
                return;
            }

            if ( saveDialog.FileName == "" )
            {
                return;
            }

            BitmapEncoder encoder;
            if ( saveDialog.FilterIndex == 1 )
            {
                encoder = new PngBitmapEncoder();
            }
            else
            {
                encoder = new BmpBitmapEncoder();
            }

            encoder.Frames.Add( BitmapFrame.Create( (BitmapSource) imageControl.Source ));
            MemoryStream outStream = new MemoryStream();
            encoder.Save( outStream );
            Bitmap bitmap = new Bitmap( outStream );
            bitmap.Save( saveDialog.FileName );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionRemoveGraphic( object sender, RoutedEventArgs e )
        {
            imageControl.Source = null;
            ChangeControlState( false, MenuSaveGraphic, MenuRemoveGraphic );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ChangeControlState( bool state, params System.Windows.Controls.MenuItem[] menus )
        {
            foreach ( System.Windows.Controls.MenuItem menu in menus )
            {
                menu.IsEnabled = state;
            }
        }
    }
}

