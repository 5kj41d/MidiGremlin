using System;
using System.Collections.Generic;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    ///<summary>
    ///IMidiOutput is a interface for the output that the user expects to recieve.
    ///</summary>
    public interface IMidiOut : IDisposable
    {
        int CurrentTime();
        void QueueMusic(IEnumerable<SingleBeatWithChannel> music);
    }
}