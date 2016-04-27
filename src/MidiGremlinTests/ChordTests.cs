using NUnit.Framework;
using MidiGremlin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        [TestCase(Tone.CSharp,5,64)]
        public void WithBaseToneTest(Tone tone, double duration, byte velocity)
        {
            Chord acd = new Chord(1,4,7);

            Orchestra orc = new Orchestra(NSubstitute.Substitute.For<IMidiOut>());
            List<SingleBeat> v =acd.WithBaseTone(tone, duration, velocity).GetChildren(orc.AddInstrument(InstrumentType.AccousticGrandPiano),0).ToList();

            

            List<SingleBeat> comp = new List<SingleBeat>();
            comp.AddRange(new Note(tone,duration,velocity).GetChildren(orc.AddInstrument(InstrumentType.AccousticGrandPiano), 0).ToList());
            comp.AddRange(new Note(Tone.E, duration, velocity).GetChildren(orc.AddInstrument(InstrumentType.AccousticGrandPiano), 0).ToList());
            comp.AddRange(new Note(Tone.G, duration, velocity).GetChildren(orc.AddInstrument(InstrumentType.AccousticGrandPiano), 0).ToList());

            Assert.IsTrue(v.SequenceEqual(comp));
        }

        [Test]
        [TestCase(Tone.CSharp, 5)]
        public void WithBaseToneTest(Tone tone, double duration)
        {
            Chord acd = new Chord(1, 4, 7);

            Orchestra orc = new Orchestra(NSubstitute.Substitute.For<IMidiOut>());
            List<SingleBeat> v = acd.WithBaseTone(Tone.CSharp, 5).GetChildren(orc.AddInstrument(InstrumentType.AccousticGrandPiano), 0).ToList();



            List<SingleBeat> comp = new List<SingleBeat>();
            comp.AddRange(new Note(tone, duration).GetChildren(orc.AddInstrument(InstrumentType.AccousticGrandPiano), 0).ToList());
            comp.AddRange(new Note(Tone.E, duration).GetChildren(orc.AddInstrument(InstrumentType.AccousticGrandPiano), 0).ToList());
            comp.AddRange(new Note(Tone.G, duration).GetChildren(orc.AddInstrument(InstrumentType.AccousticGrandPiano), 0).ToList());

            Assert.IsTrue(v.SequenceEqual(comp));
        }

        [Test]
        public void WithBaseToneIndexerTest()
        {
            Chord acd = new Chord(1,4,7);
            Orchestra orc = new Orchestra(NSubstitute.Substitute.For<IMidiOut>());

            List<SingleBeat> v = acd[Tone.CSharp].GetChildren(orc.AddInstrument(InstrumentType.Violin), 0).ToList();

            List<SingleBeat> comp = new List<SingleBeat>();
            SingleBeat compbeat = new SingleBeat(InstrumentType.Violin, 40, 64, 0, 2);
            comp.Add(compbeat);
            SingleBeat fisk = new SingleBeat(InstrumentType.Violin, 43, 64, 0, 2);
            comp.Add(fisk);
            SingleBeat ost = new SingleBeat(InstrumentType.Violin, 46, 64, 0, 2);
            comp.Add(ost);

            Assert.IsTrue(v.SequenceEqual(comp));
        }
    }
}