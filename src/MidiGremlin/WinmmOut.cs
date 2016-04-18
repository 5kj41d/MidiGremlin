using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MidiGremlin.Internal;
using MidiGremlin.Internal.Windows_Multi_Media;
using System.Diagnostics;
using System.Threading;

namespace MidiGremlin
{
    struct SimpleMidiMessage
    {
        public readonly int Timestamp;
		[DebuggerDisplay("{Data,h}")]
        public readonly int Data;

        public SimpleMidiMessage(int data, int timestamp)
        {
            this.Data = data;
            this.Timestamp = timestamp;
        }
    }



    /// <summary>
    /// The class WinmmOut stands for, Windows Multi Media Output, and does exactly that. 
    /// It communicates with Windows Multi Media player and creates a file in which you can listen to the programmed music. 
    /// </summary>
    public class WinmmOut : IMidiOut
    {
        public uint DeviceID { get; }

        private const int UpdateFrequency = 100;
        private IntPtr _handle;
        private bool _disposed = false;
        private Stopwatch _time;
        private Thread _workThread;
        private readonly object _sync = new object();
        private List<SimpleMidiMessage> toPlay = new List<SimpleMidiMessage>();
        private bool _running = true;

        public WinmmOut(uint deviceID)
        {
            uint numberOfDevices =  Winmm.midiOutGetNumDevs();
            DeviceID = numberOfDevices < deviceID ? 0 : deviceID;

            if(0!= Winmm.midiOutOpen(out _handle, DeviceID, IntPtr.Zero, IntPtr.Zero, 0))
                throw new Exception("Opening MIDI device unsuccessful.");

            _time = Stopwatch.StartNew();

            _workThread = new Thread(ThreadEntryPrt);
            _workThread.Start();
        }


        public void Dispose()
        {
            Winmm.midiOutClose(_handle);
            _disposed = true;
        }

        public int CurrentTime()
        {
            double bogus_value = 16;
            return  (int) (_time.Elapsed.TotalSeconds*bogus_value);
        }

        
        public void QueueMusic(IEnumerable<SingleBeatWithChannel> music)
        {
            lock (_sync)
            {
                toPlay.AddRange(music.SelectMany(TransformFunction));
                toPlay.Sort((lhs, rhs) => rhs.Timestamp - lhs.Timestamp);   //Beat to play next is the last in the list and so on.
            }
        }

        private IEnumerable<SimpleMidiMessage> TransformFunction(SingleBeatWithChannel arg)
        {
            yield return new SimpleMidiMessage(
                MakeMidiEvent(0x9, arg.Channel, arg.Tone, arg.ToneVelocity)   //Key down.
                , arg.ToneStartTime);

            yield return new SimpleMidiMessage(
                MakeMidiEvent(0x8, arg.Channel, arg.Tone, arg.ToneVelocity)   //Key up.
                , arg.ToneEndTime);
        }

        private int MakeMidiEvent(byte midiEventType, byte channel, byte tone, byte toneVelocity)
        {
            int data = 0;

            data |= channel << 0;
            data |= midiEventType << 4;
            data |= tone << 8;  //TODO: Tone needs to be translated this does not work.
            data |= toneVelocity << 16;

            return data;
        }


        private void ThreadEntryPrt()
        {
            while (_running)
            {
                SimpleMidiMessage next;
                lock (_sync)
                {
	                if (toPlay.Count > 0)
	                {
		                next = toPlay[toPlay.Count - 1];
						toPlay.RemoveAt(toPlay.Count - 1);
	                }
                    else
                        next = new SimpleMidiMessage(0, int.MaxValue);
                }

                int sleeptime = Math.Max(Math.Min(next.Timestamp - CurrentTime(), UpdateFrequency), 0);

                if(sleeptime > 0)
                    Thread.Sleep(sleeptime);

                if (next.Timestamp < CurrentTime())
                {
                    Winmm.midiOutShortMsg(_handle, (uint) next.Data);
                }
            }
        }
    }
}
