using System;
using System.Collections.Generic;
using System.Linq;
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


        internal Instrument (IOrchestra orchestra, InstrumentType instrumentType, Scale scale, int octave = 0)
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

        public void Play (double startTime, MusicObject music)
        {
            List<SingleBeat> singleBeats = new List<SingleBeat>(music.GetChildren(this, startTime))
                .Select(offsetByOctave)
                .ToList();
            _orchestra.CopyToOutput(singleBeats);
        }

        public void Play(Tone tone, double duration, byte velocity = 64)
        {
            Play(_orchestra.CurrentTime(), tone, duration,  velocity);
        }

        public void Play(double startTime, Tone tone, double duration,  byte velocity = 64)
        {
            Note note = new Note(tone, duration, velocity);
            Play(startTime, note);
        }

        
        private SingleBeat offsetByOctave (SingleBeat arg)
        {
            return arg;
            return new SingleBeat
                (arg.instrumentType
                , (byte) (arg.ToneOffset + Octave*12)   //The important part.
                , arg.ToneVelocity
                , arg.ToneStartTime
                , arg.ToneEndTime);
        }
    }
}