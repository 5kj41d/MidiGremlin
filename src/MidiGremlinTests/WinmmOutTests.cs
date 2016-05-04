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

            Assert.DoesNotThrow(() => { var o = new WinmmOut(0, 60); o.Dispose();});

			
        }


        //[Test()]
        //public void CurrentTimeTest ()
        //{
        //    Assert.Fail();
        //}

        [Test()]
        public void QueueMusicTest ()
        {
	        using (WinmmOut winMM = new WinmmOut(0))
	        {
				Orchestra o = new Orchestra(winMM);
				Keystroke n = new Keystroke(Tone.C, 1000);
				Instrument i = o.AddInstrument(InstrumentType.AccousticGrandPiano);

				i.Play(n);

				Thread.Sleep(5000);
			}
        }
    }
}