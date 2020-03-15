using Cwiczenie4;
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

namespace Client
{
    /// <summary>
    /// Logika interakcji dla klasy ACPage.xaml
    /// </summary>
    public partial class ACPage : Page
    {
        public ACPage()
        {
            InitializeComponent();
            RecordingDevicesSelector.ItemsSource = ACManager.Devices;
        }
    }
}
