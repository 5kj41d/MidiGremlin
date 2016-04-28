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
        void SetSource(BeatScheduler source);
	    int BeatsPerMinute { get; set; }
        /// <summary>
        /// The number of beats elapsed since this instance was created.
        /// </summary>
        /// <returns>The number of beats elapsed since this instance was created.</returns>
        double CurrentTime ();
    }
}
