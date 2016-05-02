using NUnit.Framework;

namespace MidiGremlin.Tests
{
    [TestFixture]
    public class ScaleTests
    {
        [Test]
        [TestCase(Tone.B, 1)]
        [TestCase(Tone.A, 0)]
        [TestCase(Tone.GSharp, 5)]
        //Outside octave
        [TestCase(Tone.A + 12 * 1, 6)]
        [TestCase(Tone.A + 12 * 2, 12)]
        [TestCase(Tone.B + 12 * 1, 7)]
        [TestCase(Tone.GSharp - 12 * 1, -1)]
        [TestCase(Tone.C - 12 * 3, -16)]
        //Far outside octave
        [TestCase(Tone.C + 12 * 100, 602)]
        [TestCase(Tone.B + 12 * 100, 601)]
        [TestCase(Tone.B - 12 * 100, -599)]
        public void ScaleGetToneTest (Tone expected, int index)
        {
            Scale scale = new Scale(Tone.A, Tone.B, Tone.C, Tone.D, Tone.F, Tone.GSharp);

            Tone actual = scale[index];
            
            Assert.AreEqual(expected, actual);
        }
    }
}