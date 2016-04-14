using System;

namespace MidiGremlin
{
    ///<summary>
    ///The Instrument class implements the method Play which plays the saved music from the class MusicObject.
    ///</summary>
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