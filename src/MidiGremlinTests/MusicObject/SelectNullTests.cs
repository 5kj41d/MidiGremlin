using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace MidiGremlin.Tests
{
	[TestFixture]
	class SelectNullTests
	{
		static List<MusicObject> ListWithNull()
		{
			var list = ListWithoutNull();
			list.Add(null);
			return list;
		}

		static List<MusicObject> ListWithoutNull()
			=> new List<MusicObject>(Enumerable.Range(0, 10).Select(x => new Note(Tone.C + x, 1)));

		static MusicObject bigMusicObject()
		{
			SequentialMusicList list1 = new SequentialMusicList(ListWithoutNull().Shuffle(423)),
				list2 = new SequentialMusicList(list1, new SequentialMusicList(ListWithoutNull().Shuffle(2)));


			return new ParallelMusicCollection(list1, list2);
		}


		[Test]
		public void NullCtorArray()
		{
			Assert.Throws<NullReferenceException>(() => new ParallelMusicCollection(null));
		}

		[Test]
		[TestCase(423)]
		[TestCase(1423)]
		[TestCase(43243)]
		[TestCase(13373)]
		public void NullCtor(int shuffleseed)
		{
			var list = ListWithNull();
			list.Shuffle(shuffleseed);

			Assert.Throws<NullReferenceException>(() => new ParallelMusicCollection(list.ToArray()));
		}


		[Test]
		[TestCase(423)]
		[TestCase(1423)]
		[TestCase(43243)]
		[TestCase(13373)]
		public void NullCtorOther(int shuffleseed)
		{
			var list = ListWithNull();
			list.Shuffle(shuffleseed);

			Assert.Throws<NullReferenceException>(() => new ParallelMusicCollection(list));
		}

		[Test]
		public void KeyStrokeSelectTest()
		{
			Assert.Throws<NullReferenceException>(() => bigMusicObject().Select<Keystroke>(x => null));
		}

		[Test]
		public void NoteSelectTest()
		{
			Assert.Throws<NullReferenceException>(() => bigMusicObject().Select<Note>(x => null));
		}

		[Test]
		public void MusicObjectSelectTest()
		{
			Assert.Throws<NullReferenceException>(() => bigMusicObject().Select<MusicObject>(x => null));
		}

		[Test]
		public void ParallelMusicCollectionSelectTest()
		{
			Assert.Throws<NullReferenceException>(() => bigMusicObject().Select<ParallelMusicCollection>(x => null));
		}

		[Test]
		public void PauseSelectTest()
		{
			Assert.Throws<NullReferenceException>(() => bigMusicObject().Select<Pause>(x => null));
		}

		[Test]
		public void SequentialMusicListSelectTest()
		{
			Assert.Throws<NullReferenceException>(() => bigMusicObject().Select<SequentialMusicList>(x => null));
		}
	}
}
