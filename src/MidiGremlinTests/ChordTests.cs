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

        [Test()]
        public void WithBaseToneTest()
        {
            Chord acd = new Chord(4,7);

            Orchestra orc = new Orchestra(NSubstitute.Substitute.For<IMidiOut>());
            List<SingleBeat> v =acd.WithBaseTone(Tone.CSharp, 5, 64).GetChildren(orc.AddInstrument(InstrumentType.AccousticGrandPiano),1).ToList();

            

            List<SingleBeat> comp = new List<SingleBeat>();
            SingleBeat compbeat = new SingleBeat(InstrumentType.AccousticGrandPiano, 37, 64, 1, 5);
            comp.Add(compbeat);

            Assert.IsTrue(v.SequenceEqual(comp));
        }
    }
}