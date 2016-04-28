using System;

namespace MidiGremlin
{
    /// <summary>
    /// Exception thrown when the tone contained in a Note class 
    /// is not supported by the MIDI standard because it is out of range.
    /// A rising or falling sequence of tones will be
    ///  outside normal human hearing(and perhaps speaker capacity)
    ///  long before this exception is thrown.
    /// </summary>
    public class ToneOutOfRangeExceptioin : Exception
    {
        /// <summary>
        /// The int value corresponding to the tone that could not be played. Check octave offsets and origin of tone.
        /// http://www.tonalsoft.com/pub/news/pitch-bend.aspx
        /// </summary>
        public int Tone { get; }

        /// <summary>
        /// Creates a new exception of this type.
        /// </summary>
        /// <param name="tone">The value of the tone after it was converted to the MIDI standard.</param>
        public ToneOutOfRangeExceptioin(int tone) : base("Should be between 0 and 127.")
        {
            Tone = tone;
        }
    }
}