using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Cryptography;

namespace Stegan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();            
        }        

        /************************************************************************************************************/ 
        /* OPEN A GRAPHIC FILE **************************************************************************************/

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

                    pictureBox.Image = bitmap;
                    heightImage = bitmap.Height;
                    widthImage = bitmap.Width;                    
                    EnableMenu( false, menuOpenGraphic );
                    EnableMenu( true, menuCoverText, menuUncoverFile, menuDiscoverText, menuSaveGraphic, menuRemoveGraphic );

                    if ( DataBuffer != null )
                        menuCoverFile.Enabled = true;
                 }
            }
            catch( Exception )
            {
                MessageBox.Show( "Loading graphic failed" );               
            }            
        }

        /*****************************************************************************************************************/
        /* SAVE GRAPHIC WITH HIDDEN DATA *********************************************************************************/

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
                MessageBox.Show( "An error because a new graphic is being saved with the same name as original name" );
            }
        }

        /****************************************************************************************************************************/
        /* REMOVE GRAPHIC ***********************************************************************************************************/

        private void RemoveGraphic( object sender, EventArgs e )
        {
            pictureBox.Image = null;
            pictureBox.Invalidate();
            widthImage = 0;
            heightImage = 0;
            EnableMenu( true, menuOpenGraphic );
            EnableMenu( false, menuSaveGraphic, menuRemoveGraphic, menuCoverText, menuCoverFile, menuUncoverFile, menuDiscoverText );         
        }    
        
        /****************************************************************************************************************/
        /* OPEN A FILE TO BE HIDDEN *************************************************************************************/
        /* Read data from a file and write it to a byte array ***********************************************************/
        
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

                    if (pictureBox.Image != null)
                        menuCoverFile.Enabled = true;
                }
            }
            catch( Exception )
            {
                MessageBox.Show( "Loading file failed" );
            }
        }

        /*******************************************************************************************************************/
        /* GET DATA FROM A FILE AND COVER IT  ******************************************************************************/
        
        private void StartCoverFile( object sender, EventArgs e )
        {            
            if ( FileBuffer == null )
            {
                MessageBox.Show( "No file to hide" );
                return;
            }            
            
            DataBuffer = new byte[FileBuffer.Length];
            System.Buffer.BlockCopy( FileBuffer, 0, DataBuffer, 0, DataBuffer.Length );
            CoverData();                      
        }

        /********************************************************************************************************************/
        /* GET DATA FROM A TEXTBOX CONTROL AND COVER IT  ********************************************************************/

        private void StartCoverText( object sender, EventArgs e )
        {
            if ( textControl.Text.Equals("") )
            {
                MessageBox.Show( "There is no text to hide" );
                return;
            }
            
            DataBuffer = new byte[textControl.Text.Length * sizeof(char)];         
            System.Buffer.BlockCopy( textControl.Text.ToCharArray(), 0, DataBuffer, 0, DataBuffer.Length );
            CoverData();                              
        }
        
        /******************************************************************************************************************/
        /* COVER DATA *****************************************************************************************************/

        private void CoverData()
        {
            if ( Settings.GetEncryptionState() )
            {
                string password = Settings.GetPassword();

                if ( password.Equals( "" ) )
                {
                    MessageBox.Show( "Encryption is checked, but password is empty" );
                    return; 
                } 

                try
                {
                    DataBuffer = new Encryption().Encrypt( DataBuffer, password );
                }
                catch ( Exception e )
                {
                    MessageBox.Show( e.Message.ToString() );
                    return;
                }
            }

            if ( Settings.GetCompressionState() )
            {                
                try
                {
                    DataBuffer = new Compression().Compress( DataBuffer );
                }
                catch ( Exception e )
                {
                    MessageBox.Show( e.Message.ToString() );
                    return;
                }
            }                  
                        
            // 8 value means bites in byte
            if (( DataBuffer.Length * 8 ) > (( heightImage - 1 ) * widthImage ))
            {
                MessageBox.Show( "Too many data to be hidden into a loaded graphic" );
                return;
            }

            Bitmap bitmap = (Bitmap)pictureBox.Image;            

            new Covering().CoverData( bitmap, DataBuffer, Settings.GetCompressionState() );
            pictureBox.Image = bitmap;
            pictureBox.Invalidate();
            MessageBox.Show( "Data was covered in a graphic file successfully" );
        }
        
        /**********************************************************************************************************************/
        /* START UNCOVERING PROCESS TO A FILE *********************************************************************************/
        
        private void StartUncoverFile( object sender, EventArgs e )
        {            
            if ( UncoverData() == false )
                return;

            FileBuffer = new byte[DataBuffer.Length];
            System.Buffer.BlockCopy( DataBuffer, 0, FileBuffer, 0, FileBuffer.Length );
            fileNameControl.Text = "Number of uncovered bytes: " + DataBuffer.Length.ToString();
            EnableMenu( true, menuRemoveData, menuSaveData );          
        }

        /************************************************************************************************************************/
        /* START UNCOVERING PROCESS TO TEXT CONTROL *****************************************************************************/

        private void StartUncoverText( object sender, EventArgs e )
        {
            if ( UncoverData() == false )
                return;

            textControl.Text = System.Text.Encoding.Unicode.GetString( DataBuffer );            
        }      
        
        /***********************************************************************************************************/
        /* UNCOVER DATA ********************************************************************************************/

        private Boolean UncoverData()
        {
            Bitmap bitmap = (Bitmap)pictureBox.Image;
            Boolean flagCompress = false;

            try
            {
                DataBuffer = new Uncovering().UncoverData( bitmap, ref flagCompress );
                if ( flagCompress )
                    DataBuffer = new Decompression().Decompress( DataBuffer );
            }
            catch ( Exception e )
            {
                MessageBox.Show( e.Message + e.Source + e.ToString() );
                return false;
            }

            if ( Settings.GetEncryptionState() )
            {
                string password = Settings.GetPassword();

                if ( password.Equals( "" ) )
                {
                    MessageBox.Show( "Encryption is checked, but password is empty" );
                    return false;
                }

                try
                {
                    DataBuffer = new Decryption().Decrypt( DataBuffer, password );
                }
                catch ( Exception e )
                {
                    MessageBox.Show( e.Message + e.Source + e.ToString() );
                    return false;
                }
            }

            return true;
        }
        
        /*********************************************************************************************************/
        /* SAVE UNCOVERED DATA ***********************************************************************************/

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

        /*********************************************************************************************************/
        /* REMOVE DATA UNCOVERED/TO HIDE *************************************************************************/

        private void RemoveData( object sender, EventArgs e )
        {
            DataBuffer = null;
            fileNameControl.Text = "";
            EnableMenu( true, menuOpenFile );
            EnableMenu( false, menuRemoveData, menuSaveData, menuCoverFile );                        
        }      
        
        /*************************************************************************************************************/
        /* REMOVE TEXT FROM CONTROL **********************************************************************************/

        private void removeText(object sender, EventArgs e)
        {
            textControl.Text = "";            
        }              

        /***************************************************************************************************************/
        /* ENABLE/DISABLE MENU *****************************************************************************************/

        private void EnableMenu( bool state, params ToolStripMenuItem[] menus )
        {
            foreach ( ToolStripMenuItem menu in menus )
                menu.Enabled = state;
        }        
               
        /*******************************************************************************************************************/
        /* ABOUT WINDOW ****************************************************************************************************/

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
                "<pre>Version 1.3</pre>" +
                "<pre>Author:           Przemyslaw Madej, Warsaw 2017 </pre>" + 
                "<pre>Email:            przemyslawmd@gmail.com</pre>" +
                "<pre>Home page:        http://www.przemeknet.pl/steganEn.aspx</pre></body></html>"; 
            Form.Controls.Add(browser);

            Form.Disposed += ( object obj, EventArgs eventA ) => { menuStripOne.Enabled = true; };
            menuStripOne.Enabled = false;
            Form.Show();
        }     
               
        /*******************************************************************************************************************************/
        /*******************************************************************************************************************************/
        
        private int heightImage;
        private int widthImage;
                                  
        const String htmlBegin = "<html><body style='background-color:white; font-size:11px; font-family:Verdana; line-height:180%; margin:25px; margin-left:18px;'>";
        
        byte[] FileBuffer;
        byte[] DataBuffer;   // Buffer for bytes to be hidden              
    }
}

