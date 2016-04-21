using System.Collections;
using System.Collections.Generic;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    /*
     *  Potentially a list of other MusicObjects to be played in sequence. (SequentialMusicCollection)
     *  Potentially a list of other MusicObjects to be played at the same time. (Chord)
     *  Potentially an object signifying a pause. (Take a guess)
     *  Potentially an object signifying a pitch and a duration. (Tone)
     */
    ///<summary>
    ///The MusicObject is an abstract class that represents several different music structures. 
    ///It handles the individual tones, pauses, and a sequence of tones or chords
    ///</summary>
    public abstract class MusicObject
    {
        internal abstract IEnumerable<SingleBeat> GetChildren (Instrument playedBy, double startTime);
    }

}