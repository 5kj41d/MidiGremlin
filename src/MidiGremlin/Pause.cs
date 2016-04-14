using System;
using System.Collections.Generic;

namespace MidiGremlin
{
    public class Pause : MusicObject
    {
        public int Duration { get; set; }

        public Pause (int duration)
        {
            throw new NotImplementedException();
        }

        internal override IEnumerator<SingleBeat> GetChildren ()
        {
            throw new NotImplementedException();
        }
    }
}