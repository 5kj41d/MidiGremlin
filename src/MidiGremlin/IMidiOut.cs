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
		/// Sets the BeatScheduler that is responsible for feeding this IMidiOut with MIDI messages. Setting this multiple times is an error.
		/// </summary>
		/// <param name="source">The BeatScheduler</param>
        void SetSource(BeatScheduler source);

		/// <summary>
		/// Music speed in Beats per Minute. Setting this to sixty should make one beat take one secon.
		/// </summary>
	    int BeatsPerMinute { get; set; }



        /// <summary>
        /// The number of beats elapsed since this instance was created.
        /// </summary>
        /// <returns>The number of beats elapsed since this instance was created.</returns>
        double CurrentTime ();
    }
}
