
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
            OpenFileDialog openFile = new OpenFileDialog()
            {
                Title = MenuLoadGraphic.Header.ToString(),
                Filter = "Graphical file|*.jpg; *.jpeg; *.gif; *.bmp; *.png"
            };
            if ( openFile.ShowDialog() is false )
            {
                return;
            }

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri( openFile.FileName );
            bitmap.EndInit();

            ControlImage.Stretch = bitmap.Width > imageBorder.ActualWidth || bitmap.Height > imageBorder.ActualHeight ?
                                   Stretch.Uniform : Stretch.None;

            ControlImage.Source = bitmap;
            ChangeControlState( true, MenuSaveGraphic,
                                      MenuRemoveGraphic,
                                      MenuCoverText,
                                      MenuUncoverText,
                                      MenuUncoverFile,
                                      MenuSaveFile );
            ChangeControlState( false, MenuLoadGraphic );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionSaveGraphic( object sender, RoutedEventArgs e )
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "PNG|*.png|BMP|*.bmp",
                Title = "Save Graphic File"
            };
            if ( saveDialog.ShowDialog() == false || saveDialog.FileName == "" )
            {
                return;
            }

            BitmapEncoder encoder = ( saveDialog.FilterIndex == 1 ) ? (BitmapEncoder) new PngBitmapEncoder() : 
                                                                                      new BmpBitmapEncoder();

            Bitmap bitmap = GetBitmapFromImageSource( ControlImage.Source, encoder );
            bitmap.Save( saveDialog.FileName );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionRemoveGraphic( object sender, RoutedEventArgs e )
        {
            ControlImage.Source = null;
            ChangeControlState( false, MenuSaveGraphic,
                                       MenuRemoveGraphic,
                                       MenuCoverText,
                                       MenuCoverFile,
                                       MenuUncoverText,
                                       MenuUncoverFile );
            ChangeControlState( true, MenuLoadGraphic );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionLoadFile( object sender, RoutedEventArgs e )
        {
            OpenFileDialog open = new OpenFileDialog()
            {
                Title = MenuLoadFile.HeaderStringFormat
            };
            if ( open.ShowDialog() is false )
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
            ChangeControlState( true, MenuLoadFile );
            ChangeControlState( false, MenuRemoveData, MenuSaveFile, MenuCoverFile );
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

            var data = new List< byte >( System.Text.Encoding.Unicode.GetBytes( ControlText.Text ));
            CoverData( data );
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionUncoverText( object sender, RoutedEventArgs e )
        {
            Bitmap bitmap = GetBitmapFromImageSource( ControlImage.Source, new BmpBitmapEncoder() );
            List< byte > data = Controller.UncoverData( bitmap, ref result );

            if ( data is null )
            {
                MessageBox.Show( messages.GetMessageText( result ));
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
            Bitmap bitmap = GetBitmapFromImageSource( ControlImage.Source, new BmpBitmapEncoder() );
            dataBuffer = Controller.UncoverData( bitmap, ref result );

            if ( dataBuffer == null )
            {
                MessageBox.Show( messages.GetMessageText( result ) );
                return;
            }

            ControlData.Text = "Number of uncovered bytes: " + dataBuffer.Count.ToString();
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void ActionRemoveText( object sender, RoutedEventArgs e )
        {
            ControlText.Text = null;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void CoverData( List< byte > data )
        {
            Bitmap bitmap = GetBitmapFromImageSource( ControlImage.Source, new BmpBitmapEncoder() );

            result = Controller.CoverData( data, bitmap );
            if ( result == Result.OK )
            {
                ControlImage.Source = GetBitmapSourceFromBitmap( bitmap );
                MessageBox.Show( "Data has been covered in a graphic file successfully" );
            }
            else
            {
                MessageBox.Show( messages.GetMessageText( result ) );
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
            foreach ( var menu in menus )
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
        private Result result;
        private List< byte > dataBuffer;
    }
}

