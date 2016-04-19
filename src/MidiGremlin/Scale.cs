using System.Linq;
using System.Collections;

namespace MidiGremlin
{
    ///<summary>
    ///The class represents any scale, which is used when determining Chords.
    ///</summary>

    public class Scale
    {
        private readonly Tone[] _tones;
        private const int _numberOfTones = 12;

        public int Count => _tones.Length;

        public Scale(params Tone[] tones)
        {
            _tones = tones;
        }

        public bool Contains(Tone tone)
        {
            return _tones.Any(x => ((int)x % _numberOfTones) == ((int)tone % _numberOfTones));
        }

        public int Interval(Tone tone)
        {
            int octaveDelta = (int)tone / _numberOfTones;
            Tone rawTone = (Tone)((int)tone % _numberOfTones);

            for (int i = 0; i < _tones.Length; i++)
            {
                if (_tones[i] == rawTone)
                {
                    return i + (octaveDelta * _numberOfTones);
                }
            }

            throw new NoteNotFoundException(tone);
        }

        public Tone this[int interval]
        {
            get
            {
                int octaveOffset = interval / _tones.Length;
                int index = interval % _tones.Length;
                if (index < 0)
                    index += _numberOfTones;

                return _tones[index] + (octaveOffset * _numberOfTones);
            }
        }
    }
}
