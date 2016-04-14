using System;
using System.Collections.Generic;

namespace MidiGremlin
{
    public class Note : MusicObject
    {
        public int Duration { get; set; }
        public Tone Tone { get; set; }
        public int Velocity { get; set; }
        public int OctaveOffset { get; set; }

        public Note (Tone tone, int duration, int velocity = 64)    //TODO: Make ctor that takes only offset
        {
            throw new NotImplementedException();
        }

        public Note OffsetBy(int offset, int? duration = null, int? velocity = null)
        {
            throw new NotImplementedException();
        }
        public Note OffsetBy (Scale scale, int offset, int? duration = null, int? velocity = null)
        {
            throw new NotImplementedException();
        }

        internal override IEnumerator<SingleBeat> GetChildren ()
        {
            throw new NotImplementedException();
        }
    }
}