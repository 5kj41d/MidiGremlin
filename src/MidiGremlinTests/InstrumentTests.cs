using NUnit.Framework;
using NSubstitute;
using MidiGremlin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MidiGremlin.Internal;

namespace MidiGremlin.Tests
{
    [TestFixture()]
    public class InstrumentTests
    {
        [Test()]
        public void PlayTest ()
        {
            Orchestra orchestra = Substitute.For<Orchestra>();
            orchestra.CurrentTime().Returns(0);
            Instrument i = new Instrument(orchestra, InstrumentType.AccousticGrandPiano, new Scale(), 4);
            i.Play(Tone.CSharp, 5);
            orchestra.Received().CopyToOutput(Arg.Any<List<SingleBeat>>());
        }
    }
}