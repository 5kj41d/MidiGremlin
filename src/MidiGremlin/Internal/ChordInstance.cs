using System;
using System.Collections;
using System.Collections.Generic;

namespace MidiGremlin.Internal
{
    internal class ChordInstance : MusicObject
    {
        List<Note> _notes = new List<Note>();
        internal ChordInstance(Tone tone, int duration, int[] toneSteps)
        {
            _notes.Add(new Note(tone, duration));
            foreach(int i in toneSteps)
            {
                _notes.Add(new Note(tone + i, duration));
            }
        }
        internal ChordInstance(Tone tone, int duration, byte velocity, int[] toneSteps)
        {
            _notes.Add(new Note(tone, duration, velocity));
            foreach(int i in toneSteps)
            {
                _notes.Add(new Note(tone + i, duration, velocity));
            }
        }
        internal override IEnumerable<SingleBeat> GetChildren(Instrument playedBy, int startTime)
        {
            
            foreach(Note n in _notes)
            {
                int tone = (int)n.Tone + (playedBy.Octave + n.OctaveOffset) * 12;
                yield return new SingleBeat(playedBy.InstrumentType, (byte)tone, n.Velocity, startTime, startTime + n.Duration);
            }
        }

    }
}