using System.Runtime.InteropServices;

namespace MidiGremlin.Internal
{
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

        public SingleBeat(InstrumentType instrumentType, byte toneOffset, byte toneVelocity, double toneStartTime, double toneEndTime)
        {
            this.instrumentType = instrumentType;
            ToneOffset = toneOffset;
            ToneVelocity = toneVelocity;
            ToneStartTime = toneStartTime;
            ToneEndTime = toneEndTime;
        }

        public InstrumentType instrumentType{ get; }
        public byte ToneOffset{ get; }
        public byte ToneVelocity{ get; }
        public double ToneStartTime{ get; }
        public double ToneEndTime{ get; }
    }

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
        public byte Tone { get; }
        public byte ToneVelocity { get; }
        public double ToneStartTime { get; }
        public double ToneEndTime { get; }

        public  byte Channel { get; }
    }
}