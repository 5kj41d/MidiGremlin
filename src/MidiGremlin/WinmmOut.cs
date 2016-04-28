using System;
using System.Collections.Generic;
using System.Linq;
using MidiGremlin.Internal;
using MidiGremlin.Internal.Windows_Multi_Media;
using System.Diagnostics;
using System.Threading;

namespace MidiGremlin
{
    struct SimpleMidiMessage
    {
        /// <summary> The time in beats to play this MIDI message. </summary>
        public readonly double Timestamp;
		[DebuggerDisplay("{Data,h}")]
        public readonly int Data;

        public SimpleMidiMessage(int data, double timestamp)
        {
            Data = data;
            Timestamp = timestamp;
        }
    }



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
            BeatsPerMinute = beatsPerMinutes;

            uint numberOfDevices =  Winmm.midiOutGetNumDevs();
            DeviceID = numberOfDevices < deviceID ? 0 : deviceID;
            if(0!= Winmm.midiOutOpen(out _handle, DeviceID, IntPtr.Zero, IntPtr.Zero, 0))
                throw new Exception("Opening MIDI device unsuccessful.");

            _time = Stopwatch.StartNew();

            _workThread = new Thread(ThreadEntryPrt);
            _workThread.Start();
        }

        /// <summary> How many beats corresponds to 60 seconds. If the value is set to 60, 1 beat will be the same as 1 second. </summary>
        public int BeatsPerMinute { get; set; }

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
        public int CurrentTime => (int) (_time.Elapsed.TotalMilliseconds / BeatDuratinInMilliseconds);

        /// <summary>
        /// Plays the MusicObject at its specified start-time.
        /// In normal cases, please use the Play method of an <see cref="T:MidiGremlin.Instrument"/> instead.
        /// </summary>
        /// <param name="music">The music that should be played.</param>
        public void QueueMusic(IEnumerable<SingleBeatWithChannel> music)
        {
            lock (_sync)
            {
                toPlay.AddRange(music.SelectMany(TransformFunction));
                toPlay.Sort((lhs, rhs) => lhs.Timestamp.CompareTo(rhs.Timestamp));
                toPlay.Reverse();   //Beat to play next is the last in the list and so on.
            }
        }

        /// <summary>
        /// Transforms the SimpleMidiMessages to two seperate ones each, one for key down and one for key up.
        /// </summary>
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
            bool played= false;
            SimpleMidiMessage next = new SimpleMidiMessage(0, double.MaxValue);

            while (_running)
            {
                
                
                lock (_sync)
                {
                   
	                if (toPlay.Count > 0)
                    {
                        if (played)
                        {
                            next = toPlay[toPlay.Count - 1];
                            toPlay.RemoveAt(toPlay.Count - 1);

                            played = false;
                        }
                        else
                        {
                            if(next.Timestamp > toPlay[toPlay.Count - 1].Timestamp)
                            {
                                SimpleMidiMessage actualNext = toPlay[toPlay.Count - 1];
                                toPlay.RemoveAt(toPlay.Count - 1);

                                //Put "next" back in list so we can play actualNext inistead.
                                for (int i = toPlay.Count - 1; i >= 0; i--)
                                {
                                    if(next.Timestamp <= toPlay[i].Timestamp)
                                    {
                                        toPlay.Insert(i, next);
                                        break;
                                    }
                                }
                                next = actualNext;

                                played = false;
                            }
                            //Else the current "next" is correct so keep it.
                        }
	                }
                    else
                        next = new SimpleMidiMessage(0, double.MaxValue);
                }

                int sleeptime = Math.Min
                    ( (int)Math.Floor((next.Timestamp - CurrentTime) * BeatDuratinInMilliseconds)
                    , UpdateFrequency);

                if(sleeptime > 0)
                    Thread.Sleep(sleeptime);

                if (next.Timestamp <= CurrentTime)
                {
                    Winmm.midiOutShortMsg(_handle, (uint) next.Data);
                    played = true;
                }
            }
        }
    }
}
