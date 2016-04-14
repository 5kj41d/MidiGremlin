using System;
using System.Collections.Generic;
using System.Reflection;
using libmusic;

namespace ConsoleApplication
{
	class Program
	{
		static void Main(string[] args)
		{
			GeneratorFramework.AddAssembly(Assembly.GetExecutingAssembly());
            MagicSetup();
		}

		private static void MagicSetup()
		{
			const int defaultBpm = 110;
			
			Dictionary<char, Type> types = new Dictionary<char, Type>();
			char c = 'a';

			foreach (Type knownGenerator in GeneratorFramework.KnownSimpleGenerators)
			{
				types.Add(c, knownGenerator);
				Console.WriteLine($"{c} - {knownGenerator}");
				c++;
			}

			do
			{
				ConsoleKeyInfo info = Console.ReadKey(true);
				c = info.KeyChar;
			} while (!types.ContainsKey(c));

			Console.WriteLine($"\nSelected {types[c]}");

			Console.WriteLine($"Enter BPM?[default {defaultBpm}]");
			string bpmString = Console.ReadLine();

			int bpm;
			if (string.IsNullOrWhiteSpace(bpmString))
			{
				bpm = defaultBpm;
			}
			else if (!int.TryParse(bpmString, out bpm))
			{
				bpm = defaultBpm;
			}


			GeneratorFramework framework = new GeneratorFramework(bpm);
			framework.Add(types[c]);
			framework.Start();
			Console.ReadLine();


			Console.WriteLine("Stopping!");
			framework.Stop();
		}
	}
}
