using System;
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

		public static int IndexOfSmallest<T>(this T[] array) where T : IComparable<T>
		{
			int smallestIndex = 0;
			for (int i = 1; i < array.Length; i++)
			{
				if (array[smallestIndex].CompareTo(array[i]) > 0)
				{
					smallestIndex = i;
				}
			}

			return smallestIndex;
		} 
	}
}
