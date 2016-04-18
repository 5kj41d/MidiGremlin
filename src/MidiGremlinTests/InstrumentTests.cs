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
            IOrchestra orchestra = Substitute.For<IOrchestra>();
            orchestra.CurrentTime().Returns(0);
            Instrument i = new Instrument(orchestra, InstrumentType.AccousticGrandPiano, new Scale(), 4);
            i.Play(Tone.CSharp, 5);
	        List<SingleBeat> v = Arg.Any<List<SingleBeat>>();

			orchestra.Received().CopyToOutput(v);
        }
    }
}