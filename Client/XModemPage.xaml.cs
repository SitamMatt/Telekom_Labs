using System;
using System.Collections.Generic;
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
using System.IO.Ports;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// Logika interakcji dla klasy XModemPage.xaml
    /// </summary>
    public partial class XModemPage : Page
    {
        private Task task { get; set; }

        public XModemPage()
        {
            InitializeComponent();
            ComPortsSelector1.ItemsSource = SerialPort.GetPortNames();
            ComPortsSelector2.ItemsSource = SerialPort.GetPortNames();
            var serialPort = new SerialPort("COM1");
            serialPort.Open();
            task = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        string message = serialPort.ReadLine();
                        ReceiverTextBlock.Text += message;
                    }
                    catch (TimeoutException) { }
                }
            });
        }
    }
}
