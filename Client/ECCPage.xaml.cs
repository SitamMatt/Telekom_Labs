using Cwiczenie1;
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
using Cwiczenie1.BitFormatting;
using Cwiczenie1.CodingStream;

namespace Client
{
    /// <summary>
    /// Logika interakcji dla klasy ECCPage.xaml
    /// </summary>
    public partial class ECCPage : Page
    {
        InputType InputType { get; set; }

        CodingMode CodingMode { get; set; }

        OutputType OutputType { get; set; }

        OutputStyle OutputStyle { get; set; }

        public ECCPage()
        {
            InitializeComponent();
        }

        private void InputTypeChecked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            switch (rb.Content)
            {
                case "Ciąg bitów":
                    InputType = InputType.BinarySequence;
                    break;
                case "Plik":
                    InputType = InputType.File;
                    break;
                case "Bajt":
                    InputType = InputType.Byte;
                    break;
            }
        }

        private void CodingModeChecked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            switch (rb.Content)
            {
                case "Koduj-1":
                    CodingMode = CodingMode.kod1;
                    break;
                case "Koduj-2":
                    CodingMode = CodingMode.kod2;
                    break;
                case "Odkoduj-1":
                    CodingMode = CodingMode.odkod1;
                    break;
                case "Odkoduj-2":
                    CodingMode = CodingMode.odkod2;
                    break;
            }
        }

        private void OutputTypeChecked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            switch (rb.Content)
            {
                case "Plik":
                    OutputType = OutputType.File;
                    break;
                case "Konsola":
                    OutputType = OutputType.Console;
                    break;
            }
        }

        private void OutputStyleChecked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            switch (rb.Content)
            {
                case "Binarny":
                    OutputStyle = OutputStyle.Binary;
                    break;
                case "Bity Tekstowo":
                    OutputStyle = OutputStyle.TextualBitsSeq;
                    break;
            }
        }

        private void Run(object sender, RoutedEventArgs e)
        {
            Stream input = null;
            Stream output = null;
            BitFormatter formatter = null;
            CodingStream codingStream = null;

            switch (InputType)
            {
                case InputType.BinarySequence:
                    if(InputTextBox.Text.TryReadBinaryByte(out byte b))
                    {
                        input = new MemoryStream(new byte[1] { b });
                    }
                    break;
                case InputType.Byte:
                    input = new MemoryStream(new byte[1] { Convert.ToByte(InputTextBox.Text) });
                    break;
                case InputType.File:
                    if (File.Exists(InputTextBox.Text))
                    {
                        input = new FileStream(InputTextBox.Text, FileMode.Open, FileAccess.Read);
                    }
                    break;
            }

            switch (CodingMode)
            {
                case CodingMode.kod2:
                    codingStream = new DoubleCorrectionEncoder();
                    break;
                case CodingMode.odkod2:
                    codingStream = new DoubleCorrectionDecoder();
                    break;
                case CodingMode.kod1:
                    codingStream = new SingleCorrectionEncoder();
                    break;
                case CodingMode.odkod1:
                    codingStream = new SingleCorrectionDecoder();
                    break;
            }

            switch (OutputType)
            {
                case OutputType.File:
                    output = new FileStream(OutputTextBox.Text, FileMode.Create, FileAccess.ReadWrite);
                    break;
                case OutputType.Console:
                    output = Console.OpenStandardOutput();
                    break;
            }

            switch (OutputStyle)
            {
                case OutputStyle.Binary:
                    formatter = new RawBinaryFormatter();
                    break;
                case OutputStyle.TextualBitsSeq:
                    formatter = new BitFormatter();
                    break;
            }

            codingStream.Input = input;
            codingStream.Output = output;
            codingStream.Formatter = formatter;
            codingStream.StartFlow();
            input.Close();
            output.Close();
        }
    }
}
