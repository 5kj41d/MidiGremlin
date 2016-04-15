using System;

namespace MidiGremlin
{
    internal class ToneOutOfRangeExceptioin : Exception
    {
        public int Tone { get; }

        public ToneOutOfRangeExceptioin(int tone) : base("Should be between 0 and 127.")
        {
            Tone = tone;
        }
    }
}