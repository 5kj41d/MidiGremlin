using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using MidiGremlin.Internal;

namespace MidiGremlin.Tests
{
    [TestFixture()]
    public class ChordTests
    {
        //[Test()]
        //public void ChordTest()
        //{
        //    Assert.Fail();
        //}

        //[Test()]
        //public void NameTest()
        //{
        //    Assert.Fail();
        //}
        [Test]
        [TestCase(Tone.CSharp,5.0,64)]
        public void WithBaseToneTest(Tone tone, double duration, byte velocity)
        {
            ChordVariety acd = new ChordVariety(1,4,7);

            Orchestra orc = new Orchestra(NSubstitute.Substitute.For<IMidiOut>());
            List<SingleBeat> v =acd.WithBaseTone(tone, duration, velocity).GetChildren(orc.AddInstrument(InstrumentType.AccousticGrandPiano),0).ToList();

            

            List<SingleBeat> comp = new List<SingleBeat>();
            comp.AddRange(new Keystroke(tone,duration,velocity).GetChildren(orc.AddInstrument(InstrumentType.AccousticGrandPiano), 0).ToList());
            comp.AddRange(new Keystroke(Tone.E, duration, velocity).GetChildren(orc.AddInstrument(InstrumentType.AccousticGrandPiano), 0).ToList());
            comp.AddRange(new Keystroke(Tone.G, duration, velocity).GetChildren(orc.AddInstrument(InstrumentType.AccousticGrandPiano), 0).ToList());

			CollectionAssert.AreEqual(comp, v);
		}

        [Test]
        [TestCase(Tone.CSharp, 5.0)]
        public void WithBaseToneTest(Tone tone, double duration)
        {
            ChordVariety acd = new ChordVariety(1, 4, 7);

            Orchestra orc = new Orchestra(NSubstitute.Substitute.For<IMidiOut>());
            List<SingleBeat> v = acd.WithBaseTone(Tone.CSharp, 5).GetChildren(orc.AddInstrument(InstrumentType.AccousticGrandPiano), 0).ToList();

			List<SingleBeat> comp = new List<SingleBeat>();
            comp.AddRange(new Keystroke(tone, duration).GetChildren(orc.AddInstrument(InstrumentType.AccousticGrandPiano), 0).ToList());
            comp.AddRange(new Keystroke(Tone.E, duration).GetChildren(orc.AddInstrument(InstrumentType.AccousticGrandPiano), 0).ToList());
            comp.AddRange(new Keystroke(Tone.G, duration).GetChildren(orc.AddInstrument(InstrumentType.AccousticGrandPiano), 0).ToList());

            CollectionAssert.AreEqual(comp, v);
        }

        [Test]
		[TestCase(0d, 1d)]
		[TestCase(1d, 2d)]
        public void WithBaseToneIndexerTest(double start, double def)
        {
	        ChordVariety acd = new ChordVariety(1, 4, 7) {DefaultDuration = def};
	        Orchestra orc = new Orchestra(NSubstitute.Substitute.For<IMidiOut>());
	        Instrument ins = orc.AddInstrument(InstrumentType.Violin);

			List<SingleBeat> v = acd[Tone.CSharp].GetChildren(ins, start).ToList();
	        List<SingleBeat> comp = new List<SingleBeat>
	        {
		        new SingleBeat(InstrumentType.Violin, 61, 64, start, start + acd.DefaultDuration),
		        new SingleBeat(InstrumentType.Violin, 64, 64, start, start + acd.DefaultDuration),
		        new SingleBeat(InstrumentType.Violin, 67, 64, start, start + acd.DefaultDuration)
	        };

			CollectionAssert.AreEqual(comp, v);
        }
    }
}