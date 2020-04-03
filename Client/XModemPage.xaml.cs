using System;
using System.Windows;
using System.Windows.Controls;
using System.IO.Ports;
using System.Threading.Tasks;
using Microsoft.Win32;
using Telekom.XModem;
using System.Threading;

namespace Client
{
    /// <summary>
    /// Logika interakcji dla klasy XModemPage.xaml
    /// </summary>
    public partial class XModemPage : Page
    {
        private XModemClient inClient;
        private XModemClient outClient;
        private CancellationTokenSource receiverCTS;
        private CancellationTokenSource senderCTS;

        public XModemPage()
        {
            InitializeComponent();
            ComPortsSelector1.ItemsSource = SerialPort.GetPortNames();
            ComPortsSelector2.ItemsSource = SerialPort.GetPortNames();
        }

        private async void ReceiveClick(object sender, RoutedEventArgs e)
        {
            if (inClient?.IsOperationPending ?? false)
            {
                receiverCTS.Cancel();
                ReceiverStatus.Text = "Przerywanie operacji...";
            }
            else
            {
                receiverCTS = new CancellationTokenSource();
                SaveFileDialog dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    using (inClient = new XModemClient((string)ComPortsSelector1.SelectedItem, Convert.ToInt32(ComPortSpeed1.Text)))
                    {
                        ReceiveBtn.Content = "Przerwij";
                        ReceiverStatus.Text = "Oczekiwanie na odpowiedź";
                        ComPortsSelector1.IsEnabled = false;
                        try
                        {
                            await inClient.ReceiveAsync(dialog.FileName, receiverCTS.Token);
                            ReceiverStatus.Text = "Plik zapisany";
                        }
                        catch (OperationCanceledException)
                        {
                            ReceiverStatus.Text = "Operacja przerwana";
                        }
                        catch (ExceededAttemptsException)
                        {
                            ReceiverStatus.Text = "Przekroczono ilość prób";
                        }
                        ReceiveBtn.Content = "Odbierz";
                        ComPortsSelector1.IsEnabled = true;
                    }
                }
            }
        }

        private async void SendClick(object sender, RoutedEventArgs e)
        {
            if (outClient?.IsOperationPending ?? false)
            {
                senderCTS.Cancel();
                SenderStatus.Text = "Przerywanie operacji...";
            }
            else
            {
                senderCTS = new CancellationTokenSource();
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    using (outClient = new XModemClient((string)ComPortsSelector2.SelectedItem, Convert.ToInt32(ComPortSpeed2.Text)))
                    {
                        SendBtn.Content = "Przerwij";
                        SenderStatus.Text = "Oczekiwanie na sygnał";
                        ComPortsSelector2.IsEnabled = false;
                        try
                        {
                            await outClient.SendAsync(dialog.FileName, senderCTS.Token, CRCSend.IsChecked.GetValueOrDefault());
                            SenderStatus.Text = "Plik wysłany";
                        }
                        catch (OperationCanceledException)
                        {
                            SenderStatus.Text = "Operacja przerwana";
                        }
                        catch (ExceededAttemptsException)
                        {
                            SenderStatus.Text = "Przekroczono ilość prób";
                        }
                        catch (TimeoutException)
                        {
                            SenderStatus.Text = "Czas minął";
                        }
                        SendBtn.Content = "Wyślij";
                        ComPortsSelector2.IsEnabled = true;
                    }
                }
            }
        }
    }
}
