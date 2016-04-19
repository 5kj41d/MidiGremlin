using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    ///<summary>
    ///The class Chord is used to construct and define the different chords.
    ///</summary>
    public class Chord
    {
        int[] _toneSteps;
        const int _defaultDuration = 2;
        public Chord(params int[] toneSteps)
        {
            _toneSteps = toneSteps;
        }

        public static Chord Name(string name)
        {
            throw new NotImplementedException();
        }
            
        //TODO Og så videre. Muligvis ikke rent faktisk en dur det her.
        public static Chord Major { get; } = new Chord(4, 7);
        /// <summary>
        /// Creates a new instance of the chord with a default value duration and velocity, using a tone as the value of the root.
        /// </summary>
        /// <param name="tone">A Tone enum value depicting the root of the chord.</param>
        /// <returns></returns>
        public MusicObject this[Tone tone]
        {
            get
            {
                return new ChordInstance(tone, _defaultDuration, _toneSteps);
            }
        }
        /// <summary>
        /// Creates a new muisc object which represents a chord with a given duration and velocity, using a tone as the value of the root.
        /// </summary>
        /// <param name="tone">An enum value depicting the root of the chord.</param>
        /// <param name="duration">The duration of the chord.</param>
        /// <param name="velocity">The velocity of the chord.</param>
        /// <returns></returns>
        public MusicObject WithBaseTone(Tone tone, int duration, byte velocity)
        {
            return new ChordInstance(tone,duration, velocity, _toneSteps);
        }
    }
}
