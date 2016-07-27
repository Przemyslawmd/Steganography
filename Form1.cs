using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using Compression;

namespace Stegan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label = new Labels();
            label.SetEng(ref labMenu, ref labMes, ref labAbout, ref labSettings);            
            SetLabels();
        }

        /*************************************************************************************************************/
        /* SET LANGUAGE **********************************************************************************************/

        private void SetPolish()
        {          
            isPolish = true;
            label.SetPol( ref labMenu, ref labMes, ref labAbout, ref labSettings );
            SetLabels();
        }
        
        /***********************************************************************************************************/

        private void SetEnglish()
        {           
            isPolish = false;
            label.SetEng( ref labMenu, ref labMes, ref labAbout, ref labSettings );
            SetLabels();
        }   

        /************************************************************************************************************/ 
        /* OPENS A GRAPHICAL FLE  ***********************************************************************************/

        private void OpenGraphicFile( object sender, EventArgs e )
        {            
            String dialogTitle = null;
            Bitmap bitmap = null;
            dialogTitle = labMenu["openG"];                     
            
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Title = dialogTitle;
                open.Filter = labMes["filterG"]; 
                
                if ( open.ShowDialog() == DialogResult.OK )
                {
                    bitmap = new Bitmap( open.FileName );
                    
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
            catch( Exception ex )
            {
                MessageBox.Show( labMes["failureG"] );               
            }
        }

        /*****************************************************************************************************************/
        /* SAVES GRAPHIC WITH HIDDEN DATA ********************************************************************************/

        private void SaveGraphic( object sender, EventArgs e )
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG|*.png|BMP|*.bmp";            
            saveFileDialog.Title = labMenu["saveG"];
            saveFileDialog.ShowDialog();

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

        /****************************************************************************************************************************/
        /* REMOVES GRAPHIC **********************************************************************************************************/

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
        /* OPENS A FILE TO BE HIDDEN ************************************************************************************/
        /* Reads data from a file to byte array *************************************************************************/
        
        private void OpenFile( object sender, EventArgs e )
        {            
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Title = labMenu["openFile"];
                if ( open.ShowDialog() == DialogResult.OK )
                {
                    FileStream fileStream = new FileStream( open.FileName, FileMode.Open, FileAccess.Read );
                    BinaryReader binary = new BinaryReader( fileStream );
                    long numBytes = new FileInfo( open.FileName ).Length;
                    FileBuffer = binary.ReadBytes( (int)numBytes );
                    FileInfo fileInfo = new FileInfo( open.FileName );
                    fileNameControl.Text = labMes["fileLoaded"] + fileInfo.Name;
                    EnableMenu( true, menuRemoveData );
                    EnableMenu( false, menuOpenFile );

                    if (pictureBox.Image != null)
                        menuCoverFile.Enabled = true;
                }
            }
            catch( Exception ex )
            {
                MessageBox.Show(labMes["failureF"]);
            }
        }

        /*******************************************************************************************************************/
        /* GETS DATA FROM FILE AND COVERS IT  ******************************************************************************/
        
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
        /* GETS DATA FROM TEXTBOX CONTROL AND COVERS IT  ********************************************************************/

        private void StartCoverText( object sender, EventArgs e )
        {
            if ( textControl.Text.Equals("") )
            {
                MessageBox.Show(labMes["noText"]);
                return;
            }
            
            DataBuffer = new byte[textControl.Text.Length * sizeof(char)];         
            System.Buffer.BlockCopy( textControl.Text.ToCharArray(), 0, DataBuffer, 0, DataBuffer.Length );
            CoverData();                              
        }
        
        /******************************************************************************************************************/
        /* COVERS DATA ****************************************************************************************************/

        private void CoverData()
        {
            if (isCompress)
            {                
                try
                {
                    DataBuffer = new Compress().CompressData( DataBuffer );
                }
                catch (Exception e)
                {
                    MessageBox.Show( e.Message.ToString() );
                    return;
                }
            }
            
            if (( DataBuffer.Length * 8 ) > (( heightImage - 1 ) * widthImage))
            {
                MessageBox.Show(labMes["toManyData"]);
                return;
            }

            Bitmap bitmap = (Bitmap)pictureBox.Image;            

            new Covering().CoverData( ref bitmap, DataBuffer, isCompress );
            pictureBox.Image = bitmap;
            pictureBox.Invalidate();
            MessageBox.Show( labMes["dataCovered"] );
        }
        
        /**********************************************************************************************************************/
        /* STARTS UNCOVERS TO FILE ********************************************************************************************/
        
        private void StartUncoverFile( object sender, EventArgs e )
        {            
            if ( UncoverData() == false )
                return;

            FileBuffer = new byte[DataBuffer.Length];
            System.Buffer.BlockCopy( DataBuffer, 0, FileBuffer, 0, FileBuffer.Length );
            fileNameControl.Text = labMes["numUncover"] + DataBuffer.Length.ToString();
            EnableMenu( true, menuRemoveData, menuSaveData );          
        }

        /************************************************************************************************************************/
        /* STARTS UNCOVERS DATA TO TEXT *****************************************************************************************/

        private void StartUncoverText( object sender, EventArgs e )
        {
            if ( UncoverData() == false )
                return;
            textControl.Text = System.Text.Encoding.Unicode.GetString( DataBuffer );
            return;
        }      
        
        /***********************************************************************************************************/
        /* UNCOVER DATA ********************************************************************************************/

        private Boolean UncoverData()
        {
            Bitmap bitmap = (Bitmap)pictureBox.Image;
            Boolean flagCompress = false;

            try
            {
                DataBuffer = new Covering().UncoverData( bitmap, ref flagCompress );
                if (flagCompress)
                    DataBuffer = new Decompress().decompressData( DataBuffer );
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        
        /*********************************************************************************************************/
        /* SAVE UNCOVERED DATA ***********************************************************************************/

        private void SaveUncoveredData( object sender, EventArgs e )
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = labMenu["saveFile"];
            saveFileDialog.ShowDialog();           

            if (saveFileDialog.FileName != "")
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
        /* REMOVES TEXT **********************************************************************************************/

        private void removeText(object sender, EventArgs e)
        {
            textControl.Text = "";
            return;
        }              

        /***************************************************************************************************************/
        /* ENABLE/DISABLE MENU *****************************************************************************************/

        private void EnableMenu( bool state, params ToolStripMenuItem[] menus )
        {
            foreach ( ToolStripMenuItem menu in menus )
                menu.Enabled = state;
        }        
               
        /*****************************************************************************************************************************/
        /* ABOUT WINDOW **************************************************************************************************************/

        private void ShowAbout(object sender, EventArgs e)
        {
            Form Form = new Form();
            Form.Width = 400;
            Form.Height = 260;
            Form.Text = labMenu["info"];
            Form.BackColor = Color.White;
            Form.MaximizeBox = false;
            Form.MinimizeBox = false;
            Form.ShowIcon = false;
            Form.FormBorderStyle = FormBorderStyle.FixedSingle;
            
            WebBrowser browser = new WebBrowser();
            browser.Dock = DockStyle.Fill;
            browser.DocumentText = "<html><body style='font-size:11px; font-family:Arial; line-height:150%; margin-top:15px; margin-left:15px;'>" + 
                "<p style='font-weight:bold; font-size:12px; letter-spacing:2px;'>version  1.1.6</p>" + 
                labAbout["description"] +  
                labAbout["author"] + 
                "<pre>Email:          przemyslawmd@gmail.com</pre>" +
                labAbout["web"] + "</body></html>"; 
            Form.Controls.Add(browser);

            Form.Disposed += (object obj, EventArgs eventA) => { menuStripOne.Enabled = true; };
            menuStripOne.Enabled = false;
            Form.Show();
        }       
        
        /******************************************************************************************************************************/
        /* SETS TEXT IN CONTROLS ******************************************************************************************************/

        private void SetLabels()
        {
            fileMenuStripOne.Text = labMenu["file"];
            menuOpenGraphic.Text = labMenu["openG"];
            menuOpenFile.Text = labMenu["openFile"];
            menuSaveGraphic.Text = labMenu["saveG"];
            menuSaveData.Text = labMenu["saveFile"];
            menuRemoveGraphic.Text = labMenu["remG"];
            menuRemoveData.Text = labMenu["remF"];
            menuClearText.Text = labMenu["remT"];

            actionMenuStripOne.Text = labMenu["act"];
            menuCoverFile.Text = labMenu["coverF"];
            menuCoverText.Text = labMenu["coverT"];            
            menuUncoverFile.Text = labMenu["uncoverF"];
            menuDiscoverText.Text = labMenu["uncoverT"];

            settingsMenuStripOne.Text = labMenu["set"];
            infoMenuStrip.Text = labMenu["info"];                       
        }        
        
        /*******************************************************************************************************************************/
        /*******************************************************************************************************************************/
        
        private int heightImage;
        private int widthImage;
        private Boolean isCompress = false; 
        private Boolean isPolish = false;

        Dictionary<String, String> labMenu = null;        
        Dictionary<String, String> labAbout = null;
        Dictionary<String, String> labSettings = null;
        Dictionary<String, String> labMes = null;
        
        Labels label;
        const String htmlBegin = "<html><body style='background-color:white; font-size:11px; font-family:Verdana; line-height:180%; margin:25px; margin-left:18px;'>";
        
        byte[] FileBuffer;
        byte[] DataBuffer;   // Buffer for bytes to be hidden              
    }
}

