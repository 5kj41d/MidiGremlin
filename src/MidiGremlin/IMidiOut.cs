using System;
using System.Collections.Generic;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    ///<summary>
    ///IMidiOutput is a interface for the output which the user expects to recieve.
    ///</summary>
    public interface IMidiOut : IDisposable
    {
        double CurrentTime();
        void SetSource(BeatScheduler source);
	    int BeatsPerMinute { get; set; }
    }
}