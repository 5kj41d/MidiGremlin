using NUnit.Framework;
using MidiGremlin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiGremlin.Tests
{
	[TestFixture()]
	public class InstrumentTypeExtensionsTests
	{
		[Test()]
		[TestCase(InstrumentType.Accordion, false)]
		[TestCase(InstrumentType.ElectricPiano1, false)]
		[TestCase(InstrumentType.AccousticGrandPiano, false)]
		[TestCase(InstrumentType.AcousticBassDrum, true)]
		[TestCase(InstrumentType.OpenTriangle, true)]
		public void IsDrumTest(InstrumentType type, bool expected)
		{
			Assert.AreEqual(type.IsDrum(), expected);
		}
	}
}