
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stegan
{
    public partial class FormAbout : Form
    {
        public FormAbout( MenuStrip menuStrip )
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
                "<pre>Home page:        http://www.przemeknet.pl/Steganography</pre></body></html>";
            Form.Controls.Add( browser );

            Form.Disposed += ( object obj, EventArgs eventA ) => { menuStrip.Enabled = true; };
            menuStrip.Enabled = false;
            Form.Show();
        }
    }
}
