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

        [Test]
        [TestCase(Tone.A, 0, null, -6)]
        [TestCase(Tone.B-12, -5, 1, -17)]
        [TestCase(Tone.ASharp, null, 4, null)]
        [TestCase(Tone.B+12, 7, 9, -5)]
        [TestCase(Tone.CSharp, null, null, null)]
        [TestCase(Tone.D+12*2, 15, null, -9)]
        [TestCase(Tone.F+12*5, 23, 27, -1)]
        public void ScaleIntervalTest(Tone tone, int? expected1, int? expected2, int? expected3)
        {
            try
            {
                //Scale of length 6.
                Scale s1 = new Scale(Tone.A, Tone.B, Tone.C, Tone.D, Tone.E + 12, Tone.F + 12*2);
                int result1 = s1.Interval(tone);
                Assert.AreEqual(expected1, result1);
            }
            catch (ToneNotFoundException)
            {
                Assert.Null(expected1);
            }

            try
            {
                //Scale of length 4.
                Scale s2 = new Scale(Tone.ASharp - 12, Tone.B - 12, Tone.C - 12, Tone.F - 12);
                int result2 = s2.Interval(tone);
                Assert.AreEqual(expected2, result2);
            }
            catch (ToneNotFoundException)
            {
                Assert.Null(expected2);
            }

            try
            {
                //Scale of length 6.
                Scale s3 = new Scale(Tone.A + 12, Tone.B + 12*2, Tone.C + 12*3, Tone.D + 12*4, Tone.E + 12*5, Tone.F + 12*6);
                int result3 = s3.Interval(tone);
                Assert.AreEqual(expected3, result3);
            }
            catch (ToneNotFoundException)
            {
                Assert.Null(expected3);
            }
        }

        [Test]
        [TestCase(Tone.G, false, false, false)]
        [TestCase(Tone.ASharp, false, true, false)]
        [TestCase(Tone.A, true, false, true)]
        [TestCase(Tone.B, true, true, true)]
        [TestCase(Tone.B+12*100, true, true, true)]
        [TestCase(Tone.E-12, true, false, true)]
        public void ScaleContainsTest(Tone tone, bool expected1, bool expected2, bool expected3)
        {
            Scale s1 = new Scale(Tone.A, Tone.B, Tone.C, Tone.D, Tone.E+12, Tone.F+12*2);
            Assert.AreEqual(s1.Contains(tone), expected1);

            Scale s2 = new Scale(Tone.ASharp-12, Tone.B-12, Tone.C-12, Tone.F-12);
            Assert.AreEqual(s2.Contains(tone), expected2);

            Scale s3 = new Scale(Tone.A+12, Tone.B+12*2, Tone.C+12*3, Tone.D+12*4, Tone.E+12*5, Tone.F+12*6);
            Assert.AreEqual(s3.Contains(tone), expected3);
        }
    }
}