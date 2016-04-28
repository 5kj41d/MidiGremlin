using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiGremlin.Helpers
{
	public class SequentialHelper
	{
		private readonly List<MusicObject> _list = new List<MusicObject>();

		public SequentialHelper Solo(Note note)
		{
			_list.Add(note);
			_list.Add(new Pause(note.Duration));
			return this;
		}

		public SequentialHelper Any(MusicObject any)
		{
			_list.Add(any);
			return this;
		}

		public SequentialMusicCollection Build()
		{
			return new SequentialMusicCollection(_list);
		}
	}
}
