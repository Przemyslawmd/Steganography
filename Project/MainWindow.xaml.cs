
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void OpenGraphicFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = LoadGraphic.Header.ToString();
            open.Filter = "Graphical file|*.jpg; *.jpeg; *.gif; *.bmp; *.png";

            if ( open.ShowDialog() == false )
            {
                MessageBox.Show(" Loading graphic file failed" );
                return;
            }

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri( open.FileName );
            bitmap.EndInit();
            imageControl.Source = bitmap;
        }
    }
}
