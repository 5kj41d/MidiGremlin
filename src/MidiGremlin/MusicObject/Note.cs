using System;
using System.Collections.Generic;
using System.Diagnostics;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    /// <summary>
    /// Contains a <see cref="T:MidiGremlin.Keystroke"/> and a <see cref="T:MidiGremlin.Pause"/>.
    /// </summary>
    [DebuggerDisplay("{Keystroke.Tone} {Pause.Duration}")]
    public class Note : MusicObject
    {
        /// <summary> The actual sound in the note. </summary>
        public Keystroke Keystroke;
        /// <summary> Ensures that there is a wait time before the next note is played. </summary>
        public Pause Pause;



        /// <summary>
        /// Creates a new instance of the Note class by initializing a <see cref="T:MidiGremlin.Keystroke"/> and a <see cref="T:MidiGremlin.Pause"/> with the same duration.
        /// </summary>
        /// <param name="tone"> The tone that the keystroke represents. </param>
        /// <param name="duration">How long the tone is played in beats, and how long to wait before playing the next MusicObject. </param>
        /// <param name="velocity"> The severety with which the keystroke is struck. </param>
        public Note (Tone tone, double duration, byte velocity = 64)
        {
            Keystroke = new Keystroke(tone, duration, velocity);
            Pause = new Pause(duration);
        }



        /// <summary>
        /// Returns the full contents of this MusicObject as SingleBeats.
        /// These are modified by the octave of the instrument that played them.
        /// </summary>
        /// <param name="playedBy">The instrument that requests the children.</param>
        /// <param name="startTime">The time at which the SingleBeats should start playing.</param>
        /// <returns>The full contents of this MusicObject as SingleBeats.</returns>
        internal override IEnumerable<SingleBeat> GetChildren(Instrument playedBy, double startTime)
        {
	        foreach (SingleBeat beat  in Keystroke.GetChildren(playedBy, startTime))
		        yield return beat;

			foreach (SingleBeat beat in Pause.GetChildren(playedBy, startTime))
				yield return beat;
        }



        /// <summary>
        /// Offsets the keystroke by an interval.
        /// </summary>
        /// <param name="offset"> Moves by intevals/tonesteps. </param>
        /// <returns> Returns a new keystroke that has been offset. </returns>
        public Note OffsetBy (int offset)
        {
            return OffsetBy(Scale.ChromaticScale, offset);
        }
        
        
        
        /// <summary>
        /// Offsets the keystroke in a specified scale.
        /// </summary>
        /// <param name="scale">The scale you want to offset by</param>
        /// <param name="offset"> Moves by intevals/tonesteps. </param>
        /// <returns> Returns a new keystroke that has been offset within the scale. </returns>
        /// <exception cref="ToneNotFoundException">If the Keystroke's Tone is not part of the scale.</exception>
        public Note OffsetBy (Scale scale, int offset)
        {
            return OffsetBy(scale, offset, 0);
        }



        /// <summary>
        /// Offsets the keystroke by both an interval and an octave.
        /// </summary>
        /// <param name="offset"> Moves by intevals/tonesteps. </param>
        /// <param name="octaveOffset"> Moves by octave. </param>
        /// <returns> Returns a new keystroke that has been offset within the scale. </returns>
        public Note OffsetBy (int offset, int octaveOffset)
        {
            return OffsetBy(Scale.ChromaticScale, offset, octaveOffset);
        }
        
        
        
        /// <summary>
        /// Offsets the keystroke by both an interval and an octave.
        /// </summary>
        /// <param name="scale">The scale you want to offset by</param>
        /// <param name="offset"> Moves by intevals/tonesteps. </param>
        /// <param name="octaveOffset"> Moves by octave. </param>
        /// <returns> Returns a new keystroke that has been offset within the scale. </returns>
        public Note OffsetBy (Scale scale, int offset, int octaveOffset)
        {
            
            Keystroke k = Keystroke.OffsetBy(scale, offset, octaveOffset);
			Note result = new Note(k.Tone, Pause.Duration, k.Velocity);
			return result;
        }


        /// <summary>
        /// Projects all music objects of specified type into a <see cref="MusicObject"/> of the same structure.
        /// </summary>
        /// <typeparam name="T">The MusicObject subtype to modify.</typeparam>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>A <see cref="MusicObject"/> of identical structure that is the result of invoking the transform function of all elements of type T.</returns>
        public override MusicObject Select<T>(Func<T, MusicObject> selector)
        {
            Note result = new Note(Keystroke.Tone, Pause.Duration, Keystroke.Velocity);
            result.Keystroke.Select(selector);
            result.Pause.Select(selector);

            if (this is T)
                return selector(result as T);
            else
                return result;
        }
    }
}
