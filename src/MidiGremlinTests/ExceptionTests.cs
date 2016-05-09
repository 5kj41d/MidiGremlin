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
	public class ExceptionTests
	{
		[Test()]
		public void ToneNotFoundExceptionTest()
		{
			ToneNotFoundException ex = new ToneNotFoundException(Tone.CSharp);

			Assert.AreEqual(ex.Tone, Tone.CSharp);
		}

		[Test]
		public void ToneOutOfRangeExceptionTest()
		{
			ToneOutOfRangeException ex = new ToneOutOfRangeException(244);

			Assert.AreEqual(ex.Tone, 244);
		}
	}
}