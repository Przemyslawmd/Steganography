using System;
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
            settingForm.Height = 290;
            settingForm.MinimizeBox = false;
            settingForm.MaximizeBox = false;
            settingForm.ShowIcon = false;
            settingForm.FormBorderStyle = FormBorderStyle.FixedSingle;
                  

            GroupBox groupCompression = CreateGroupBox( 10, 20, 50, 330 );
            CheckBox checkCompression = CreateCheckBox( 300, 15, 15, Settings.Compression );
            groupCompression.Controls.Add( checkCompression );
            groupCompression.Controls.Add( CreateLabel( 10, 20, "Data compression", 100 ) );


            GroupBox groupEncryption =  CreateGroupBox( 10, 90, 90, 330 );
            CheckBox checkEncryption = CreateCheckBox( 300, 15, 15, Settings.Encryption );
            TextBox textBoxEncryption = CreateTextBox( 213, 50, 100, Settings.Password );
            groupEncryption.Controls.Add( CreateLabel( 10, 20, "Data encryption", 100 ) );
            groupEncryption.Controls.Add( checkEncryption );
            groupEncryption.Controls.Add( CreateLabel( 10, 55, "Password", 100 ) );
            groupEncryption.Controls.Add( textBoxEncryption );
            

            Button buttonAccept = new Button();
            buttonAccept.Text = "Accept";
            buttonAccept.Location = new Point( 140, 210 );
            buttonAccept.Click += ( object senderAc, EventArgs eAc ) => { Settings.Compression = checkCompression.Checked; };
            buttonAccept.Click += ( object senderAc, EventArgs aAc ) => { Settings.Encryption = checkEncryption.Checked; };
            buttonAccept.Click += ( object senderAc, EventArgs aAc ) => { Settings.Password = textBoxEncryption.Text; };
            buttonAccept.Click += ( object senderAc, EventArgs aAc ) => { settingForm.Dispose(); };
                     
            settingForm.Controls.Add( groupCompression );
            settingForm.Controls.Add( groupEncryption );
            settingForm.Controls.Add( buttonAccept );

            menuStripOne.Enabled = false;
            settingForm.Disposed += ( object obj, EventArgs eventA ) => { menuStripOne.Enabled = true; };
            settingForm.Show();
        }                   

        /* GENERAL CONTROL SETTINGS**************************************************************/
    
        private void SetGeneralControl( int x, int y, int width, Control control )
        {
            control.Width = width;
            control.Location = new Point( x, y );
        }

        /* GROUPBOX ****************************************************************************/

        private GroupBox CreateGroupBox( int x, int y, int height, int width )
        {
            GroupBox group = new GroupBox();
            group.Height = height;
            SetGeneralControl( x, y, width, group );
            return group;
        }        

        /* CHECKBOX ****************************************************************************/

        private CheckBox CreateCheckBox( int x, int y, int width, bool state )
        {
            CheckBox checkBox = new CheckBox();            
            SetGeneralControl( x, y, width, checkBox );
            checkBox.Checked = state;
            return checkBox;
        }

        /* LABEL *******************************************************************************/

        private Label CreateLabel( int x, int y, String text, int width ) 
        {
            Label label = new Label();
            SetGeneralControl( x, y, width, label );
            label.Text = text;           
            return label;
        }

        /* TEXT BOX ****************************************************************************/

        private TextBox CreateTextBox( int x, int y, int width, string password )
        {
            TextBox textBox = new TextBox();
            SetGeneralControl( x, y, width, textBox );
            textBox.Text = password;
            return textBox;
        }
    }
}
