using System;
using System.Collections.Generic;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    public interface IMidiOut : IDisposable
    {
        int CurrentTime();
        void QueueMusic(IEnumerable<SingleBeatWithChannel> music);
    }
}