using System;

namespace MidiGremlin
{
    public class Instrument
    {
        public Scale Scale { get; set; }
        public int Octave { get; set; }

        internal Instrument ()
        {
            throw new NotImplementedException();
        }

        public void Play (MusicObject music)
        {
            throw new NotImplementedException();
        }

        public void Play(Tone tone, int duration, int velocity = 64)
        {
            throw new NotImplementedException();
        }
    }
}