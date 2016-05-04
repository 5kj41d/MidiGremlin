using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiGremlin.Internal
{
	internal static class ListExtensions
	{
		//Should probably be an extension method, insert an item into a sorted list keeping the list sorted
		public static void MergeInsert<T>(this List<T> list, T item, Comparison<T> comparison)
		{
			int i = 0;
			while (list.Count > i && comparison(list[i], item) > 0)
			{
				i++;
			}

			list.Insert(i, item);
		}

		public static T PopLast<T>(this List<T> list)
		{
			T t = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
			return t;
		}

		public static T LastItem<T>(this List<T> list)
		{
			return list[list.Count - 1];
		}
	}
}
