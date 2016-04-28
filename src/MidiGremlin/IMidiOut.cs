using System;
using System.Collections.Generic;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    ///<summary>
    ///An interface that manages output of the music.
    ///</summary>
    public interface IMidiOut : IDisposable
    {
        /// <summary>
        /// The number of beats elapsed since this instance was created.
        /// </summary>
        /// <returns>The number of beats elapsed since this instance was created.</returns>
        int CurrentTime { get; }

        /// <summary>
        /// Queques a MusicObject to be output at the time the MusicObject indicates.
        /// </summary>
        /// <param name="music"></param>
        void QueueMusic(IEnumerable<SingleBeatWithChannel> music);
    }
}