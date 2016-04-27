using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using MidiGremlin.Internal;

namespace MidiGremlin.Tests
{
    [TestFixture()]
    public class InstrumentTests
    {
        [TestCase(4, Tone.E, 9, 100, 112)]
        [TestCase(1, Tone.D, 9, 100, 74)]
        [TestCase(0, Tone.D, 44, 55, 62)]
        [TestCase(0, Tone.C, 22, 33, 60)]
        [Test]
        public void PlayTest_SimpleNewTone(int octave, Tone tone, int duration, byte velocity, byte expectedToneOffset)
        {
            IOrchestra o = Substitute.For<IOrchestra>();
            Instrument i = new  Instrument(o, InstrumentType.AccousticBass, new Scale(), octave);
            i.Play(tone, duration, velocity);

            List<SingleBeat> expected = new List<SingleBeat>
            {
                new SingleBeat(i.InstrumentType, expectedToneOffset, velocity, 0, duration)
            };

            o.Received().CopyToOutput(Arg.Is<List<SingleBeat>>(value => value.SequenceEqual(expected)));
        }

    }
}