using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiGremlin.Internal
{
	internal static class ArrayExtensions
	{
		public static int IndexOf<T>(this T[] array, T item)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (EqualityComparer<T>.Default.Equals(array[i], item)) return i;
			}

			return -1;
		}
	}
}
