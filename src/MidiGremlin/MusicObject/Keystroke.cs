using System;
using System.Collections.Generic;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    ///<summary>
    ///The Keystroke class consists of a tone and its duration. Note that two keystrokes are played with the same start-time unless a pause is added between them.
    ///</summary>
    public class Keystroke : MusicObject
    {
        ///<summary> The tone that the keystroke represents. </summary>
        public Tone Tone { get; set; }
       
        ///<summary> The duration of the keystroke. </summary>
        public double Duration { get; set; }
        
        ///<summary> The severety with which the keystroke is struck. </summary>
        public byte Velocity { get; set; }
        
        ///<summary> The keystroke's offset from the middle octave. </summary>
        public int OctaveOffset { get; set; }



        /// <summary>
        /// Creates a new instance of the Keystroke class.
        /// </summary>
        /// <param name="tone"> The tone that the keystroke represents. </param>
        /// <param name="duration">How long the keystroke is played in beats. </param>
        /// <param name="velocity"> The severety with which the keystroke is struck. </param>
        public Keystroke (Tone tone, double duration, byte velocity = 64)
        {
            OctaveOffset = (int)tone/12;
            Tone = (Tone)((int)tone%12);
            Duration = duration;
            Velocity = velocity;

        }



        /// <summary>
        /// Offsets the keystroke by an interval.
        /// </summary>
        /// <param name="offset"> Moves by intevals/tonesteps. </param>
        /// <returns> Returns a new keystroke that has been offset. </returns>
        public Keystroke OffsetBy(int offset)
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
        public Keystroke OffsetBy(Scale scale, int offset)
        {
            return OffsetBy(scale, offset, 0);
        }
        
        
        
        /// <summary>
        /// Offsets the keystroke by both an interval and an octave.
        /// </summary>
        /// <param name="offset"> Moves by intevals/tonesteps. </param>
        /// <param name="octaveOffset"> Moves by octave. </param>
        /// <returns> Returns a new keystroke that has been offset within the scale. </returns>
        public Keystroke OffsetBy(int offset, int octaveOffset)
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
        public Keystroke OffsetBy(Scale scale, int offset, int octaveOffset)
        {
            Tone newTone = scale[scale.Interval(Tone) + offset + scale.Count*octaveOffset];

            return new Keystroke(newTone, Duration, Velocity);
        }



        /// <summary>
        /// Returns a singleBeat corresponding to the keystroke.
        /// This is modified by the octave of the instrument that plays it.
        /// </summary>
        /// <param name="playedBy">The instrument that requests the children.</param>
        /// <param name="startTime">The time at which the SingleBeats should start playing.</param>
        /// <returns>The full contents of this MusicObject as SingleBeats.</returns>
        internal override IEnumerable<SingleBeat> GetChildren (Instrument playedBy, double startTime)
        {
            byte velocity = Math.Min((byte) 127, Velocity);

            int tone = MidiPitchFromTone(Tone, playedBy.Octave);

            yield return new SingleBeat(playedBy.InstrumentType, (byte)tone, velocity, startTime, startTime + Duration);
        }



        /// <summary>
        /// Projects all music objects of specified type into a <see cref="MusicObject"/> of the same structure.
        /// </summary>
        /// <typeparam name="T">The MusicObject subtype to modify.</typeparam>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>A <see cref="MusicObject"/> of identical structure that is the result of invoking the transform function of all elements of type T.</returns>
        public override MusicObject Select<T>(Func<T, T> selector)
        {
            Keystroke result = new Keystroke(Tone, Duration, Velocity)
            {
                Tone = Tone,
                OctaveOffset = OctaveOffset
            };  //Tone and OctaveOffset must be copied directly because of the weird conversion in the constructor.

            if (this is T)
                return selector(result as T);
            else
                return result;
        }



        /// <summary>
        /// Converts the MIDI Gremlin tone to the pitch that the MIDI standard specifies.
        /// </summary>
        internal int MidiPitchFromTone(Tone tone, int octave)
        {
            int pitch = (int) tone + //Tone enum is a value between 1 and 12, where C is the first tone.
                (
                    octave + 5 + //Pitch 0 has octave -5, so octave 0 starts at 5*12=60. 
                    OctaveOffset //Apply OctaveOffset of the keystroke's tone.
                )*12;   //1 octave is 12 tone steps.
            if (0 > pitch || pitch > 127)
                throw new ToneOutOfRangeException(pitch);

            return pitch;
        }
    }
}
