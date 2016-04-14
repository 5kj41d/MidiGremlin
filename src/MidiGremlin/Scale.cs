using System;

namespace MidiGremlin
{
    public class Scale
    {
        public Scale(params Tone[] tones)
        {
            
        }

        MusicObject this[int interval]
        {
            get { throw new NotImplementedException();}
        }
    }
}