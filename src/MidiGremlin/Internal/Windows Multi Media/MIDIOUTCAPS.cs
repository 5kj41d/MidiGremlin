using System;
using System.Runtime.InteropServices;

namespace MidiGremlin.Internal.Windows_Multi_Media
{
    /// <summary>
    /// Struct that contains the capabilities of the MIDI output device.
    /// Modified version of 
    /// http://pinvoke.net/default.aspx/winmm/MIDIOUTCAPS.html
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct MIDIOUTCAPS
    {
        public ushort wMid;
        public ushort wPid;
        public uint vDriverVersion;     //MMVERSION
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szPname;
        public ushort wTechnology;
        public ushort wVoices;
        public ushort wNotes;
        public ushort wChannelMask;
        public uint dwSupport;
    }

    // values for wTechnology field of MIDIOUTCAPS structure
    enum MidiDeviceTechnologyType : ushort
    {
        MOD_MIDIPORT = 1,   // output port
        MOD_SYNTH = 2,      // generic internal synth
        MOD_SQSYNTH = 3,    // square wave internal synth
        MOD_FMSYNTH = 4,    // FM internal synth
        MOD_MAPPER = 5,     // MIDI mapper
        MOD_WAVETABLE = 6,  // hardware wavetable synth
        MOD_SWSYNTH = 7     // software synth
    }

    // flags for dwSupport field of MIDIOUTCAPS structure
    [Flags]
    enum dwSupport : ushort
    {
        MIDICAPS_VOLUME = 1,    // supports volume control
        MIDICAPS_LRVOLUME = 2,  // separate left-right volume control
        MIDICAPS_CACHE = 4,
        MIDICAPS_STREAM = 8,    // driver supports midiStreamOut directly
    }
}
