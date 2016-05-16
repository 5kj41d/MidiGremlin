using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiGremlin.Tests
{
	internal static class TestHelpers
	{
		internal static List<T> Shuffle<T>(this List<T> list, int seed)
		{
			Random random = new Random(seed);

			for (int i = 0; i < list.Count; i++)
			{
				T temp = list[i];
				int pos = random.Next(i, list.Count);
				list[i] = list[pos];
				list[pos] = temp;
			}

			return list;
		}
	}
}
