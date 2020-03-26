using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using Telekom.Sockets;

namespace Client
{
    /// <summary>
    /// Logika interakcji dla klasy HuffmanPage.xaml
    /// </summary>
    public partial class HuffmanPage : Page
    {
        public HuffmanPage()
        {
            InitializeComponent();
        }

        private FileTransferClient client;
        private FileTransferServer server;
        private bool isClientConnected = false;
        private bool isServerConnected = false;

        private async void ConnectToServer(object sender, RoutedEventArgs e)
        {
            if (isClientConnected)
            {
                client.Close();
                ClientStatus.Text = "Status: Niepołączony";
                SendBtn.IsEnabled = false;
                ClientConBtn.Content = "Połącz";
                isClientConnected = false;
            }
            else
            {
                try
                {
                    isClientConnected = true;
                    client = new FileTransferClient(ClientAddress.Text, Convert.ToInt32(ClientPort.Text));
                    ClientStatus.Text = "Status: Łączenie...";
                    ClientConBtn.Content = "Przerwij";
                    await client.ConnectAsync();
                    ClientStatus.Text = "Status: Połączony";
                    SendBtn.IsEnabled = true;
                }
                catch (Exception)
                {
                    ClientStatus.Text = "Status: Niepołączony";
                    SendBtn.IsEnabled = false;
                    ClientConBtn.Content = "Połącz";
                    isClientConnected = false;
                }
            }
        }

        private async void SendClick(object sender, RoutedEventArgs e)
        {
            if (isClientConnected)
            {

                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    SendStatus.Text = "Wysyłanie pliku...";
                    await client.SendAsync(dialog.FileName);
                    SendStatus.Text = "Wysłano plik";

                }
            }
        }

        private async void SaveClick(object sender, RoutedEventArgs e)
        {
            if (isServerConnected)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    ReceiveStatus.Text = "Oczekiwanie na plik...";
                    await server.ReceiveAsync(dialog.FileName);
                    ReceiveStatus.Text = "Odebrano plik";
                }
            }
        }

        private async void StartServer(object sender, RoutedEventArgs e)
        {
            if (isServerConnected)
            {
                server.Close();
                ServerStatus.Text = "Status: Niepołączony";
                SaveBtn.IsEnabled = false;
                ServerBtn.Content = "Nasłuchuj";
                isServerConnected = false;
            }
            else
            {
                try
                {
                    isServerConnected = true;
                    server = new FileTransferServer(ServerAddress.Text, Convert.ToInt32(ServerPort.Text));
                    ServerStatus.Text = "Status: Nasłuchuje...";
                    ServerBtn.Content = "Przerwij";
                    await server.AcceptAsync();
                    ServerStatus.Text = "Status: Połączony";
                    SaveBtn.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    var a = ex.Message;
                    ServerStatus.Text = "Status: Niepołączony";
                    SaveBtn.IsEnabled = false;
                    ServerBtn.Content = "Nasłuchuj";
                    isServerConnected = false;
                }
            }
        }
    }
}
