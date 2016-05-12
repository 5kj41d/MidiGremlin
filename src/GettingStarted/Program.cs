using MidiGremlin;

namespace GettingStarted
{
	class Program
	{
		static void Main(string[] args)
		{

			#region Orchestra
			const int midiDeviceId = 0;  //Most computers will only have one
			const int beatsPerSecond = 60;  //Speed of the music
			IMidiOut output = new WinmmOut(midiDeviceId, beatsPerSecond);
			Orchestra orchestra = new Orchestra(output);

			Instrument piano = orchestra.AddInstrument(InstrumentType.AccousticGrandPiano);
			#endregion

			#region SingleSound
			//Play a single sound
			piano.Play(Tone.C, 1);
			orchestra.WaitForFinished();
			#endregion

			#region MusicObjects
			MusicObject longC = new Note(Tone.C, 1);
			MusicObject shortC = new Note(Tone.C, 0.5);
			MusicObject shortE = new Note(Tone.E, 0.5);
			MusicObject shortG = new Note(Tone.G, 0.5);

			//We can play any of those on an instrument if we wish
			piano.Play(shortG);
			orchestra.WaitForFinished();

			#endregion

			#region LargeMusicObject
			//Create 2 smaller MusicObjects made out of the base pieces
			MusicObject sequence1 = new SequentialMusicList(longC, shortC, shortE, shortE, shortG, shortG, shortC);
			MusicObject sequence2 = new SequentialMusicList(shortC, shortG, shortE, shortE, longC);

			//Now create a bigger MusicObject made of those 2 smaller ones.
			MusicObject bigMusicObject = new SequentialMusicList(sequence1, sequence2, sequence1);

			//We can play this too
			//We can play any of those on an instrument if we wish
			piano.Play(bigMusicObject);
			orchestra.WaitForFinished();

			#endregion


		}
	}
}
