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

        public SingleBeat(InstrumentType instrumentType, byte toneOffset, byte toneVelocity, int toneStartTime, int toneEndTime)
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
        public int ToneStartTime{ get; }
        public int ToneEndTime{ get; }
    }

    public struct SingleBeatWithChannel
    {
        public SingleBeatWithChannel(InstrumentType instrumentType, byte tone, byte toneVelocity, int toneStartTime, int toneEndTime, byte channel)
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
        public int ToneStartTime { get; }
        public int ToneEndTime { get; }

        public  byte Channel { get; }
    }
}