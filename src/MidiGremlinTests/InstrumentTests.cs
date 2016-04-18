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
        public void PlayTest()
        {
            IOrchestra orchestra = Substitute.For<IOrchestra>();
            orchestra.CurrentTime().Returns(0);
            Instrument i = new Instrument(orchestra, InstrumentType.AccousticGrandPiano, new Scale(), 4);
            i.Play(Tone.CSharp, 5);
            
            orchestra.Received().CopyToOutput(Arg.Any<List<SingleBeat>>());
            
        }

        [Test]
        public void PlayTest_1()
        {
            IOrchestra orchestra = Substitute.For<IOrchestra>();
            orchestra.CurrentTime().Returns(0);
            Instrument i = new Instrument(orchestra, InstrumentType.AccousticBass, new Scale(), 4);
            i.Play(Tone.DSharp, 4);
            List<SingleBeat> complist = new List<SingleBeat>();
            

            complist.Add(new SingleBeat(InstrumentType.AccousticBass,54,64,0,4));

            orchestra.Received().CopyToOutput(Arg.Is<List<SingleBeat>>(value => value.SequenceEqual(complist)));
        }

}
}