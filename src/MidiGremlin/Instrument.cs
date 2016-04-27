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

        
        /// <summary>
        /// Plays the MusicObject.
        /// </summary>
        /// <param name="music"> The music that the user wants played</param>
        public void Play(MusicObject music)
        {
            Play(_orchestra.CurrentTime(), music);
        }
        /// <summary>
        /// Plays the MusicObject.With given start time.
        /// </summary>
        /// <param name="startTime">When the music should play. In beats</param>
        /// <param name="music">The actual music</param>
        public void Play (double startTime, MusicObject music)
        {
            List<SingleBeat> singleBeats = new List<SingleBeat>(music.GetChildren(this, startTime))
                .Select(offsetByOctave)
                .ToList();
            _orchestra.CopyToOutput(singleBeats);
        }
        /// <summary>
        /// Plays the tone specified in the duration and velocity specified.
        /// </summary>
        /// <param name="tone">An enum which represents a tone</param>
        /// <param name="duration">How long the tone should last. In beats</param>
        /// <param name="velocity">Goes from 0 to 127. Default is 64. </param>
        public void Play(Tone tone, double duration, byte velocity = 64)
        {
            Play(_orchestra.CurrentTime(), tone, duration,  velocity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime">When the music should play. In beats</param>
        /// <param name="tone">An enum which represents a tone</param>
        /// <param name="duration">How long the tone should last. In beats</param>
        /// <param name="velocity">Goes from 0 to 127. Default is 64.</param>
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