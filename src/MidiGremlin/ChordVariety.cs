using System;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    ///<summary>
    ///Used to construct and define the different chords.
    ///</summary>
    public class ChordVariety
    {
        int[] _toneSteps;

        /// <summary> The default duration used when creating chord instances from index. </summary>
        public double DefaultDuration = 1;

	    /// <summary>
        /// Initialises a new ChordVariety defined by the tone steps.
        /// The tone steps are indicated by intervals(Always on the full 12-tone scale). NB: The root tone has interval 1 and is not implicit.
        /// </summary>
        /// <param name="toneSteps"> The tone steps that makes the chord. </param>
        public ChordVariety (params int[] toneSteps)
        {
            _toneSteps = toneSteps;
        }


        /// <summary>
        /// Creates a new instance of the chord using DefaultDuration and a standard velocity , using tone as the root.
        /// </summary>
        /// <param name="tone">A Tone enum value depicting the root of the chord.</param>
        /// <returns></returns>
        public MusicObject this[Tone tone]
        {
            get
            {
                return WithBaseTone(tone, DefaultDuration);
            }
        }

	    /// <summary>
        /// Creates a new muisc object which represents a chord with a given duration and velocity, using a tone as the value of the root.
        /// </summary>
        /// <param name="tone">An enum value depicting the root of the chord.</param>
        /// <param name="duration">The duration of the chord.</param>
        /// <param name="velocity">The velocity of the chord.</param>
        /// <returns>A single chord with the given root tone.</returns>
        public MusicObject WithBaseTone(Tone tone, double duration, byte velocity)
        {
            return new ChordInstance(tone, duration, velocity, _toneSteps);
        }
        /// <summary>
        /// Creates a new muisc object which represents a chord with a given duration and velocity, using a tone as the value of the root.
        /// </summary>
        /// <param name="tone">An enum value depicting the root of the chord.</param>
        /// <param name="duration">The duration of the chord.</param>
        /// <returns>A single chord with the given root tone.</returns>
        public MusicObject WithBaseTone (Tone tone, double duration)
        {
            return new ChordInstance(tone, duration, _toneSteps);
        }

		public static ChordVariety Major { get; } = new ChordVariety(1, 3, 5);

	}
}
