using System.Runtime.InteropServices;

namespace MidiGremlin.Internal
{
    public struct SingleBeat
    {
        InstrumentType instrumentType;
        byte ToneOffset;
        byte ToneVelocity;
        int ToneStartTime;
        int ToneEndTime;
    }

    public struct SingleBeatWithChannel
    {
        InstrumentType instrumentType;
        byte ToneOffset;
        byte ToneVelocity;
        int ToneStartTime;
        int ToneEndTime;

        byte Channel;
    }
}