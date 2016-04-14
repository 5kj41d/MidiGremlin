using NUnit.Framework;
using MidiGremlin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [TestCase(Tone.A + 12, 6)]
        [TestCase(Tone.A + 24, 12)]
        [TestCase(Tone.B + 12, 7)]
        [TestCase(Tone.GSharp-12, -1)]
        //Far outside octave
        [TestCase(Tone.C + 12*100, 603)]
        [TestCase(Tone.B + 12 * 100, 602)]
        [TestCase(Tone.B - 12 * 100, -598)]
        public void ScaleGetToneTest (Tone expected, int index)
        {
            Scale scale = new Scale(Tone.A, Tone.B, Tone.C, Tone.D, Tone.F, Tone.GSharp);

            Assert.AreEqual(expected, scale[index]);
        }
    }
}