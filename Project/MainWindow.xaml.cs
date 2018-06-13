
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
            messages = new Messages();
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionOpenGraphic( object sender, RoutedEventArgs e )
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
            ChangeControlState( true, MenuSaveGraphic, MenuRemoveGraphic, MenuCoverText, MenuUncoverText, MenuUncoverFile, MenuSaveFile );
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

            Bitmap bitmap = GetBitmapFromImageSource( ControlImage.Source, encoder );
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
                ChangeControlState( true, MenuCoverFile );
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
            ShowMinorWindow( about );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionSettings( object sender, RoutedEventArgs e )
        {
            Window settings = new WindowSettings();
            ShowMinorWindow( settings );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionCoverText( object sender, RoutedEventArgs e )
        {
            if ( ControlText.Text.Equals( "" ) )
            {
                MessageBox.Show( "There is no typed text to hide" );
                return;
            }

            List< byte > data = new List<byte>( System.Text.Encoding.Unicode.GetBytes( ControlText.Text ) );
            CoverData( data );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionUncoverText( object sender, RoutedEventArgs e )
        {
            Bitmap bitmap = GetBitmapFromImageSource( ControlImage.Source, new BmpBitmapEncoder() );
            List< byte > data = Controller.UncoverData( bitmap, ref code );

            if ( data == null )
            {
                MessageBox.Show( messages.GetMessageText( code ) );
                return;
            }

            ControlText.Text = System.Text.Encoding.Unicode.GetString( data.ToArray() );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionCoverFile( object sender, RoutedEventArgs e )
        {
            if ( dataBuffer == null || dataBuffer.Count == 0 )
            {
                MessageBox.Show( "There is no loaded file to cover" );
                return;
            }

            CoverData( dataBuffer );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionUncoverFile( object sender, RoutedEventArgs e )
        {
            Bitmap bitmap = GetBitmapFromImageSource( ControlImage.Source, new BmpBitmapEncoder());
            dataBuffer = Controller.UncoverData( bitmap, ref code );

            if ( dataBuffer == null )
            {
                MessageBox.Show( messages.GetMessageText( code ) );
                return;
            }

            ControlData.Text = "Number of uncovered bytes: " + dataBuffer.Count.ToString();
        }
        
        /**************************************************************************************/
        /**************************************************************************************/

        private void CoverData( List< byte > data )
        {
            Bitmap bitmap = GetBitmapFromImageSource( ControlImage.Source, new BmpBitmapEncoder() );

            if ( Controller.CoverData( data, bitmap, ref code ) )
            {
                ControlImage.Source = GetBitmapSourceFromBitmap( bitmap );
                MessageBox.Show( "Data was covered in a graphic file successfully" );
            }
            else
            {
                MessageBox.Show( messages.GetMessageText( code ) );
            }
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionSaveUncoveredData( object sender, EventArgs e )
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save Uncovered Data as File";
            saveFileDialog.ShowDialog();

            if ( saveFileDialog.FileName.Equals( "" ) )
            {
                return;
            }

            FileInfo fileInfo = new FileInfo( saveFileDialog.FileName );
            FileStream fileStream = fileInfo.Open( FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None );
            fileStream.Write( dataBuffer.ToArray(), 0, dataBuffer.Count );
            fileStream.Close();
        }
        
        /**************************************************************************************/
        /**************************************************************************************/

        private void ShowMinorWindow( Window window )
        {
            Window settings = new WindowSettings();
            MenuMain.IsEnabled = false;
            window.Closed += MinorWindowClosed;
            window.Show();
        }

        /**************************************************************************************/
        /**************************************************************************************/

        public void MinorWindowClosed( object sender, System.EventArgs e )
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

        private Bitmap GetBitmapFromImageSource( ImageSource source, BitmapEncoder encoder )
        {
            encoder.Frames.Add( BitmapFrame.Create( (BitmapSource) source ) );
            MemoryStream outStream = new MemoryStream();
            encoder.Save( outStream );
            return new Bitmap( outStream );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private BitmapImage GetBitmapSourceFromBitmap( Bitmap bitmap )
        {
            MemoryStream memory = new MemoryStream();
            bitmap.Save( memory, System.Drawing.Imaging.ImageFormat.Bmp );
            memory.Position = 0;
            BitmapImage bitmapimage = new BitmapImage();
            bitmapimage.BeginInit();
            bitmapimage.StreamSource = memory;
            bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapimage.EndInit();
            return bitmapimage;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private Messages messages;
        private Messages.MessageCode code;
        private List< byte > dataBuffer;
    }
}

