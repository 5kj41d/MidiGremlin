using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using libmusic;
using Midi;

namespace ConsoleApplication.Generators
{
	class DrumBeatGenerator : IMusicGenerator
	{
		private float _deadline = 0;
		private InfoObject _info;
		private Channel _channel;
		/// <summary>
		/// Setup method called by framework while also passing needed info in an object.
		/// 
		/// If Setup gets called multiple times it is an error and the IMusicGenerator may 
		/// throw an Exception or fail to function correctly
		/// </summary>
		/// <param name="infoObject">An object to interact with the framework</param>
		public void Setup(InfoObject infoObject)
		{
			_info = infoObject;
			_channel = _info.GetWorkingChannel(Instrument.SynthDrum);
		}

		/// <summary>
		/// Deadline for next call to generatemusic. 
		/// </summary>
		/// <returns>Last time to call GenerateMusic</returns>
		public float Deadline()
		{
			return _deadline;
		}

		/// <summary>
		/// Generates Music
		/// </summary>
		/// <returns>A collection of music notes</returns>
		public ICollection<LibNoteMessage> GenerateMusic()
		{
			List<LibNoteMessage> beat = new List<LibNoteMessage>();

			AcctualGenerationFunction(beat);
			
			_deadline += 2f;
			return beat;
		}

		public Pitch BigDrum { get; set; } = Pitch.C2;
		public Pitch SmallDrum { get; set; } = Pitch.D2;
	
		private Pitch _progress = Pitch.CNeg1;

		private void AcctualGenerationFunction(List<LibNoteMessage> beat)
		{
			Console.WriteLine(BigDrum);
			beat.Add(new NoteOnOff(BigDrum, 74, _deadline, 0.15f));
			beat.Add(new NoteOnOff(SmallDrum, 74, _deadline, 0.15f));
			beat.Add(new NoteOnOff(SmallDrum, 74, _deadline + 1, 0.15f));
		}
	}
}
