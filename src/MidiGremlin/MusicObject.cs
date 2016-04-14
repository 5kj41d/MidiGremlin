using System.Collections.Generic;

namespace MidiGremlin
{
    /*
     *  Potentially a list of other MusicObjects to be played in sequence. (SequentialMusicCollection)
     *  Potentially a list of other MusicObjects to be played at the same time. (Chord)
     *  Potentially an object signifying a pause. (Take a guess)
     *  Potentially an object signifying a pitch and a duration. (Tone)
     */
    public abstract class MusicObject
    {
        internal abstract IEnumerator<SingleBeat> GetChildren ();
    }
}