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
    /// <summary>
    /// The class WinmmOut stands for, Windows Multi Media Output, and does exactly that. 
    /// It communicates with Windows Multi Media player and creates a file in which you can listen to the programmed music. 
    /// </summary>
    public class WinmmOut : IMidiOut
    {
        public uint DeviceID { get; }
        
        private const int UpdateFrequency = 20;
        private IntPtr _handle;
        private bool _disposed = false;
        private Stopwatch _time;
        private Thread _workThread;
        private readonly object _sync = new object();
        private List<SimpleMidiMessage> toPlay = new List<SimpleMidiMessage>();
        private bool _running = true;
        /// <summary>
        /// Plays the music using the windows music player
        /// </summary>
        /// <param name="deviceID">The underlaying hardware port used to play music</param>
        /// <param name="beatsPerMinutes">Represents the pace of the music</param>
        public WinmmOut (uint deviceID, int beatsPerMinutes=60)
        {
            BeatsPerMinute = beatsPerMinutes;

            uint numberOfDevices =  Winmm.midiOutGetNumDevs();
            DeviceID = numberOfDevices < deviceID ? 0 : deviceID;
            if(0!= Winmm.midiOutOpen(out _handle, DeviceID, IntPtr.Zero, IntPtr.Zero, 0))
                throw new Exception("Opening MIDI device unsuccessful.");

            _time = Stopwatch.StartNew();

            _workThread = new Thread(ThreadEntryPrt);
        }

        /// <summary> How many beats corresponds to 60 seconds. If the value is set to 60, 1 beat will be the same as 1 second. </summary>
        public int BeatsPerMinute { get; set; }

        /// <summary> Conversion constant between minutes and milliseconds. </summary>
        private static double _minutesToMilliseconds = (5 / 3) * Math.Pow(10, -5);

	    private BeatScheduler _source;

	    /// <summary>
        /// The duration of 1 beat in milliseconds.
        /// </summary>
        /// <returns>The duration of 1 beat in milliseconds.</returns>
        public double BeatDuratinInMilliseconds
        {
            get
            {
                double durationInMinutes = 1.0/BeatsPerMinute;
                double durationInMilliseconds = durationInMinutes * _minutesToMilliseconds;
                return durationInMilliseconds;
            }
        }
        /// <summary>
        /// closes safely the winmmout
        /// </summary>
        public void Dispose()
        {
            Winmm.midiOutClose(_handle);
            _disposed = true;
        }

	    /// <summary>
	    /// The amount of beats that have passed since this class was instantiated.
	    /// </summary>
	    /// <returns>The amount of beats that have passed since this class was instantiated.</returns>
	    public double CurrentTime()
        {
            return  _time.Elapsed.TotalMilliseconds / BeatDuratinInMilliseconds;
        }

	    void IMidiOut.SetSource(BeatScheduler source)
	    {
		    lock (_sync)
		    {
			    if (_source == null)
			    {
					_source = source;
					_workThread.Start();
				}
			    else
			    {
				    throw new InvalidOperationException("This IMidiOut is already in use");
			    }
		    }
	    }

	    private void ThreadEntryPrt()
        {
            SimpleMidiMessage next;

            while (_running)
            {
	            next = _source.GetNextMidiCommand(block: true);

	            Winmm.midiOutShortMsg(_handle, next.Data);
            }
        }
    }
}
