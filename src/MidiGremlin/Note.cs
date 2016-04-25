using System;
using System.Collections;
using System.Collections.Generic;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    ///<summary>
    ///The class Note constructs an individual and simpel sound.
    ///The class consists of duration, tone, velocity and an Octaveoffset which represents the octave which the note is placed at.
    ///</summary>
    public class Note : MusicObject
    {
        public Tone Tone { get; set; }
        public double Duration { get; set; }
        public byte Velocity { get; set; }
        public int OctaveOffset { get; set; }

        /// <summary>
        /// Represents a note. A note consist of tone,duration and velocity
        /// </summary>
        /// <param name="tone">An enum which represent a tone</param>
        /// <param name="duration">How long the note is played. In beats</param>
        /// <param name="velocity">How hard the note is played</param>
        public Note (Tone tone, double duration, byte velocity = 64)
        {
            OctaveOffset = (int)tone/12;
            Tone = tone;
            Duration = duration;
            Velocity = velocity;

        }
        /// <summary>
        /// Offsets the note.
        /// </summary>
        /// <param name="offset">Moves by intevals/tonesteps.</param>
        /// <param name="duration">How long the note is played. In beats</param>
        /// <param name="velocity">How hard the note is played.</param>
        /// <returns> Returns a new note where tone is added by offset</returns>
        public Note OffsetBy(int offset, double? duration = null, byte? velocity = null)
        {
            byte tempVelocity = (byte)velocity;
            double tempDuration = (double)duration;


            if (velocity == null)
            {
                tempVelocity = Velocity;
            }

            if (duration == null)
            {
                tempDuration = Duration;
            }

            return new Note(Tone + offset, tempDuration, tempVelocity);
        }
        /// <summary>
        /// Offsets the note. Overload with scale.
        /// </summary>
        /// <param name="scale">The scale you want to offset by</param>
        /// <param name="offset">Moves by intevals/tonesteps</param>
        /// <param name="duration">How long the note is played. In beats</param>
        /// <param name="velocity">How hard the note is played</param>
        /// <returns>Returns a note offset by a scale</returns>
        public Note OffsetBy(Scale scale, int offset, double? duration = null, byte? velocity = null)
        {

            byte tempVelocity = (byte)velocity;
            double tempDuration = (double)duration;


            if (velocity == null)
            {
                tempVelocity = Velocity;
            }

            if (duration == null)
            {
                tempDuration = Duration;
            }

            return new Note((Tone)(scale.Interval(Tone) + (int)scale[offset]), tempDuration, tempVelocity);
        }
        /// <summary>
        /// Offsets the note. Overload with octave
        /// </summary>
        /// <param name="scale">The scale you want to offset by</param>
        /// <param name="offset">Moves by intevals/tonesteps</param>
        /// <param name="OctaveOffset">the octave you want to offset by</param>
        /// <param name="duration">How long the note is played. In beats</param>
        /// <param name="velocity">How hard the note is played</param>
        /// <returns>Returns a note offset by an octave</returns>
        public Note OctaveOffsetBy(Scale scale, int offset, int OctaveOffset, double? duration = null, byte? velocity = null)
        {

            byte tempVelocity = (byte)velocity;
            double tempDuration = (double)duration;


            if (velocity == null)
            {
                tempVelocity = Velocity;
            }

            if (duration == null)
            {
                tempDuration = Duration;
            }
 
            return new Note((Tone)(scale.Interval(Tone) + ((int)scale[offset] * (int)OctaveOffset)), tempDuration, tempVelocity);
        }


        internal override IEnumerable<SingleBeat> GetChildren (Instrument playedBy, double startTime)
        {
            byte velocity = Math.Min((byte) 127, Velocity);

            int tone = MidiPithFromTone(Tone, playedBy.Octave);

            yield return new SingleBeat(playedBy.InstrumentType, (byte)tone, velocity, startTime, startTime + Duration);
        }


        internal int MidiPithFromTone(Tone tone, int octave)
        {
            int pitch = (int) Tone //Tone enum is a value between 1 and 12, where C is the first tone.
                        + (octave + 5 //Pitch 0 has octave -5, so octave 0 starts at 5*12=60. 
                           + OctaveOffset //Apply OctaveOffset of the note's tone.
                            )*12;   //1 octave is 12 tone steps.
            if (0 > pitch || pitch > 127)
                throw new ToneOutOfRangeExceptioin(pitch);

            return pitch;
        }
    }
}