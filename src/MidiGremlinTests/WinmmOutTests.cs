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
    public class WinmmOutTests
    {
        [Test()]
        public void WinmmOutTest ()
        {
            new WinmmOut(0);
            Thread.Sleep(1000);
            Assert.Pass();
        }


        [Test()]
        public void CurrentTimeTest ()
        {
            Assert.Fail();
        }

        [Test()]
        public void QueueMusicTest ()
        {
            Orchestra o = new Orchestra(new WinmmOut(0), 8);
			Note n = new Note(Tone.C,  1000);
	        Instrument i = o.AddInstrument(InstrumentType.AccousticGrandPiano);

			i.Play(n);

			Thread.Sleep(5000);

			Assert.Pass();
        }
    }
}