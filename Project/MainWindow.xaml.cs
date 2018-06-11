
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Collections.Generic;
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
            open.Title = MenuLoadGraphic.Header.ToString();
            open.Filter = "Graphical file|*.jpg; *.jpeg; *.gif; *.bmp; *.png";

            if ( open.ShowDialog() == false )
            {
                return;
            }

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri( open.FileName );
            bitmapImage.EndInit();

            if ( bitmapImage.Width > imageBorder.ActualWidth || bitmapImage.Height > imageBorder.ActualHeight )
            {
                ControlImage.Stretch = Stretch.Uniform;
            }
            else
            {
                ControlImage.Stretch = Stretch.None;
            }

            ControlImage.Source = bitmapImage;
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

            if ( saveDialog.ShowDialog() == false || saveDialog.FileName == "" )
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

            encoder.Frames.Add( BitmapFrame.Create( (BitmapSource) ControlImage.Source ));
            MemoryStream outStream = new MemoryStream();
            encoder.Save( outStream );
            Bitmap bitmap = new Bitmap( outStream );
            bitmap.Save( saveDialog.FileName );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionRemoveGraphic( object sender, RoutedEventArgs e )
        {
            ControlImage.Source = null;
            ChangeControlState( false, MenuSaveGraphic, MenuRemoveGraphic );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionLoadFile( object sender, RoutedEventArgs e )
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = MenuLoadFile.HeaderStringFormat;

            if ( open.ShowDialog() == false )
            {
                return;
            }

            FileStream fileStream = new FileStream( open.FileName, FileMode.Open, FileAccess.Read );
            BinaryReader binary = new BinaryReader( fileStream );
            FileInfo fileInfo = new FileInfo( open.FileName );
            dataBuffer = new List< byte >( binary.ReadBytes( (int) fileInfo.Length ));

            ControlData.Text = "A file was loaded: " + fileInfo.Name + " : " + (dataBuffer.Count / 1024) + "kB";
            ChangeControlState( true, MenuRemoveData );
            ChangeControlState( false, MenuLoadFile );

            if ( ControlImage.Source != null )
            {
                ChangeControlState( true, MenuCoverData );
            }
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionRemoveData( object sender, RoutedEventArgs e )
        {
            dataBuffer.Clear();
            ControlData.Text = "";
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionAbout( object sender, RoutedEventArgs e )
        {
            Window about = new About();
            MenuMain.IsEnabled = false;
            about.Closed += WindowAboutClosed;
            about.Show();
        }

        /**************************************************************************************/
        /**************************************************************************************/

        public void WindowAboutClosed( object sender, System.EventArgs e )
        {
            MenuMain.IsEnabled = true;
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

        /**************************************************************************************/
        /**************************************************************************************/

        private List< byte > dataBuffer;
    }
}

