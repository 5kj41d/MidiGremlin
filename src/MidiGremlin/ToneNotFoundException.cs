using System;

namespace MidiGremlin
{
    /// <summary>
    /// A simple class which handles exceptions that could happen when trying to get the index of a note that does not exist.
    /// </summary>
    public class ToneNotFoundException : Exception
    {
        /// <summary> The tone that was requested but not found. </summary>
        public Tone Tone { get; }

        
        
        /// <summary>
        /// Creates a new exception of this type.
        /// </summary>
        /// <param name="tone"> The tone that was not found. </param>
        public ToneNotFoundException(Tone tone)
        {
            Tone = tone;
        }
    }
}