using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using MidiGremlin.Internal;

namespace MidiGremlin.Tests
{
	[TestFixture]
	class ListExtensionsTest
	{
		[Test]
		public void TestPopLast()
		{
			List<int> list = new List<int>{ 2, 3, 5, 7 };

			Assert.AreEqual(7, list.PopLast());
			Assert.AreEqual(5, list.PopLast());
		}

		[Test]
		public void TestMergeInsert()
		{
			List<int> orginalList = new List<int>{ 2, 4, 6, 9 };

			orginalList.MergeInsert(3, (i, i1) => i1.CompareTo(i));
			orginalList.MergeInsert(7, (i, i1) => i1.CompareTo(i));

			CollectionAssert.AreEqual(new List<int> {2, 3, 4, 6, 7, 9}, orginalList);

			orginalList.MergeInsert(-3, (i, i1) => i1.CompareTo(i));

			CollectionAssert.AreEqual(new List<int> {-3,  2, 3, 4, 6, 7, 9 }, orginalList);

			orginalList.MergeInsert(12, (i, i1) => i1.CompareTo(i));


			CollectionAssert.AreEqual(new List<int> { -3, 2, 3, 4, 6, 7, 9, 12 }, orginalList);
		}

		[Test]
		public void TestLast()
		{
			List<int> orginalList = new List<int> { 2, 4, 6, 9 };

			Assert.AreEqual(9, orginalList.Last());
		}
	}
}
