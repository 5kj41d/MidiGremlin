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
        /// <summary> The scale that the instrument plays in. This is only used by MIDI Gremlin when playing chords. </summary>
        public Scale Scale { get; set; }

       
        
        /// <summary> The octave(from the middle octave) that the instrument plays in. </summary>
        public int Octave { get; set; }

        
        
        /// <summary> The instrument sound that will be used when playing MusicObjects from this instance. </summary>
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
        /// Plays the MusicObject using this instrument.
        /// </summary>
        /// <param name="music"> The music that the user wants played</param>
        public void Play(MusicObject music)
        {
            Play(_orchestra.CurrentTime(), music);
        }
        
        
        
        /// <summary>
        /// Plays the MusicObject at the given start time, using this instrument.
        /// Start time is measured in beats from the start of the music. Get the curent time by calling CurrentTime from the orchestra.
        /// </summary>
        /// <param name="startTime">The moment when the music should play. </param>
        /// <param name="music">The music to play. </param>
        public void Play (double startTime, MusicObject music)
        {
	        List<SingleBeat> singleBeats = new List<SingleBeat>(
				music
				.GetChildren(this, startTime)
				.Where(x => !(x.ToneVelocity == 0xff && x.Tone == 0xff))
		        //.Select(offsetByOctave)
			);
            _orchestra.CopyToOutput(singleBeats);
        }



        /// <summary>
        /// Plays a tone with duration and velocity specified.
        /// </summary>
        /// <param name="tone">The tone to play. </param>
        /// <param name="duration">How long the tone should last in beats. </param>
        /// <param name="velocity">From 0 to 127. Default is 64. </param>
        public void Play(Tone tone, double duration, byte velocity = 64)
        {
            Play(_orchestra.CurrentTime(), tone, duration,  velocity);
        }
        
        
        
        /// <summary>
        /// Plays a tone with duration, velocity and start-time specified.
        /// Time is measured in beats from the start of the music. Get the curent time by calling CurrentTime from the orchestra.
        /// </summary>
        /// <param name="startTime">When the music should play.</param>
        /// <param name="tone">The tone to play. </param>
        /// <param name="duration">How long the tone should las in beats</param>
        /// <param name="velocity">Goes from 0 to 127. Default is 64.</param>
        public void Play(double startTime, Tone tone, double duration,  byte velocity = 64)
        {
            Keystroke keystroke = new Keystroke(tone, duration, velocity);
            Play(startTime, keystroke);
        }
    }
}