using System;

namespace MidiGremlin
{
    ///<summary>
    ///The class Scale implC:\Users\Christian Skole\Documents\AAU-P2\code\MIDI Gremlin\MidiGremlin\SequentialMusicCollection.csements the most used scale in Music(twelvetonescale).
    ///This 
    ///</summary>
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