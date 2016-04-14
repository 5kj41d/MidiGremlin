using System;
using System.Collections;
using System.Collections.Generic;

namespace MidiGremlin.Internal
{
    internal class ChordInstance : MusicObject
    {
        internal override IEnumerable<SingleBeat> GetChildren(Instrument playedBy, int startTime)
        {
            throw new NotImplementedException();
        }
    }
}