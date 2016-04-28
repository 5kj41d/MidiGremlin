using NUnit.Framework;
using System.Threading;

namespace MidiGremlin.Tests
{
    [TestFixture()]
    public class WinmmOutTests
    {
        [Test()]
        public void WinmmOutTest ()
        {
            new WinmmOut(0, 60);
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
            Orchestra o = new Orchestra(new WinmmOut(0));
			Keystroke n = new Keystroke(Tone.C,  1000);
	        Instrument i = o.AddInstrument(InstrumentType.AccousticGrandPiano);

			i.Play(n);

			Thread.Sleep(5000);

			Assert.Pass();
        }
    }
}