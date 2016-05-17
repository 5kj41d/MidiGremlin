using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MidiGremlin.Internal;
using NUnit.Framework;

namespace MidiGremlin.Tests
{
	[TestFixture]
	class ChannelAllocatorTest
	{
		private class AsEnumerable : IEnumerable<SimpleMidiMessage>
		{
			private readonly ChannelAllocator _inner;

			public AsEnumerable(ChannelAllocator inner)
			{
				_inner = inner;
			}

			/// <summary>Returns an enumerator that iterates through the collection.</summary>
			/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
			public IEnumerator<SimpleMidiMessage> GetEnumerator()
			{
				while (!_inner.Empty)
				{
					yield return _inner.GetNext();
				}
			}

			/// <summary>Returns an enumerator that iterates through a collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		internal class Comp : IComparer
		{
			/// <summary>Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
			/// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero <paramref name="x" /> is less than <paramref name="y" />. Zero <paramref name="x" /> equals <paramref name="y" />. Greater than zero <paramref name="x" /> is greater than <paramref name="y" />. </returns>
			/// <param name="x">The first object to compare. </param>
			/// <param name="y">The second object to compare. </param>
			/// <exception cref="T:System.ArgumentException">Neither <paramref name="x" /> nor <paramref name="y" /> implements the <see cref="T:System.IComparable" /> interface.-or- <paramref name="x" /> and <paramref name="y" /> are of different types and neither one can handle comparisons with the other. </exception>
			public int Compare(object x, object y)
			{
				if (x is SimpleMidiMessage && y is SimpleMidiMessage)
				{
					SimpleMidiMessage xm = (SimpleMidiMessage) x;
					SimpleMidiMessage ym = (SimpleMidiMessage) y;

					return xm.Timestamp.CompareTo(ym.Timestamp);
				}
				return 0;
			}
		}

		[Test]
		public void ChannelRotation()
		{
			ChannelAllocator allocator = new ChannelAllocator();

			List<SingleBeat> test =
				new List<SingleBeat>(
					Enumerable.Range(0, 20).Select(i => new SingleBeat(InstrumentType.AccousticGrandPiano + i, 64, 64, i * 2, (i * 2) + 1)));

			Assert.DoesNotThrow(() => allocator.Add(test));
		}

		[Test]
		public void ChannelRotationReuse()
		{
			ChannelAllocator allocator = new ChannelAllocator();

			List<SingleBeat> test =
				new List<SingleBeat>(
					Enumerable.Range(0, 20).Select(i => new SingleBeat(InstrumentType.AccousticGrandPiano + i, 64, 64, i*2, (i*2) + 1)));

			List<SingleBeat> test2 =
				new List<SingleBeat>(
					Enumerable.Range(0, 20).Select(i => new SingleBeat(InstrumentType.AccousticGrandPiano + i, 64, 64, (i * 2) + 45, (i * 2) + 46)));

			Assert.DoesNotThrow(() => allocator.Add(test));

			Assert.DoesNotThrow(() => allocator.Add(test2));

			List<SimpleMidiMessage> list = new List<SimpleMidiMessage>(new AsEnumerable(allocator));
			CollectionAssert.IsOrdered(list, new Comp());
		}

		[Test]
		public void OverflowTest()
		{
			ChannelAllocator allocator = new ChannelAllocator();

			List<SingleBeat> test =
				new List<SingleBeat>(
					Enumerable.Range(0, 20).Select(i => new SingleBeat(InstrumentType.AccousticGrandPiano + i, 64, 64, i * 0.1, (i * 0.1) + 3)));

			Assert.Throws<OutOfChannelsException>(() => allocator.Add(test));
		}

		[Test]
		public void SauritateTest()
		{
			ChannelAllocator allocator = new ChannelAllocator();

			List<SingleBeat> test =
				new List<SingleBeat>(
					Enumerable.Range(0, 99).Select(i => new SingleBeat(InstrumentType.AccousticGrandPiano + (i % 15), 64, 64, i * 0.1, (i * 0.1) + 1.5)));

			Assert.DoesNotThrow(() => allocator.Add(test));

			List<SimpleMidiMessage> list = new List<SimpleMidiMessage>(new AsEnumerable(allocator));
			CollectionAssert.IsOrdered(list, new Comp());
		}

	}

	
}
