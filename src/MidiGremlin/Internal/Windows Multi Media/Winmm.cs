using System;
using System.Runtime.InteropServices;

namespace MidiGremlin.Internal.Windows_Multi_Media
{
    static class Winmm
    {
        /// <summary>
        /// Gets the number of active MIDI devices.
        /// http://pinvoke.net/default.aspx/winmm/midiOutGetNumDevs.html
        /// </summary>
        /// <returns>The number of MIDI devices available.</returns>
        [DllImport("winmm.dll", SetLastError = true)]
        public static extern uint midiOutGetNumDevs ();


	    /// <summary> 
	    /// Device capabilities.
	    /// http://pinvoke.net/default.aspx/winmm/midiOutGetDevCaps.html
	    /// </summary>
	    /// <param name="uDeviceID">Device id.</param>
	    /// <param name="lpMidiOutCaps">Struct containing capabilities of the MIDI device.</param>
	    /// <param name="cbMidiOutCaps">Size of the struct.</param>
	    /// <returns>Method call status.</returns>
	    [DllImport("winmm.dll", SetLastError = true)]
        public static extern MMRESULT midiOutGetDevCaps (UIntPtr uDeviceID, ref MIDIOUTCAPS lpMidiOutCaps, uint cbMidiOutCaps);


	    /// <summary>
	    /// Opens a MIDI output device for playback. 
	    /// http://pinvoke.net/default.aspx/winmm/midiOutOpen.html
	    /// </summary>
	    /// <param name="lphMidiOut">Pointer to which the device handle will be written.</param>
	    /// <param name="uDevDiceId">Dunno</param>
	    /// <param name="dwCallback">A callback for this function</param>
	    /// <param name="dwInstance">A pointer passed to the callback, for it to do as it wishes</param>
	    /// <param name="dwFlags">Describes the type of callback</param>
	    /// <returns>Method call status.</returns>
	    [DllImport("winmm.dll")]
        public static extern uint midiOutOpen (out IntPtr lphMidiOut, uint uDevDiceId, IntPtr dwCallback, IntPtr dwInstance, uint dwFlags);


        /// <summary>
        /// Closes the MIDI output device.
        /// http://pinvoke.net/default.aspx/winmm/midiOutClose.html
        /// </summary>
        /// <param name="hMidiOut">Handle for the device.</param>
        /// <returns>Method call status.</returns>
        [DllImport("winmm.dll")]
        public static extern uint midiOutClose (IntPtr hMidiOut);


        /// <summary>
        /// Sends a short MIDI message to the device with handle hMidiOut.
        /// </summary>
        /// <param name="hMidiOut">Handle of device.</param>
        /// <param name="dwMsg">The short message.</param>
        /// <returns>Method call status.</returns>
        [DllImport("winmm.dll")]
        public static extern uint midiOutShortMsg (IntPtr hMidiOut, int dwMsg);
    }
}
