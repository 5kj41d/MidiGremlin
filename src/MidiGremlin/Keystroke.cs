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
            Tone = tone;
            Duration = duration;
            Velocity = velocity;

        }


        /// <summary>
        /// Offsets the keystroke by an interval.
        /// </summary>
        /// <param name="offset"> Moves by intevals/tonesteps. </param>
        /// <param name="duration"> Changes how long the keystroke is played. In beats</param>
        /// <param name="velocity"> Changes how hard the keystroke is played. </param>
        /// <returns> Returns a new keystroke that has been offset. </returns>
        public Keystroke OffsetBy(int offset, double? duration = null, byte? velocity = null)
        {
            byte tempVelocity = Velocity;
            double tempDuration = Duration;


            if (velocity != null)
            {
                tempVelocity = velocity.Value;
            }

            if (duration != null)
            {
                tempDuration = duration.Value;
            }

            return new Keystroke(Tone + offset, tempDuration, tempVelocity);
        }
        /// <summary>
        /// Offsets the keystroke in a specified scale.
        /// </summary>
        /// <param name="scale">The scale you want to offset by</param>
        /// <param name="offset"> Moves by intevals/tonesteps. </param>
        /// <param name="duration"> Changes how long the keystroke is played. In beats</param>
        /// <param name="velocity"> Changes how hard the keystroke is played. </param>
        /// <returns> Returns a new keystroke that has been offset within the scale. </returns>
        public Keystroke OffsetBy(Scale scale, int offset, double? duration = null, byte? velocity = null)
        {
            byte tempVelocity = Velocity;
            double tempDuration = Duration;


            if (velocity != null)
            {
                tempVelocity = velocity.Value;
            }

            if (duration != null)
            {
                tempDuration = duration.Value;
            }

            return new Keystroke((Tone)(scale.Interval(Tone) + (int)scale[offset]), tempDuration, tempVelocity);
        }

        /// <summary>
        /// Offsets the keystroke by both an interval and an octave.
        /// </summary>
        /// <param name="scale">The scale you want to offset by</param>
        /// <param name="offset"> Moves by intevals/tonesteps. </param>
        /// <param name="octaveOffset"> Moves by octave. </param>
        /// <param name="duration"> Changes how long the keystroke is played. In beats</param>
        /// <param name="velocity"> Changes how hard the keystroke is played. </param>
        /// <returns> Returns a new keystroke that has been offset within the scale. </returns>
        public Keystroke OctaveOffsetBy(Scale scale, int offset, int octaveOffset, double? duration = null, byte? velocity = null)
        {
            byte tempVelocity = Velocity;
            double tempDuration = Duration;

            if (velocity != null)
            {
                tempVelocity = velocity.Value;
            }

            if (duration != null)
            {
                tempDuration = duration.Value;
            }

            return new Keystroke((Tone)(scale.Interval(Tone) + ((int)scale[offset] * octaveOffset)), tempDuration, tempVelocity);
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
                throw new ToneOutOfRangeExceptioin(pitch);

            return pitch;
        }
    }
}