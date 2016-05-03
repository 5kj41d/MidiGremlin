using System;
using System.Collections.Generic;
using System.Linq;
using MidiGremlin.Internal;
using MidiGremlin.Internal.Windows_Multi_Media;
using System.Diagnostics;
using System.Threading;

namespace MidiGremlin
{
    /// <summary>
    /// The class Winmm Windows Multi Media Output.
    /// This class communicates with winmm which outputs to the speaker using the specified device.
    /// </summary>
    public class WinmmOut : IMidiOut
    {
        /// <summary> The ID of the device to use. 
        /// If the device is not found, the default(ID 0) Windows virtual synthesizer will be used.</summary>
        public uint DeviceID { get; }

	    private const int UpdateFrequency = 20;
		private double _oldTime = 0;
        private IntPtr _handle;
        private bool _disposed = false;
        private Stopwatch _time;
        private Thread _workThread;
        private readonly object _sync = new object();
        private List<SimpleMidiMessage> toPlay = new List<SimpleMidiMessage>();
        private bool _running = true;


        /// <summary>
        /// Creates a new instance of the WinmmOut class which opens a MIDI port at the specified device ID.
        /// </summary>
        /// <param name="deviceID">The underlaying hardware port used to play music. 
        /// Windows should have a built-in virtual synthesizer as device 0.
        /// The next port(if available) will be port 1 and so on.</param>
        /// <param name="beatsPerMinutes">Specifies the length of a beat, which is the unit of time used throughout MIDI Gremlin.
        /// If left at 60, a beat will be the same as a second.</param>
        public WinmmOut (uint deviceID, int beatsPerMinutes=60)
        {
			_time = new Stopwatch();
			BeatsPerMinute = beatsPerMinutes;

            uint numberOfDevices =  Winmm.midiOutGetNumDevs();
            DeviceID = numberOfDevices < deviceID ? 0 : deviceID;
            if(0!= Winmm.midiOutOpen(out _handle, DeviceID, IntPtr.Zero, IntPtr.Zero, 0))
                throw new Exception("Opening MIDI device unsuccessful.");

            
	        _workThread = new Thread(ThreadEntryPrt)
	        {
		        IsBackground = true
	        };
			
        }

	    private int _beatsPerMinute;

	    /// <summary> How many beats corresponds to 60 seconds. If the value is set to 60, 1 beat will be the same as 1 second. </summary>
	    public int BeatsPerMinute
	    {
		    get { return _beatsPerMinute; }
		    set
		    {
			    if (_beatsPerMinute != value)
			    {
				    _oldTime += CurrentScaleTime();
					_time.Restart();
				    _beatsPerMinute = value;
			    }
		    }
	    }

	    private BeatScheduler _source;

        /// <summary> Conversion constant between minutes and milliseconds. </summary>
        private static double _minutesToMilliseconds = 60000;

        
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
        /// Closes the WinmmOut instance safely.
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
            return  _oldTime + CurrentScaleTime();
        }

	    private double CurrentScaleTime()
	    {
		    return _time.Elapsed.TotalMilliseconds / BeatDuratinInMilliseconds;
	    }

	    void IMidiOut.SetSource(BeatScheduler source)
	    {
		    lock (_sync)
		    {
			    if (_source == null)
			    {
					_source = source;
					_workThread.Start();
					_time.Start();
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
