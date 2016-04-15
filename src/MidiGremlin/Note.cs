using System;
using System.Collections;
using System.Collections.Generic;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    ///<summary>
    ///The class Note constructs an individual and simpel sound.
    ///The class consists of duration, tone, velocity and a Octaveoffset which represents which octave the note is placed at.
    ///</summary>
    public class Note : MusicObject
    {
        public Tone Tone { get; set; }
        public int Duration { get; set; }
        public byte Velocity { get; set; }
        public int OctaveOffset { get; set; }
        

        public Note (Tone tone, int duration, byte velocity = 64)
        {
            //OctaveOffset = (int)tone/12;
            Tone = tone;
            Duration = duration;
            Velocity = velocity;

        }

        public Note OffsetBy(int offset, int? duration = null, int? velocity = null)
        {
            throw new NotImplementedException();
        }
        public Note OffsetBy (Scale scale, int offset, int? duration = null, int? velocity = null)
        {
            throw new NotImplementedException();
        }


        internal override IEnumerable<SingleBeat> GetChildren (Instrument playedBy, int startTime)
        {
            byte velocity = Math.Min((byte) 127, Velocity);

            int tone = (int) Tone + (playedBy.Octave + OctaveOffset)*12;
            if (0 > tone || tone > 127)
                throw new ToneOutOfRangeExceptioin(tone);

            yield return new SingleBeat(playedBy.InstrumentType, (byte)tone, velocity, startTime, startTime + Duration);
        }
    }
}