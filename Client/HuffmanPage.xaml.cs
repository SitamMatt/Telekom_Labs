using Cwiczenie3;
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

namespace Client
{
    /// <summary>
    /// Logika interakcji dla klasy HuffmanPage.xaml
    /// </summary>
    public partial class HuffmanPage : Page
    {
        FileTransferClient client;
        FileTransferServer server;
        byte[] data;

        public HuffmanPage()
        {
            InitializeComponent();
        }

        private void ConnectToServer(object sender, RoutedEventArgs e)
        {
            if (client?.IsConnected ?? false)
            {
                //close connection
                ClientStatus.Text = "Status: Niepołączony";
                ClientConBtn.Content = "Połącz";
            }
            else
            {
                client = new FileTransferClient();
                //try
                //{
                    client.Connect(ClientAddress.Text, Convert.ToInt32(ClientPort.Text));
                    ClientStatus.Text = "Status: Łączenie";
                    client.Connected += Client_Connected;
                //}catch(Exception)
                //{
                //    ClientStatus.Text = "Status: Niepołączony";
                //    ClientConBtn.Content = "Rozłącz";
                //}
                
            }
            
        }

        private void Client_Connected(object sender, EventArgs e)
        {
            Dispatcher.Invoke((Action)delegate
            {
                ClientStatus.Text = "Status: Połączony";
                ClientConBtn.Content = "Rozłącz";
            });
            
        }

        private void StartServer(object sender, RoutedEventArgs e)
        {
            server = new FileTransferServer(ServerAddress.Text, Convert.ToInt32(ServerPort.Text));
            server.ClientConnected += Server_ClientConnected;
            server.DataReceived += Server_DataReceived;
            server.Listen();
            ServerStatus.Text = "Status: Nasłuchuje";
        }

        private void Server_DataReceived(object sender, EventArgs e)
        {
            var server = (FileTransferServer)sender;
            data = server.buffer;
            Dispatcher.Invoke(delegate
            {
                DataStatus.Content = $"Odebrano: {data.Length} B";
                SaveBtn.IsEnabled = true;
            });
        }

        private void Server_ClientConnected(object sender, EventArgs e)
        {
         
            Dispatcher.Invoke(delegate
            {
                ServerStatus.Text = $"Status: Połączono";
            });
        }

        private void SendClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                var data = File.ReadAllBytes(dialog.FileName);
                client.Send(data);
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if(dialog.ShowDialog() == true)
            {
                File.WriteAllBytes(dialog.FileName, data);
            }
        }
    }
}
