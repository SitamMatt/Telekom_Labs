using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telekom;

namespace Client
{
    /// <summary>
    /// Logika interakcji dla klasy ECCDecodingPage.xaml
    /// </summary>
    public partial class ECCDecodingPage : Page
    {
        public ECCDecodingPage()
        {
            InitializeComponent();
        }

        private string inputFileName;


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Stream source = null;
            if (FileSelectRadio.IsChecked.GetValueOrDefault())
            {
                source = new FileStream(inputFileName, FileMode.Open, FileAccess.Read);
            }
            else if (BitsSelectRadio.IsChecked.GetValueOrDefault())
            {
                source = new MemoryStream(BitsUtils.ToBinary(BitsTextBox.Text));
            }
            else
            {
                throw new Exception("Source not specified");
            }
            int mode = 0;
            if (DECCRadio.IsChecked.GetValueOrDefault())
                mode = 2;
            else if (SECCRadio.IsChecked.GetValueOrDefault())
                mode = 1;
            else
                throw new Exception("Mode not specified");

            if (FileOutputCheckBox.IsChecked.GetValueOrDefault())
            {
                SaveFileDialog dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    using (var fs = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write))
                    {
                        ECC.Decode(mode, source, fs);
                    }
                }
            }
            else
            {
                OutputText.Document.Blocks.Clear();
                byte[] encoded = Telekom.ECC.Decode(mode, source);
                for (int i = 0; i < encoded.Length; i++)
                {
                    OutputText.AppendText(Convert.ToString(encoded[i], 2).PadLeft(8, '0'));
                }
            }
            source.Close();
        }

        private void PickFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                inputFileName = dialog.FileName;
            }
        }
    }
}
