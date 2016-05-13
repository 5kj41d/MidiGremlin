using System.Linq;

namespace MidiGremlin
{
    ///<summary>
    ///The class represents a scale as a list of tones from the cromatic scale.
    /// This is used when determining Chords.
    ///</summary>
    public class Scale
    {
        private readonly Tone[] _tones;
        private const int _numberOfTones = 12;



        /// <summary> The Chromatic Scale, which contains all 12 tones starting at A. </summary>
        public static Scale ChromaticScale { get; } = new Scale(Tone.A, Tone.ASharp, Tone.B, Tone.C, Tone.CSharp, Tone.D, Tone.DSharp, Tone.E, Tone.F, Tone.FSharp, Tone.G, Tone.GSharp);

        /// <summary> The amount of tones the scale contains. </summary>
        public int Count => _tones.Length;



        /// <summary>
        /// Uses the chromatic scale.
        /// </summary>
        public Scale() : this(ChromaticScale._tones)
        {
        }
        
        
        
        /// <summary>
        /// Creates a new instance of the Scale class.
        /// </summary>
        /// <param name="tones">The tones in the scale. </param>
        public Scale(params Tone[] tones)
        {
            _tones = tones;
        }



        /// <summary>
        /// Checks if the tone is in the scale.
        /// </summary>
        /// <param name="tone">The tone which is compared to the tones in the scale. </param>
        /// <returns>true if the tone is in the scale; false otherwise.</returns>
        public bool Contains(Tone tone)
        {
            Tone baseToneToCheck = (Tone) ((int) tone%_numberOfTones);
            if (baseToneToCheck < 0)
                baseToneToCheck += _numberOfTones;
            foreach (Tone t in _tones)
            {
                Tone baseToneInScale = (Tone) ((int) t%_numberOfTones);
                if (baseToneInScale < 0)
                    baseToneInScale += _numberOfTones;
                if (baseToneInScale == baseToneToCheck)
                    return true;
            }
            return false;
        }



        /// <summary>
        /// Finds the position of the specified tone within the scale.
        /// If the number p is returned searching for a tone t in scale S, writing S[p] will always yield t.
        /// </summary>
        /// <param name="tone">The tone whose interval will be returned.</param>
        /// <returns>Returns the position of the tone in the scale.</returns>
        /// <exception cref="ToneNotFoundException"> Thrown if the tone does not exist in the scale. </exception>
        public int Interval(Tone tone)
        {
            Tone baseToneToCheck = (Tone)((int)tone % _numberOfTones);
            if (baseToneToCheck < 0)
                baseToneToCheck += _numberOfTones;

            for (int i = 0; i < _tones.Length; i++)
            {
                Tone baseToneInScale = (Tone) ((int) _tones[i]%_numberOfTones);
                if (baseToneInScale < 0)
                    baseToneInScale += _numberOfTones;

                if (baseToneInScale == baseToneToCheck)
                {
                    //a = b + (a-b)
                    int octaveDelta = (tone - _tones[i])/_numberOfTones;
                    return i + (octaveDelta * _tones.Length);
                }
            }

            throw new ToneNotFoundException(tone);
        }



        /// <summary>
        /// Finds a tone in the scale corresponding to the given interval.
        /// </summary>
        /// <param name="interval">Position in scale.</param>
        /// <returns>The tone at the given interval.</returns>
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

                Tone result =  _tones[index] + (octaveOffset * _numberOfTones);
                return result;
            }
        }
    }
}
