using System;
using System.Collections.Generic;

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
            throw new NotImplementedException();
        }

        internal override IEnumerator<SingleBeat> GetChildren ()
        {
            throw new NotImplementedException();
        }
    }
}