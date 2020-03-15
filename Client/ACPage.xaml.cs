using Cwiczenie4;
using Microsoft.Win32;
using SFML.Audio;
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
        public SoundBufferRecorder recorder;
        public bool IsRecording { get; set; } = false;
        public Sound sound;

        public ACPage()
        {
            InitializeComponent();
            // pobranie dostępnych urządzeń do nagrywania
            RecordingDevicesSelector.ItemsSource = SoundRecorder.AvailableDevices;
            RecordingDevicesSelector.SelectedItem = SoundRecorder.DefaultDevice;
            // inicjalizacja odtwarzacza i mikrofonu
            recorder = new SoundBufferRecorder();
            sound = new Sound();
        }

        private void Recording(object sender, RoutedEventArgs e)
        {
            if (IsRecording)
            {
                recorder.Stop();
                RecordBtn.Content = "Nagrywaj";
                IsRecording = false;
                SaveBtn.IsEnabled = true;
            }
            else
            {
                SaveBtn.IsEnabled = false;
                recorder.SetDevice((string)RecordingDevicesSelector.SelectedItem); // wybór urządzenia do nagrywania
                recorder.Start(); // rozpoczęcie nagrywania
                RecordBtn.Content = "Stop";
                IsRecording = true;
            }

        }

        private void SaveRecording(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == true)
            {
                recorder.SoundBuffer.SaveToFile(dialog.FileName); // zapis nagrania do pliku
            }
        }

        private void PickRecording(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if(dialog.ShowDialog() == true)
            {
                SoundBuffer buffer = new SoundBuffer(dialog.FileName); // wczytanie nagrania (pliku audio)
                sound.SoundBuffer = buffer;
                PlayBtn.IsEnabled = true;
            }
        }

        private void PlaySound(object sender, RoutedEventArgs e)
        {
            if(sound.Status == SoundStatus.Playing)
            {
                sound.Pause(); // pauza odtwarzania
                PlayBtn.Content = "Odtwórz";
            }
            else
            {
                sound.Play(); // odtwarzanie dźwięku
                PlayBtn.Content = "Pauza";
            }
        }
    }
}
