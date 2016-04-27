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
            _workThread.Start();
        }

        /// <summary> How many beats corresponds to 60 seconds. If the value is set to 60, 1 beat will be the same as 1 second. </summary>
        public int BeatsPerMinute { get; set; }

        /// <summary> Conversion constant between minutes and milliseconds. </summary>
        private static double _minutesToMilliseconds = (5 / 3) * Math.Pow(10, -5);
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
        public int CurrentTime()
        {
            return  (int) (_time.Elapsed.TotalMilliseconds / BeatDuratinInMilliseconds);
        }

       /// <summary>
       /// plays the music in order.
       /// </summary>
       /// <param name="music">the actual music that should be played</param>
        public void QueueMusic(IEnumerable<SingleBeatWithChannel> music)
        {
            lock (_sync)
            {
                toPlay.AddRange(music.SelectMany(TransformFunction));
                toPlay.Sort((lhs, rhs) => lhs.Timestamp.CompareTo(rhs.Timestamp));
                toPlay.Reverse();   //Beat to play next is the last in the list and so on.
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
                    ( (int)Math.Floor((next.Timestamp - CurrentTime()) * BeatDuratinInMilliseconds)
                    , UpdateFrequency);

                if(sleeptime > 0)
                    Thread.Sleep(sleeptime);

                if (next.Timestamp <= CurrentTime())
                {
                    Winmm.midiOutShortMsg(_handle, (uint) next.Data);
                    played = true;
                }
            }
        }
    }
}
