using System.Collections.Generic;

namespace MidiGremlin.Internal
{
    internal class ChordInstance : MusicObject
    {
        List<Note> _notes = new List<Note>();


        internal ChordInstance(Tone tone, double duration, int[] toneSteps)
        {
            //The root tone is not implicit.
            foreach(int i in toneSteps)
            {
                int interval = i - 1;   //The root tone is defined as having interval 1.
                _notes.Add(new Note(tone + interval, duration));
            }
        }
        internal ChordInstance(Tone tone, double duration, byte velocity, int[] toneSteps)
        {
            //The root tone is not implicit.
            foreach (int i in toneSteps)
            {
                int interval = i - 1; //The root tone is defined as having interval 1.
                _notes.Add(new Note(tone + interval, duration, velocity));
            }
        }


        internal override IEnumerable<SingleBeat> GetChildren(Instrument playedBy, double startTime)
        {
            foreach (Note n in _notes)
            {
                int tone = (int)n.Tone + (playedBy.Octave + n.OctaveOffset) * 12;
                yield return new SingleBeat(playedBy.InstrumentType, (byte)tone, n.Velocity, startTime, startTime + n.Duration);
            }
        }

    }
}