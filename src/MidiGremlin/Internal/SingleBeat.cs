using System.Runtime.InteropServices;

namespace MidiGremlin.Internal
{
    /// <summary>
    /// Represents a single musical tune with a beginning and an end.
    /// </summary>
    public struct SingleBeat
    {
        public override bool Equals(object obj)
        {
            if (obj is SingleBeat)
            {
                SingleBeat other = (SingleBeat)obj;

                return
                    other.instrumentType == this.instrumentType &&
                    other.ToneOffset == this.ToneOffset &&
                    other.ToneVelocity == this.ToneVelocity &&
                    other.ToneStartTime == this.ToneStartTime &&
                    other.ToneEndTime == this.ToneEndTime;
            }

            return false;
        }
        /// <summary>
        /// the actual tone that gets played
        /// </summary>
        /// <param name="instrumentType">the type of instrument that should be playing the music</param>
        /// <param name="toneOffset"> MIDI note number on this chart:http://www.tonalsoft.com/pub/news/pitch-bend.aspx </param>
        /// <param name="toneVelocity">how hard the key is hit on the instrument </param>
        /// <param name="toneStartTime">start time of the note</param>
        /// <param name="toneEndTime">end of the note</param>
        public SingleBeat(InstrumentType instrumentType, byte toneOffset, byte toneVelocity, double toneStartTime, double toneEndTime)
        {
            this.instrumentType = instrumentType;
            ToneOffset = toneOffset;
            ToneVelocity = toneVelocity;
            ToneStartTime = toneStartTime;
            ToneEndTime = toneEndTime;
        }

        public InstrumentType instrumentType{ get; }
        /// <summary>
        /// Represents the tone as it is saved in the MIDI standard.
        /// http://www.tonalsoft.com/pub/news/pitch-bend.aspx
        /// </summary>
        public byte ToneOffset{ get; }
        public byte ToneVelocity{ get; }
        public double ToneStartTime{ get; }
        public double ToneEndTime{ get; }
    }
    /// <summary>
    /// a singlebeat with a channel
    /// </summary>
    public struct SingleBeatWithChannel
    {
        public SingleBeatWithChannel(InstrumentType instrumentType, byte tone, byte toneVelocity, double toneStartTime, double toneEndTime, byte channel)
        {
            this.instrumentType = instrumentType;
            Tone = tone;
            ToneVelocity = toneVelocity;
            ToneStartTime = toneStartTime;
            ToneEndTime = toneEndTime;
            Channel = channel;
        }

        public InstrumentType instrumentType { get; }
        /// <summary>
        /// Represents the tone as it is saved in the MIDI standard.
        /// http://www.tonalsoft.com/pub/news/pitch-bend.aspx
        /// </summary>
        public byte Tone { get; }
        public byte ToneVelocity { get; }
        public double ToneStartTime { get; }
        public double ToneEndTime { get; }

        public  byte Channel { get; }
    }
}