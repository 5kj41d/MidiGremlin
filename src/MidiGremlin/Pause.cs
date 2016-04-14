using System;
using System.Collections;
using System.Collections.Generic;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    public class Pause : MusicObject
    {
        public int Duration { get; set; }

        public Pause (int duration)
        {
            throw new NotImplementedException();
        }

        internal override IEnumerable<SingleBeat> GetChildren (Instrument playedBy, int startTime)
        {
            throw new NotImplementedException();
        }
    }
}