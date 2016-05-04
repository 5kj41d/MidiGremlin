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
		[Test()]
		[TestCase(Tone.C, 1, 3)]
		[TestCase(Tone.F, 1, 4)]
		[TestCase(Tone.CSharp, 1, 2)]
		[TestCase(Tone.G, 2, 4)]
		[TestCase(Tone.A, 1, 2)]
		[TestCase(Tone.ASharp, 1, 0)]
		[TestCase(Tone.B, 5, 1)]
		public void NoteTest(Tone tone, double duration, double starttime)
		{
			Note n = new Note(tone, duration);
			Orchestra o = new Orchestra(Substitute.For<IMidiOut>());
			Instrument i = o.AddInstrument(InstrumentType.Banjo);

			IEnumerable<SingleBeat> list = n.GetChildren(i, starttime);

			List<SingleBeat> expected = new List<SingleBeat>
			{
				new SingleBeat(i.InstrumentType, (byte)(tone + 60), 64, starttime, starttime+duration),
				new SingleBeat(0, 0xff, 0xff, starttime+duration,starttime+duration)
			};

			CollectionAssert.AreEqual(expected, list);
		}
	}
}