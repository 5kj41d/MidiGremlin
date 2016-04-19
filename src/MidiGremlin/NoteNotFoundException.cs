using System;

namespace MidiGremlin
{
    /// <summary>
    /// The class NoteNotFoundException is a simple class which handles exceptions that could happen when running the program. 
    /// </summary>
    public class NoteNotFoundException : Exception
    {
        public Tone Tone { get; }

        public NoteNotFoundException(Tone tone)
        {
            Tone = tone;
        }
    }
}