namespace MidiGremlin
{
    ///<summary>
    ///An enumeration which represents the tones by their names. 
    /// Starts at C. Values outside the ones enumerated are interpreted as part being a different octave.
    ///</summary>
    public enum Tone
    {
        /// <summary> C(First element in the enum.) </summary>
        C,
        /// <summary> C♯ </summary>
        CSharp,
        /// <summary> D </summary>
        D,
        /// <summary> D♯ </summary>
        DSharp,
        /// <summary> E </summary>
        E,
        /// <summary> F </summary>
        F,
        /// <summary> F♯ </summary>
        FSharp,
        /// <summary> G </summary>
        G,
        /// <summary> G♯ </summary>
        GSharp,
        /// <summary> A </summary>
        A,
        /// <summary> A♯ </summary>
        ASharp,
        /// <summary> B(Or H in european notation.) </summary>
        B
    }
}