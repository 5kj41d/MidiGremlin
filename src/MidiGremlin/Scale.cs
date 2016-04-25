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

        /// <summary>
        /// The Chromatic Scale, witch contains all 12 tones in order.
        /// </summary>
        public static Scale ChromaticScale { get; } = new Scale(Tone.A, Tone.ASharp, Tone.B, Tone.C, Tone.CSharp, Tone.D, Tone.DSharp, Tone.E, Tone.F, Tone.FSharp, Tone.GSharp, Tone.GSharp);
        
        public int Count => _tones.Length;


        public Scale() : this(ChromaticScale._tones)
        {
        }
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
                {
                    index += _tones.Length; //So as to have a sort of overflow when it becomes negative.
                    octaveOffset -= 1;  //Negative intervals start at offset -1.
                }

                return _tones[index] + (octaveOffset * _numberOfTones);
            }
        }
    }
}
