using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    /// <summary>
    /// Contains a <see cref="T:MidiGremlin.Keystroke"/> and a <see cref="T:MidiGremlin.Pause"/>.
    /// </summary>
    class Note : MusicObject
    {
        public Keystroke Keystroke;
        public Pause Pause;


        /// <summary>
        /// Creates a new instance of the Note class by initializing a <see cref="T:MidiGremlin.Keystroke"/> and a <see cref="T:MidiGremlin.Pause"/> with the same duration.
        /// </summary>
        /// <param name="tone"> The tone that the keystroke represents. </param>
        /// <param name="duration">How long the tone is played in beats, and how long to wait before playing the next MusicObject. </param>
        /// <param name="velocity"> The severety with which the keystroke is struck. </param>
        public Note (Tone tone, double duration, byte velocity = 64)
        {
            Keystroke = new Keystroke(tone, duration, velocity);
            Pause = new Pause(duration);
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
            List<SingleBeat> result = new List<SingleBeat>();
            result.AddRange(Keystroke.GetChildren(playedBy, startTime));
            result.AddRange(Pause.GetChildren(playedBy, startTime));

            return result;
        }
    }
}
