using System;
using MidiGremlin;

namespace GettingStarted
{
	class Program
	{
		static void Main(string[] args)
		{

			
			#region Orchestra
			const int midiDeviceId = 0; //Most computers will only have one
			const int beatsPerSecond = 60; //Speed of the music
			IMidiOut output = new WinmmOut(midiDeviceId, beatsPerSecond);
			Orchestra orchestra = new Orchestra(output);

			Instrument piano = orchestra.AddInstrument(InstrumentType.BrightAcousticPiano);
			#endregion

			#region SingleSound

			//Play a single sound
			piano.Play(Tone.C, 1);
			orchestra.WaitForFinished();

			#endregion

			#region MusicObjects
			MusicObject longF = new Note(Tone.F, 1);
			MusicObject shortF = new Note(Tone.F, 0.5);
			MusicObject longA = new Note(Tone.A, 1);
			MusicObject shortA = new Note(Tone.A, 0.5);
			MusicObject longG = new Note(Tone.G, 1);
			MusicObject shortG = new Note(Tone.G, 0.5);


			Console.WriteLine("Press enter to play a single sound");
			Console.ReadLine();
			//We can play any of those on an instrument if we wish
			piano.Play(shortG);
			orchestra.WaitForFinished();

			#endregion

			#region LargeMusicObject

			//Create 2 smaller MusicObjects made out of the base pieces
			MusicObject sequence1 = new SequentialMusicList(longF, shortA, longG, shortA);
			MusicObject sequence2 = new SequentialMusicList(shortA, shortA, shortA, longA, shortF);
			
			//Now create a bigger MusicObject made of those 2 smaller ones and one new
			SequentialMusicList bigMusicObject = new SequentialMusicList(sequence1, sequence1, sequence2, new Note(Tone.D, 2));

			//We can play this too
			//We can play any of those on an instrument if we wish
			Console.WriteLine("Press enter to play a longer sequence of sound");
			Console.ReadLine();
			//piano.Play(bigMusicObject);
			//orchestra.WaitForFinished();

			#endregion

			#region Transformation
			//Make the #1th object a little lower on the scale, using a transform
			int[] offsets = { 0, -3, -3, -2 };
			bigMusicObject[1] = bigMusicObject[1].Select<Note>((x,y) => x.OffsetBy(Scale.MajorScale, offsets[y]));

			//Play our final piece.
			Console.WriteLine("Press enter to play \"Drømte mig en drøm i nat\", ");
			Console.ReadLine();
			piano.Play(bigMusicObject);
			orchestra.WaitForFinished();

			//You have just heard https://en.wikipedia.org/wiki/Dr%C3%B8mde_mik_en_dr%C3%B8m_i_nat
			#endregion

			#region OtherInstrument
			//Create a flute
			Instrument flute = orchestra.AddInstrument(InstrumentType.Flute);

			


			Console.WriteLine("Press enter to play \"Drømte mig en drøm i nat\" on a flute");
			Console.ReadLine();
			flute.Play(bigMusicObject);
			orchestra.WaitForFinished();
			#endregion


		}
	}
}
