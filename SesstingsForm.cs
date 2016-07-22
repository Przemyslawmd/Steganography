using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Stegan
{
    public partial class Form1 : Form
    {
        private void showSetting(object sender, EventArgs e)
        {
            Form settingForm = new Form();
            settingForm.Width = 360;
            settingForm.Height = 230;
            settingForm.Text = labMenu["set"];
            settingForm.MinimizeBox = false;
            settingForm.MaximizeBox = false;
            settingForm.ShowIcon = false;
            settingForm.FormBorderStyle = FormBorderStyle.FixedSingle;
                       

            /********* COMPRESSION ***********************************************************************/

            GroupBox gbCompress = new GroupBox();
            gbCompress.Location = new Point(10, 20);
            gbCompress.Width = 330;
            gbCompress.Height = 50;

            Label lbCompress = new Label();
            lbCompress.Text = labSettings["compress"];
            lbCompress.Width = 240;
            lbCompress.Location = new Point(5, 20);

            CheckBox ctCompress = new CheckBox();
            ctCompress.Location = new Point(310, 15);
            ctCompress.Width = 15;
            ctCompress.Checked = isCompress;

            gbCompress.Controls.Add(lbCompress);
            gbCompress.Controls.Add(ctCompress);

            /******** LANGUAGE ***************************************************************************/

            GroupBox gbLan = new GroupBox();
            gbLan.Location = new Point(10, 80);
            gbLan.Width = 330;
            gbLan.Height = 40;

            Label lbLan = new Label();
            lbLan.Text = labSettings["lan"];
            lbLan.Width = 60;
            lbLan.Location = new Point(15, 95);

            RadioButton rbPol = new RadioButton();
            rbPol.Text = "PL";
            rbPol.Width = 40;
            rbPol.Location = new Point(230, 92);

            RadioButton rbEng = new RadioButton();
            rbEng.Text = "ENG";
            rbEng.Width = 48;
            rbEng.Location = new Point(290, 92);

            if (isPolish)
                rbPol.Checked = true;
            else
                rbEng.Checked = true;

            /* ACCEPT ***************************************************************************************/

            Button btAccept = new Button();
            btAccept.Text = labSettings["accept"];
            btAccept.Location = new Point(140, 145);
            btAccept.Click += (object senderAc, EventArgs eAc) => { acceptSetting(ctCompress.Checked, rbPol.Checked); };
            btAccept.Click += (object senderAc, EventArgs aAc) => { settingForm.Dispose(); };
            
            settingForm.Controls.Add(lbLan);
            settingForm.Controls.Add(rbPol);
            settingForm.Controls.Add(rbEng);
            settingForm.Controls.Add(gbLan);
            settingForm.Controls.Add(gbCompress);
            settingForm.Controls.Add(btAccept);

            menuStripOne.Enabled = false;
            settingForm.Disposed += (object obj, EventArgs eventA) => { menuStripOne.Enabled = true; };
            settingForm.Show();
        }

        /**********************************************************************************************************************/
        /* ACCEPTS SETTINGS ***************************************************************************************************/

        private void acceptSetting(Boolean compress, Boolean polLan)
        {
            isCompress = compress;

            if (polLan) SetPolish();
            else SetEnglish();           
        }   
    }
}
