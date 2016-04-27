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
        /// The Chromatic Scale, which contains all 12 tones in order.
        /// </summary>
        public static Scale ChromaticScale { get; } = new Scale(Tone.A, Tone.ASharp, Tone.B, Tone.C, Tone.CSharp, Tone.D, Tone.DSharp, Tone.E, Tone.F, Tone.FSharp, Tone.GSharp, Tone.GSharp);
        
        public int Count => _tones.Length;

        /// <summary>
        /// Uses the Chromaticscale
        /// </summary>
        public Scale() : this(ChromaticScale._tones)
        {
        }
        /// <summary>
        /// Constructs a new scale
        /// </summary>
        /// <param name="tones">enum which represent the tones in the scale </param>
        public Scale(params Tone[] tones)
        {
            _tones = tones;
        }
        /// <summary>
        /// Checks if the tone is in the scale
        /// </summary>
        /// <param name="tone">The tone which is compared to the tones in the scale </param>
        /// <returns>returns a bool depending if it is true or not</returns>
        public bool Contains(Tone tone)
        {
            return _tones.Any(x => ((int)x % _numberOfTones) == ((int)tone % _numberOfTones));
        }
        /// <summary>
        /// Finds the position of the tone in the scale 
        /// </summary>
        /// <param name="tone">The tone which interval will be returned</param>
        /// <returns>Returns the position of the tone in the scale</returns>
        public int Interval(Tone tone)
        {
            int octaveDelta = (int)tone / _numberOfTones;
            Tone rawTone = (Tone)((int)tone % _numberOfTones);

            for (int i = 0; i < _tones.Length; i++)
            {
                if (_tones[i] == rawTone)
                {
                    return i + (octaveDelta * _numberOfTones); // TODO this isn't should be tone.length?? 
                }
            }

            throw new NoteNotFoundException(tone);
        }
        /// <summary>
        /// Finds a tone in the scale from an interval
        /// </summary>
        /// <param name="interval">Position in scale</param>
        /// <returns>Returns a tone</returns>
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
