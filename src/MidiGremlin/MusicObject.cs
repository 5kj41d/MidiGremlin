using System.Collections;
using System.Collections.Generic;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    /*
     *  Potentially a list of other MusicObjects to be played in sequence. (SequentialMusicCollection)
     *  Potentially a list of other MusicObjects to be played at the same time. (ChordVariety)
     *  Potentially an object signifying a pause. (Take a guess)
     *  Potentially an object signifying a pitch and a duration. (Tone)
     */
    /// <summary>
    /// The MusicObject is an abstract class that represents several different music structures. 
    /// It handles the individual tones, pauses and chords. 
    /// It also handles groups containing other MusicObjects, to be played either in sequence or in parallel.
    /// </summary>
    public abstract class MusicObject
    {
        /// <summary>
        /// Returns the full contents of this MusicObject as SingleBeats.
        /// These are modified by the octave of the instrument that played them.
        /// </summary>
        /// <param name="playedBy">The instrument that requests the children.</param>
        /// <param name="startTime">The time at which the SingleBeats should start playing.</param>
        /// <returns>The full contents of this MusicObject as SingleBeats.</returns>
        internal abstract IEnumerable<SingleBeat> GetChildren (Instrument playedBy, double startTime);
    }

}