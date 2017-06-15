using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Cryptography;
using System.Collections.Generic;
using System.Text;

namespace Stegan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            messages = new Messages();
        }        

        /*********************************************************************************************/
        /* OPEN A GRAPHIC FILE ***********************************************************************/

        private void OpenGraphicFile( object sender, EventArgs e )
        {        
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Title = menuOpenGraphic.Text;
                open.Filter = "Graphical file|*.jpg; *.jpeg; *.gif; *.bmp; *.png"; 
                
                if ( open.ShowDialog() == DialogResult.OK )
                {
                    Bitmap bitmap = new Bitmap( open.FileName );
                    
                    if (( bitmap.Height > 316 ) || ( bitmap.Width > 527 )) 
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    else
                        pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;

                    pictureBox.Image = new Bitmap( bitmap );
                    EnableMenu( false, menuOpenGraphic );
                    EnableMenu( true, menuCoverText, menuUncoverFile, menuDiscoverText, menuSaveGraphic, menuRemoveGraphic, menuCoverFile );
                 }
            }
            catch( Exception )
            {
                MessageBox.Show( "Loading graphic file failed" );
            }            
        }

        /*********************************************************************************************/
        /* SAVE GRAPHIC WITH HIDDEN DATA *************************************************************/

        private void SaveGraphic( object sender, EventArgs e )
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG|*.png|BMP|*.bmp";            
            saveFileDialog.Title = "Save Graphic File";
            saveFileDialog.ShowDialog();

            try
            {
                if ( saveFileDialog.FileName != "" )
                {
                    switch ( saveFileDialog.FilterIndex )
                    {
                        case 1:
                            pictureBox.Image.Save( saveFileDialog.FileName, ImageFormat.Png );
                            break;
                        case 2:
                            pictureBox.Image.Save( saveFileDialog.FileName, ImageFormat.Bmp );
                            break;
                    }
                }
            }
            catch ( Exception )
            {
                MessageBox.Show( "An error while saving graphic" );
            }
        }

        /*********************************************************************************************/
        /* REMOVE GRAPHIC ****************************************************************************/

        private void RemoveGraphic( object sender, EventArgs e )
        {
            pictureBox.Image = null;
            pictureBox.Invalidate();
            EnableMenu( true, menuOpenGraphic );
            EnableMenu( false, menuSaveGraphic, menuRemoveGraphic, menuCoverText, menuCoverFile, menuUncoverFile, menuDiscoverText );         
        }    
        
        /*********************************************************************************************/
        /* OPEN A FILE TO BE HIDDEN ******************************************************************/
        /* Read data from a file and write it to a byte array ****************************************/
        
        private void OpenFile( object sender, EventArgs e )
        {            
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Title = menuOpenFile.Text;
                if ( open.ShowDialog() == DialogResult.OK )
                {
                    FileStream fileStream = new FileStream( open.FileName, FileMode.Open, FileAccess.Read );
                    BinaryReader binary = new BinaryReader( fileStream );
                    long numBytes = new FileInfo( open.FileName ).Length;
                    FileBuffer = binary.ReadBytes( (int)numBytes );
                    FileInfo fileInfo = new FileInfo( open.FileName );
                    fileNameControl.Text = "A file was loaded: " + fileInfo.Name;
                    EnableMenu( true, menuRemoveData );
                    EnableMenu( false, menuOpenFile );

                    if ( pictureBox.Image != null )
                        menuCoverFile.Enabled = true;
                }
            }
            catch( Exception )
            {
                MessageBox.Show( "Loading file failed" );
            }
        }

        /*********************************************************************************************/
        /* GET DATA FROM A FILE TO COVER *************************************************************/
        
        private void StartCoverFile( object sender, EventArgs e )
        {            
            if ( FileBuffer == null )
            {
                MessageBox.Show( "There is no loaded file to hide" );
                return;
            }

            List<byte> data = new List<byte>( FileBuffer );
            CoverData( data );
        }

        /*********************************************************************************************/
        /* GET DATA FROM A TEXTBOX TO COVER IT  ******************************************************/

        private void StartCoverText( object sender, EventArgs e )
        {
            if ( textControl.Text.Equals("") )
            {
                MessageBox.Show( "There is no text to hide" );
                return;
            }

            List<byte> data = new List<byte>( Encoding.Unicode.GetBytes( textControl.Text ) );
            CoverData( data );
        }

        /*********************************************************************************************/
        /* COVER DATA ********************************************************************************/

        private void CoverData( List<byte> data )
        {
            Bitmap bitmap = (Bitmap)pictureBox.Image;

            if ( Controller.CoverData( data, bitmap, ref code ) )
            {
                pictureBox.Image = bitmap;
                pictureBox.Invalidate();
                MessageBox.Show( "Data was covered in a graphic file successfully" );
            }
            else
                MessageBox.Show( messages.GetMessageText( code ) );
        }

        /*********************************************************************************************/
        /* START UNCOVERING TO BE SAVED AS A FILE ****************************************************/
        
        private void StartUncoverFile( object sender, EventArgs e )
        {
            List<byte> data = Controller.UncoverData( (Bitmap)pictureBox.Image, ref code );

            if ( data == null )
            {
                MessageBox.Show( messages.GetMessageText( code ) );
                return;
            }

            FileBuffer = data.ToArray();
            fileNameControl.Text = "Number of uncovered bytes: " + data.Count.ToString();
            EnableMenu( true, menuRemoveData, menuSaveData );          
        }

        /*********************************************************************************************/
        /* START UNCOVER DATA TO BE DISPLAYED AS A TEXT **********************************************/

        private void StartUncoverText( object sender, EventArgs e )
        {
            List<byte> data = Controller.UncoverData( (Bitmap)pictureBox.Image, ref code );

            if ( data == null )
            {
                MessageBox.Show( messages.GetMessageText( code ) );
                return;
            }

            textControl.Text = System.Text.Encoding.Unicode.GetString( data.ToArray() );
        }
        
        /*********************************************************************************************/
        /* SAVE UNCOVERED DATA ***********************************************************************/

        private void SaveUncoveredData( object sender, EventArgs e )
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save Uncovered Data as File";
            saveFileDialog.ShowDialog();           

            if ( saveFileDialog.FileName != "" )
            {
                FileInfo fileInfo = new FileInfo( saveFileDialog.FileName );
                FileStream fileStream = fileInfo.Open( FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.None );
                fileStream.Write( FileBuffer, 0, FileBuffer.Length );                
                fileStream.Close();          
            }
        }

        /*********************************************************************************************/
        /* REMOVE DATA UNCOVERED/TO HIDE *************************************************************/

        private void RemoveData( object sender, EventArgs e )
        {
            dataBuffer = null;
            fileNameControl.Text = "";
            EnableMenu( true, menuOpenFile );
            EnableMenu( false, menuRemoveData, menuSaveData, menuCoverFile );                        
        }      
        
        /*********************************************************************************************/
        /* REMOVE TEXT FROM CONTROL ******************************************************************/

        private void removeText(object sender, EventArgs e)
        {
            textControl.Text = "";            
        }              

        /*********************************************************************************************/
        /* ENABLE/DISABLE MENU ***********************************************************************/

        private void EnableMenu( bool state, params ToolStripMenuItem[] menus )
        {
            foreach ( ToolStripMenuItem menu in menus )
                menu.Enabled = state;
        }        
               
        /*********************************************************************************************/
        /* ABOUT WINDOW ******************************************************************************/

        private void ShowAbout(object sender, EventArgs e)
        {
            Form Form = new Form();
            Form.Width = 470;
            Form.Height = 260;
            Form.BackColor = Color.White;
            Form.MaximizeBox = false;
            Form.MinimizeBox = false;
            Form.ShowIcon = false;
            Form.FormBorderStyle = FormBorderStyle.FixedSingle;
            
            WebBrowser browser = new WebBrowser();
            browser.Dock = DockStyle.Fill;
            browser.DocumentText = "<html><body style='font-size:11px; font-family: Arial; line-height:150%; margin-top:15px; margin-left:15px;'>" + 
                "<p style='font-weight:bold; font-size:12px; letter-spacing:2px;'>Steganography application</p>" + 
                "<pre>Version 1.3.1</pre>" +
                "<pre>Author:           Przemyslaw Madej, Warsaw 2017 </pre>" + 
                "<pre>Email:            przemyslawmd@gmail.com</pre>" +
                "<pre>Home page:        http://www.przemeknet.pl/steganEn.aspx</pre></body></html>"; 
            Form.Controls.Add(browser);

            Form.Disposed += ( object obj, EventArgs eventA ) => { menuStripOne.Enabled = true; };
            menuStripOne.Enabled = false;
            Form.Show();
        }

        /********************************************************************************************/
        /********************************************************************************************/

        
        byte[] FileBuffer;

        // Buffer for uncovered data that may be saved as a file
        List<byte> dataBuffer;
        Messages messages;
        Messages.MessageCode code;
    }
}

 