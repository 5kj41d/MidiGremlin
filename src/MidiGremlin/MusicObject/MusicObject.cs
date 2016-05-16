using System;
using System.Collections.Generic;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    /*
     *  Potentially a list of other MusicObjects to be played in sequence. (SequentialMusicList)
     *  Potentially a list of other MusicObjects to be played at the same time. (ChordVariety)
     *  Potentially an object signifying a pause. (Take a guess)
     *  Potentially an object signifying a pitch and a duration. (Tone)
     */
    /// <summary>
    /// The MusicObject is an abstract class that represents several different music structures. 
    /// It handles the individual tones, pauses and chords. 
    /// It also handles groups containing other MusicObjects, to be played either in sequence or in parallel.
    /// </summary>
    public abstract class MusicObject
    {
        /// <summary>
        /// Returns the full contents of this MusicObject as SingleBeats.
        /// These are modified by the octave of the instrument that played them.
        /// </summary>
        /// <param name="playedBy">The instrument that requests the children.</param>
        /// <param name="startTime">The time at which the SingleBeats should start playing.</param>
        /// <returns>The full contents of this MusicObject as SingleBeats.</returns>
        internal abstract IEnumerable<SingleBeat> GetChildren (Instrument playedBy, double startTime);



        /// <summary>
        /// Projects all music objects of specified type into a <see cref="MusicObject"/> of the same structure.
        /// </summary>
        /// <typeparam name="T">The MusicObject subtype to modify.</typeparam>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>A <see cref="MusicObject"/> of identical structure that is the result of invoking the transform function of all elements of type T.</returns>
        public abstract MusicObject Select<T>(Func<T, T> selector) where T : MusicObject;


		/// <summary>
		/// Projects all music objects of specified type into a <see cref="MusicObject"/> of the same structure.
		/// </summary>
		/// <typeparam name="T">The MusicObject subtype to modify.</typeparam>
		/// <param name="selector">A transform function to apply to each element with an optional count of how many objects have been transformed</param>
		/// <returns>A <see cref="MusicObject"/> of identical structure that is the result of invoking the transform function of all elements of type T.</returns>
		public MusicObject Select<T>(Func<T, int, T> selector) where T : MusicObject
	    {
		    IndexedSelectHelper<T> indexedSelectHelper = new IndexedSelectHelper<T>(selector);

		    return Select<T>(indexedSelectHelper.SelectMethod);
	    }

	    private class IndexedSelectHelper<T>
	    {
		    private readonly Func<T, int, T> _selectorFunction;
		    private int _count;
		    internal IndexedSelectHelper(Func<T, int, T> selectorFunction)
		    {
			    _selectorFunction = selectorFunction;
		    }

		    internal T SelectMethod(T input)
		    {
			    T temporary = _selectorFunction(input, _count);
			    _count++;
			    return temporary;
		    }
	    }
    }

}