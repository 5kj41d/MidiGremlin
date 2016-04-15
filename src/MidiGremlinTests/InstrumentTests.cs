using NUnit.Framework;
using MidiGremlin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MidiGremlin.Tests
{
    [TestFixture()]
    public class InstrumentTests
    {
        [Test()]
        public void PlayTest ()
        {
            Orchestra orchestra = new Orchestra(new WinmmOut(0));

            Instrument instrument = orchestra.AddInstrument(InstrumentType.Violin);

            instrument.Play(Tone.A, 5);

            Thread.Sleep(100);

            instrument.Play(Tone.B, 30, byte.MaxValue);

            Thread.Sleep(100);

            instrument.Play(Tone.CSharp, 100);

            Thread.Sleep(300);

            instrument.Play(Tone.B, 200);


            Thread.Sleep(100);

            instrument.Play(Tone.B, 500);


            Assert.Pass();
        }
    }
}