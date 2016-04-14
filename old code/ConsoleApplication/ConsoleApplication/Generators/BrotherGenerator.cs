using System;
using System.Collections.Generic;
using System.Linq;
using libmusic;
using Midi;

namespace ConsoleApplication.Generators
{
	class BrotherGenerator : IMusicGenerator
	{
		private static Pitch[] Pitches;

		private static readonly Dictionary<Pitch, Pitch[]> Second = new Dictionary<Pitch, Pitch[]>()
		{
			{ Pitch.CSharp4, new[] {Pitch.FSharp4, Pitch.ASharp3, Pitch.GSharp4, Pitch.F4, Pitch.CSharp3} },
			{ Pitch.DSharp4, new[] {Pitch.FSharp4, Pitch.ASharp3, Pitch.GSharp3, Pitch.C3, Pitch.DSharp3} },
			{ Pitch.FSharp4, new[] {Pitch.ASharp3, Pitch.CSharp3, Pitch.DSharp3, Pitch.FSharp3} },
			{ Pitch.GSharp4, new[] {Pitch.CSharp3, Pitch.DSharp3, Pitch.C3, Pitch.F3, Pitch.GSharp3} },
			{ Pitch.ASharp4, new[] {Pitch.FSharp4, Pitch.CSharp4, Pitch.DSharp4, Pitch.F4, Pitch.C4, Pitch.ASharp3} },
			{ Pitch.C4, new[] {Pitch.DSharp4, Pitch.F4, Pitch.GSharp4, Pitch.ASharp3, Pitch.C3} },
			{ Pitch.F4, new[] {Pitch.C3, Pitch.CSharp3, Pitch.GSharp4, Pitch.ASharp4, Pitch.F3} },
		};



		private InfoObject info;
		Random random = new Random();
		private float beat = 0;

		/// <summary>
		/// Setup method called by framework while also passing needed info in an object.
		/// 
		/// If Setup gets called multiple times it is an error and the IMusicGenerator may 
		/// throw an Exception or fail to function correctly
		/// </summary>
		/// <param name="infoObject">An object to interact with the framework</param>
		public void Setup(InfoObject infoObject)
		{
			Pitches = Second.Keys.ToArray();

			info = infoObject;
		}

		/// <summary>
		/// Deadline for next call to generatemusic. 
		/// </summary>
		/// <returns>Last time to call GenerateMusic</returns>
		public float Deadline()
		{
			return beat;
		}



		/// <summary>
		/// Generates Music
		/// </summary>
		/// <returns>A collection of music notes</returns>
		public ICollection<LibNoteMessage> GenerateMusic()
		{
			int remaining = 8;
			List<LibNoteMessage> messages = new List<LibNoteMessage>();

			Console.WriteLine("", beat/4);

			while (remaining > 0)
			{
				if (remaining != 8) Console.Write(" ");

				int[] sizes = new[] {1, 2, 3, 4, 8}.Where(x => x <= remaining).ToArray();

				int size = Math.Min(remaining, sizes[random.Next(sizes.Length)]);

				float lenght = size/2f;
				float ofset = (8 - remaining)/2f;

				int state = random.Next(Pitches.Length);
				messages.Add(new NoteOnOff(Pitches[state], 64 + random.Next(-32, 32), beat + ofset, lenght));

				Console.Write($"{$"({ofset}->{ofset + lenght})".PadRight(remaining == 8 ? 8 : 10)} {(Pitches[state]).ToString().Replace("Sharp", "#").PadRight(3)}".PadRight(14));

				Pitch[] second = Second[Pitches[state]];
				int index = random.Next(second.Length + 1);

				const int extra = 10;

				if (index >= second.Length)
				{
					Console.Write("".PadRight(extra));
                }
				else
				{
					messages.Add(new NoteOnOff(second[index], 64 + random.Next(-32, 32), beat + ofset, lenght));

					Console.Write($"->{second[index]}".Replace("Sharp", "#").PadRight(extra));
				}

				remaining -= size;
			}

			beat += 4;
			Console.WriteLine();


			
			return messages;
		}
	}
}
