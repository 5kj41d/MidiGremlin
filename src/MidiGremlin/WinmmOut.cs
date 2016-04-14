using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MidiGremlin.Internal;
using MidiGremlin.Internal.Windows_Multi_Media;

namespace MidiGremlin
{
    public class WinmmOut : IMidiOut
    {
        public uint DevicID { get; }

        private IntPtr _handle;
        private bool disposed = false;

        public WinmmOut(uint deviceID)
        {
            uint numberOfDevices =  Winmm.midiOutGetNumDevs();
            DevicID = numberOfDevices < deviceID ? 0 : deviceID;

            if(0!= Winmm.midiOutOpen(out _handle, DevicID, IntPtr.Zero, IntPtr.Zero, 0))
                throw new Exception("Opening MIDI device unsuccessful.");

            Winmm.midiOutShortMsg(_handle, 0x404000);
        }


        public void Dispose()
        {
            Winmm.midiOutClose(_handle);
            disposed = true;
        }

        public int CurrentTime()
        {
            throw new NotImplementedException();
        }

        public void QueueMusic(IEnumerable<SingleBeatWithChannel> music)
        {
            throw new NotImplementedException();
        }
    }
}
