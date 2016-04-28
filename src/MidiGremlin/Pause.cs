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
        /// <summary>
        /// Property which determines how long the pause is 
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// Pause constructor
        /// </summary>
        /// <param name="duration">Determines the length of a pause</param>
        public Pause (double duration)
        {
            Duration = duration;
        }

        internal override IEnumerable<SingleBeat> GetChildren (Instrument playedBy, double startTime)
        {
            yield return new SingleBeat();
        }
    }
}