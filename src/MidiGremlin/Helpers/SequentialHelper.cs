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

		public SequentialHelper Solo(Keystroke keystroke)
		{
			_list.Add(keystroke);
			_list.Add(new Pause(keystroke.Duration));
			return this;
		}

		public SequentialHelper Any(MusicObject any)
		{
			_list.Add(any);
			return this;
		}

		public SequentialMusicList Build()
		{
			return new SequentialMusicList(_list);
		}
	}
}
