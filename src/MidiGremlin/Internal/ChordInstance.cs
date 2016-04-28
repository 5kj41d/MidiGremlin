using System.Collections.Generic;

namespace MidiGremlin.Internal
{
    internal class ChordInstance : MusicObject
    {
        List<Keystroke> _notes = new List<Keystroke>();

        /// <summary>
        /// A chord consisting of a root tone and the tone-steps from the root-tone.
        /// The tone steps are indicated by intervals(Always on the full 12-tone scale). NB: The root tone has interval 1 and is not implicit.
        /// </summary>
        /// <param name="tone">The root tone of the chord.</param>
        /// <param name="duration">The duration of the chord.</param>
        /// <param name="toneSteps">The tones that the chord contains.</param>
        internal ChordInstance (Tone tone, double duration, int[] toneSteps)
        {
            //The root tone is not implicit.
            foreach(int i in toneSteps)
            {
                int interval = i - 1;   //The root tone is defined as having interval 1. So if i is 1, the tone should be shifted 0 intervals etc.
                _notes.Add(new Keystroke(tone + interval, duration));
            }
        }
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
            //The root tone is not implicit.
            foreach (int i in toneSteps)
            {
                int interval = i - 1; //The root tone is defined as having interval 1.
                _notes.Add(new Keystroke(tone + interval, duration, velocity));
            }
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
            foreach (Keystroke n in _notes)
            {
                int tone = (int)n.Tone + (playedBy.Octave + n.OctaveOffset) * 12;
                yield return new SingleBeat(playedBy.InstrumentType, (byte)tone, n.Velocity, startTime, startTime + n.Duration);
            }
        }

    }
}