using NUnit.Framework;
using System.Linq;
using MidiGremlin.Internal;
using NSubstitute;

namespace MidiGremlin.Tests
{
    [TestFixture]
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
            Instrument i = new Instrument(o, InstrumentType.AcousticBass, new Scale(), octave);

            Keystroke n = new Keystroke(tone, 22, 33);

            SingleBeat actual = n.GetChildren(i, 0).First();
            SingleBeat expected = new SingleBeat(i.InstrumentType, expectedPitch, n.Velocity, 0, n.Duration);
            Assert.AreEqual(actual, expected);
        }


        [TestCase(Tone.B, 1, 0, Tone.C + 12)]
        [TestCase(Tone.C, 0, 1, Tone.C + 12)]
        [TestCase(Tone.B, 1, 1, Tone.C + 12*2)]
        [TestCase(Tone.B, 0, 99, Tone.B + 12*99)]
        [TestCase(Tone.C, 12, 0, Tone.C + 12)]
        [TestCase(Tone.C, 14, 0, Tone.D + 12)]
        [Test]
        public void OffsetTest(Tone startTone, int offset, int octaveOffset, Tone expectedTone)
        {
            Keystroke k = new Keystroke(startTone, 1);

            Keystroke expected = new Keystroke(expectedTone, 1);
            Keystroke result = k.OffsetBy(offset, octaveOffset);

            Assert.AreEqual(expected.Tone, result.Tone);
        }
    }
}