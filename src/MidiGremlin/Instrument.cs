using System;
using System.Collections.Generic;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    ///<summary>
    ///The Instrument class implements the method Play which plays the saved music from the class MusicObject.
    ///</summary>
    public class Instrument
    {
        public Scale Scale { get; set; }
        public int Octave { get; set; }

        public InstrumentType InstrumentType { get; }

        private IOrchestra _orchestra;


        internal Instrument (IOrchestra orchestra, InstrumentType instrumentType, Scale scale, int octave)
        {
            Scale = scale;
            Octave = octave;
            _orchestra = orchestra;
            InstrumentType = instrumentType;
        }

        
        
        public void Play(MusicObject music)
        {
            Play(_orchestra.CurrentTime(), music);
        }

        public void Play (int startTime, MusicObject music)
        {
            _orchestra.CopyToOutput(new List<SingleBeat>(music.GetChildren(this, startTime)));
        }

        public void Play(Tone tone, int duration, byte velocity = 64)
        {
            Play(_orchestra.CurrentTime(), tone, duration,  velocity);
        }

        public void Play(int startTime, Tone tone, int duration,  byte velocity = 64)
        {
            Note note = new Note(tone, duration, velocity);
            Play(startTime, note);
        }
    }
}