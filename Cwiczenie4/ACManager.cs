using System;
using System.Collections.Generic;
using System.Text;
using SFML.Audio;

namespace Cwiczenie4
{
    public class ACManager
    {
        public static string[] Devices => SoundRecorder.AvailableDevices;
    }
}
