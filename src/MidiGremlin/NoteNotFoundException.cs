using System;

namespace MidiGremlin
{
    public class NoteNotFoundException : Exception
    {
        public Tone Tone { get; }

        public NoteNotFoundException(Tone tone)
        {
            Tone = tone;
        }
    }
}