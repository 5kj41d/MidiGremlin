using System;
using System.Collections;
using System.Collections.Generic;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    ///<summary>
    ///The class Pause inherits from the class MusicObject.
    ///The class Pause handles the time in a order which implements pauses in the composed music. 
    ///</summary>
    public class Pause : MusicObject
    {
        public int Duration { get; set; }

        public Pause (int duration)
        {
            Duration = duration;
        }

        internal override IEnumerable<SingleBeat> GetChildren (Instrument playedBy, int startTime)
        {
            yield return new SingleBeat();
        }
    }
}