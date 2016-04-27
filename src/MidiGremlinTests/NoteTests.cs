using NUnit.Framework;
using MidiGremlin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidiGremlin.Internal;
using NSubstitute;

namespace MidiGremlin.Tests
{
    [TestFixture()]
    public class NoteTests
    {
        [TestCase(4, Tone.E, 112)]
        [TestCase(-5, Tone.C, 0)]
        [TestCase(5, Tone.G, 127)]
        [TestCase(1, Tone.B, 83)]
        [TestCase(1, Tone.D, 74)]
        [TestCase(0, Tone.B, 71)]
        [TestCase(0, Tone.CSharp, 61)]
        [TestCase(0, Tone.C, 60)]
        [Test]
        public void GetChildrenTest(int octave, Tone tone, byte expectedPitch)
        {
            IOrchestra o = Substitute.For<IOrchestra>();
            Instrument i = new Instrument(o, InstrumentType.AccousticBass, new Scale(), octave);

            Note n = new Note(tone, 22, 33);

            SingleBeat actual = n.GetChildren(i, 0).First();
            SingleBeat expected = new SingleBeat(i.InstrumentType, expectedPitch, n.Velocity, 0, n.Duration);
            Assert.AreEqual(actual, expected);
        }
    }
}