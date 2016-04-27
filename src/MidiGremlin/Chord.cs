using System;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    ///<summary>
    ///Used to construct and define the different chords.
    ///</summary>
    public class Chord
    {
        int[] _toneSteps;
        const double _defaultDuration = 2;
        
        //TODO Og så videre. Muligvis ikke rent faktisk en dur det her.
        public static Chord Major { get; } = new Chord(4, 7);

        /// <summary>
        /// A list of singlebeats.Are played on the same time.
        /// </summary>
        /// <param name="toneSteps">Distance from basetone.Basetone included .</param>
        public Chord(params int[] toneSteps)
        {
            _toneSteps = toneSteps;
        }
        /// <summary>
        ///  A list of singlebeats. Find chord by name.
        /// </summary>
        /// <param name="name">The name of a chord</param>
        /// <returns></returns>
        public static Chord Name(string name)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Creates a new instance of the chord with a default value duration and velocity, using a tone as the value of the root.
        /// </summary>
        /// <param name="tone">A Tone enum value depicting the root of the chord.</param>
        /// <returns></returns>
        public MusicObject this[Tone tone]
        {
            get
            {
                return WithBaseTone(tone, _defaultDuration);
            }
        }
        /// <summary>
        /// Creates a new muisc object which represents a chord with a given duration and velocity, using a tone as the value of the root.
        /// </summary>
        /// <param name="tone">An enum value depicting the root of the chord.</param>
        /// <param name="duration">The duration of the chord.</param>
        /// <param name="velocity">The velocity of the chord.</param>
        /// <returns></returns>
        public MusicObject WithBaseTone(Tone tone, double duration, byte velocity)
        {
            return new ChordInstance(tone, duration, velocity, _toneSteps);
        }
        /// <param name="tone">A Tone enum value depicting the root of the chord.</param>
        /// <param name="duration">The duration of the chord.</param>
        /// <returns></returns>
        public MusicObject WithBaseTone (Tone tone, double duration)
        {
            return new ChordInstance(tone, duration, _toneSteps);
        }
    }
}
