using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidiGremlin;


namespace MinimalExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Orchestra orchestra = new Orchestra(new WinmmOut(0, 70));
            Instrument bagpipe = orchestra.AddInstrument(InstrumentType.BagPipe);
            bagpipe.Play(Tone.CSharp, 1);
            orchestra.WaitForFinished();
        }
    }
}
