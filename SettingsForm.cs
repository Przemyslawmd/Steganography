using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Stegan
{
    public partial class Form1 : Form
    {
        private void showSetting( object sender, EventArgs e )
        {
            Form settingForm = new Form();
            settingForm.Width = 365;
            settingForm.Height = 180;
            settingForm.MinimizeBox = false;
            settingForm.MaximizeBox = false;
            settingForm.ShowIcon = false;
            settingForm.FormBorderStyle = FormBorderStyle.FixedSingle;
            
            // Compression //

            GroupBox groupCompress = CreateGroupBox( 50, 330, 10, 20 );
            CheckBox ctCompress = CreateCheckBox( 15, 300, 15, isCompress );
            
            groupCompress.Controls.Add( CreateLabel( "Data compression", 240, 5, 20 ) );
            groupCompress.Controls.Add( ctCompress );
                        
            // Accept //

            Button btAccept = new Button();
            btAccept.Text = "Accept";
            btAccept.Location = new Point( 140, 100 );
            btAccept.Click += ( object senderAc, EventArgs eAc) => { acceptSetting(ctCompress.Checked ); };
            btAccept.Click += ( object senderAc, EventArgs aAc) => { settingForm.Dispose(); };
                     
            settingForm.Controls.Add( groupCompress );
            settingForm.Controls.Add( btAccept );

            menuStripOne.Enabled = false;
            settingForm.Disposed += ( object obj, EventArgs eventA ) => { menuStripOne.Enabled = true; };
            settingForm.Show();
        }

        /**********************************************************************************************************************/
        /* ACCEPTS SETTINGS ***************************************************************************************************/

        private void acceptSetting( Boolean compress )
        {
            isCompress = compress;                       
        }

        /**************************************************************************************/
        /* HELPERS FOR CONTROLS CREATING ******************************************************/

        /* GENERAL ****************************************************************************/
    
        private void SetGeneralControl( Control control, int width, int x, int y )
        {
            control.Width = width;
            control.Location = new Point( x, y );
        }

        /* GROUPBOX ***************************************************************************/

        private GroupBox CreateGroupBox( int height, int width, int x, int y )
        {
            GroupBox group = new GroupBox();
            group.Height = height;
            SetGeneralControl( group, width, x, y );
            return group;
        }

        /* RADIO BUTTON ***********************************************************************/

        private RadioButton CreateRadio( String text, int width, int x, int y, bool state )
        {
            RadioButton radio = new RadioButton();
            SetGeneralControl( radio, width, x, y );
            radio.Text = text;            
            radio.Checked = state;
            return radio;
        }

        /* CHECKBOX ***************************************************************************/

        private CheckBox CreateCheckBox( int width, int x, int y, bool state )
        {
            CheckBox checkBox = new CheckBox();            
            SetGeneralControl( checkBox, width, x, y );
            checkBox.Checked = state;
            return checkBox;
        }

        /* LABEL *******************************************************************************/

        private Label CreateLabel( String text, int width, int x, int y ) 
        {
            Label label = new Label();
            SetGeneralControl( label, width, x, y );
            label.Text = text;           
            return label;
        } 
    }
}
