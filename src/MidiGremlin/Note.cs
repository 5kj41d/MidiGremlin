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
        

        public Note (Tone tone, double duration, byte velocity = 64)
        {
            OctaveOffset = (int)tone/12;
            Tone = tone;
            Duration = duration;
            Velocity = velocity;

        }

        public Note OffsetBy(int offset, double? duration = null, byte? velocity = null)
        {
            byte tempVelocity = this.Velocity;
            double tempDuration = this.Duration;


            if (velocity != null)
            {
                tempVelocity = velocity.Value;
            }

            if (duration != null)
            {
                tempDuration = duration.Value;
            }

            return new Note(Tone + offset, tempDuration, tempVelocity);
        }
            
        public Note OffsetBy(Scale scale, int offset, double? duration = null, byte? velocity = null)
        {

            byte tempVelocity = this.Velocity;
            double tempDuration = this.Duration;


            if (velocity != null)
            {
                tempVelocity = velocity.Value;
            }

            if (duration != null)
            {
                tempDuration = duration.Value;
            }

            return new Note((Tone)(scale.Interval(Tone) + (int)scale[offset]), tempDuration, tempVelocity);
        }

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