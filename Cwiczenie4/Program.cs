using SFML.Audio;
using System;
using System.Threading;

namespace Cwiczenie4
{
    class Program
    {
        static void Main(string[] args)
        {
            var devices = SoundRecorder.AvailableDevices;
            foreach (var item in devices)
            {
                Console.WriteLine(item);
            }

            if (SoundBufferRecorder.IsAvailable)
            {
                // error
            }

            SoundBufferRecorder recorder = new SoundBufferRecorder();


            recorder.Start();
            Console.WriteLine("Recording...");

            Thread.Sleep(5000);

            recorder.Stop();
            Console.WriteLine("Finished.");

            SoundBuffer buffer = recorder.SoundBuffer;

            buffer.SaveToFile(@"E:\sound.wav");

            Sound sound = new Sound(buffer);
            sound.Play();
            Console.ReadLine();
        }
    }
}
