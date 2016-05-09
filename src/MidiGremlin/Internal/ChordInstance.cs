using System;
using System.Collections.Generic;
using System.Linq;

namespace MidiGremlin.Internal
{
    internal class ChordInstance : MusicObject
    {
        private int[] _toneSteps;
        private double _duration;
        private Tone _rootTone;
        private byte _velocity;

        /// <summary>
        /// A chord consisting of a root tone and the tone-steps from the root-tone.
        /// The tone steps are indicated by intervals(Always on the full 12-tone scale). NB: The root tone has interval 1 and is not implicit.
        /// </summary>
        /// <param name="tone">The root tone of the chord.</param>
        /// <param name="duration">The duration of the chord.</param>
        /// <param name="toneSteps">The tones that the chord contains.</param>
        internal ChordInstance (Tone tone, double duration, int[] toneSteps) : this(tone, duration, 64, toneSteps) { }
        
		/// <summary>
        /// A chord consisting of a root tone and the tone-steps from the root-tone.
        /// The tone steps are indicated by intervals(Always on the full 12-tone scale). NB: The root tone has interval 1 and is not implicit.
        /// </summary>
        /// <param name="tone">The root tone of the chord.</param>
        /// <param name="duration">The duration of the chord.</param>
        /// <param name="velocity"> The velocity of the chord.</param>
        /// <param name="toneSteps">The tones that the chord contains.</param>
        internal ChordInstance (Tone tone, double duration, byte velocity, int[] toneSteps)
        {
            _rootTone = tone;
            _duration = duration;
            _velocity = velocity;
            _toneSteps = toneSteps;
        }

        /// <summary>
        /// Returns the full contents of this MusicObject as SingleBeats.
        /// These are modified by the octave of the instrument that played them.
        /// </summary>
        /// <param name="playedBy">The instrument that requests the children.</param>
        /// <param name="startTime">The time at which the SingleBeats should start playing.</param>
        /// <returns>The full contents of this MusicObject as SingleBeats.</returns>
        internal override IEnumerable<SingleBeat> GetChildren(Instrument playedBy, double startTime)
        {
            Scale scale = playedBy.Scale;

            foreach (int t in _toneSteps)
            {
                int offset = t - 1; //Chords are defined with 1 being the first element. Not very programmer-like, I know.

                //New Keystroke with correct tone.
                Keystroke k = new Keystroke
                    (
                    scale[scale.Interval(_rootTone) + offset],
                    _duration, _velocity
                    );

                yield return k.GetChildren(playedBy, startTime)   //Tone conversion handled by Keystroke class.
                    .First();   //Keystroke is a leaf so there will only be one element in the list. Only want that.
            }
        }

        public override MusicObject Select<T>(Func<T, T> selector)
        {
            Keystroke result = new Keystroke(_rootTone, _duration, _velocity);

            if (this is T)
                return selector(result as T);
            else
                return result;
        }
    }
}