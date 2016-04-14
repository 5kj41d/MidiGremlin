using System;
using System.Collections.Generic;
using libmusic;
using Midi;

namespace ConsoleApplication.Generators
{
	class ShittyGeneratorExample : IMusicGenerator
	{
		private static readonly Pitch[] Pitches = {Pitch.A4, Pitch.B4, Pitch.C4, Pitch.D4};

		private InfoObject info;
		Random random = new Random();
		private float beat = 1;

		/// <summary>
		/// Setup method called by framework while also passing needed info in an object.
		/// 
		/// If Setup gets called multiple times it is an error and the IMusicGenerator may 
		/// throw an Exception or fail to function correctly
		/// </summary>
		/// <param name="infoObject">An object to interact with the framework</param>
		public void Setup(InfoObject infoObject)
		{
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
			List<LibNoteMessage> messages = new List<LibNoteMessage>();
			int state = random.Next(Pitches.Length);
			messages.Add(new NoteOnOff(Pitches[state], 80, beat, 1));
			beat += 1;
			return messages;
		}
	}
}
