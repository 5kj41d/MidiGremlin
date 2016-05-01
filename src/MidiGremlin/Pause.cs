using System.Collections.Generic;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    ///<summary>
    ///The class Pause inherits from the class MusicObject.
    ///It represents an amount of time(in beats) before the next MusicObject in the list should be played.
    ///</summary>
    public class Pause : MusicObject
    {
        /// <summary>
        /// Property that determines how long the pause is.
        /// </summary>
        public double Duration { get; set; }


        /// <summary>
        /// Creates a new instance of the Pause class.
        /// </summary>
        /// <param name="duration">Determines the length of the pause. </param>
        public Pause (double duration)
        {
            Duration = duration;
        }


        /// <summary>
        /// Returns the full contents of this MusicObject as SingleBeats.
        /// These are modified by the octave of the instrument that played them.
        /// </summary>
        /// <param name="playedBy">The instrument that requests the children.</param>
        /// <param name="startTime">The time at which the SingleBeats should start playing.</param>
        /// <returns>The full contents of this MusicObject as SingleBeats.</returns>
        internal override IEnumerable<SingleBeat> GetChildren (Instrument playedBy, double startTime)
        {
	        yield return new SingleBeat(0, 0xff, 0xff, startTime + Duration, startTime + Duration);
        }
    }
}