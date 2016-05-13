using System;
using System.Collections.Generic;
using System.Linq;
using MidiGremlin;

namespace Für_Elise
{
    /// <summary>
    /// Static method Main plays the first bit of Für Elise, read from
    /// https://upload.wikimedia.org/wikipedia/commons/6/6b/Für_Elise_preview.svg
    /// Please take a look if you wish to follow the explanations given by the comments.
    /// For a full reference on note notaition, please refer to the most convenient university degree on the subject.
    /// </summary>
    class FurEliseIntro
    {
        /// <summary>
        /// Helping class.
        /// Creates a list of notes that all have the same duration and velocity.
        /// This can be used to make notation simpler when reading from a note-sheet full of barred notes.
        /// </summary>
        /// <param name="duration">The duration of all the notes. </param>
        /// <param name="velocity">The velocity of all the notes.</param>
        /// <param name="keepSustainedFor"> Values not 0 causes behavior similar to pressing the sustain pedal for that many beats.</param>
        /// <param name="tones">The tones, in the order they should appear in the list.</param>
        /// <returns>A list of notes that all have the same duration and velocity.</returns>
        private static SequentialMusicList SimilarNotes(double duration, byte velocity, double keepSustainedFor, params Tone[] tones)
        {
            List<Note> result = new List<Note>(tones.Length);
            foreach (Tone tone in tones)
                result.Add(new Note(tone, duration, velocity));
            
            //Mimics the effect of keeping the Sustainment Pedal pressed on a piano.
            //Tones are normally cut off when a key on a piano is released, but this is not the case
            // as long as the sustain key is pressed.
            if (keepSustainedFor != 0)
            {
                double timeLeftToSustain = keepSustainedFor;  //Each note starts 1 duration sooner than the last.
                foreach (Note note in result)
                {
                    note.Keystroke.Duration += timeLeftToSustain;

                    timeLeftToSustain -= duration;  //Each note's duration should be 1 'duration' less than the last.
                    if (timeLeftToSustain < 0)
                        timeLeftToSustain = 0;
                }
            }

            return new SequentialMusicList(result);
        }



        static void Main ()
        {
            int bpm = 30; //Tempo of the music. Beats per minute.

            //Like most old western music, this is played in the A minor scale.
            //Note that as the tone enum starts at C, tones A and B are part of the lower octave.
            Tone[] majorScaleTones = {Tone.A - 12, Tone.B - 12, Tone.C, Tone.D, Tone.E, Tone.F, Tone.G};
            Scale majorScale = new Scale(majorScaleTones); 
            //The left hand is lowered by 1 octave.
            Scale loweredMajorScale = new Scale(majorScaleTones.Select(x => x - 12).ToArray());

            //We will be using these constants a lot.
            double eigth = 1 / 8.0;
            byte baseVelocity = 50;

            //To play music, first we need an orchestra with access to an output.
            Orchestra o = new Orchestra(new WinmmOut(0, bpm));
            //The piece should be played on a grand piano. Let's just get one.
            Instrument piano = o.AddInstrument(InstrumentType.AccousticGrandPiano, majorScale, 0);

            //----------------------------
            //   Creating the right hand:
            //----------------------------

            //The treble clef shows the position of the G
            //(which has index 6 in majorScale)
            // so by counting a note's offset from the G 
            // and adding the value corresponding to G in the scale
            // you get the tone you want.
            int trebleClef = majorScale.Interval(Tone.G);

			//Making all six bars:

            MusicObject rBar0 = //It seems, the first bar is only two eigths long.
                //The two notes have the same duration and velocity, so we are using a helper class.(The zero means the sustain pedal is not used.)
                SimilarNotes(eigth, baseVelocity, 0,
                    majorScale[5 + trebleClef],
                    majorScale[4 + trebleClef] + 1); //The ♯ elevates all tones on the line with 1 until cancelled by a ♮ or the bar ends.

            MusicObject rBar1And5 =
                SimilarNotes(eigth, baseVelocity, 0,
                    majorScale[5 + trebleClef],
                    majorScale[4 + trebleClef] + 1, 
                    majorScale[5 + trebleClef],
                    majorScale[2 + trebleClef],
                    majorScale[4 + trebleClef],
                    majorScale[3 + trebleClef]);

            MusicObject rBar2 = new SequentialMusicList
                (
                new Note(majorScale[1 + trebleClef], eigth*2, baseVelocity),
                new Pause(eigth),
                SimilarNotes(eigth, baseVelocity, 0,
                    majorScale[-4 + trebleClef],
                    majorScale[-2 + trebleClef],
                    majorScale[1 + trebleClef])
                );

            MusicObject rBar3 = new SequentialMusicList
                (
                new Note(majorScale[2 + trebleClef], eigth*2, baseVelocity),
                new Pause(eigth),
                SimilarNotes(eigth, baseVelocity, 0,
                    majorScale[-2 + trebleClef],
                    majorScale[0 + trebleClef] + 1,
                    majorScale[2 + trebleClef])
                );

            MusicObject rBar4 = new SequentialMusicList
                (
                new Note(majorScale[3 + trebleClef], eigth*2, baseVelocity),
                new Pause(eigth),
                SimilarNotes(eigth, baseVelocity, 0,
                    majorScale[-2 + trebleClef],
                    majorScale[5 + trebleClef],
                    majorScale[4 + trebleClef] + 1)
                );

			//rBar5 is already accounted for

            //The whole right hand.
            MusicObject rightHand = new SequentialMusicList(rBar0, rBar1And5, rBar2, rBar3, rBar4, rBar1And5);

            //----------------------------
            //   Creating the left hand:
            //----------------------------

            //The bass clef shows the position of the F
            //(which has index 5 in majorScale)
            // so by counting a note's offset from the F 
            // and adding the value corresponding to F in the scale
            // you get the tone you want.
            int bassClef = loweredMajorScale.Interval(Tone.F);

            //Making all six bars:

            MusicObject lBar0 = new Pause(eigth*2);

            MusicObject lBar1And5 = new Pause(eigth*6);

            MusicObject lBar2And4 = new SequentialMusicList
                (
                SimilarNotes(eigth, baseVelocity, 6,    //These notes should be sustained for the rest of the bar.
                    loweredMajorScale[-5 + bassClef],
                    loweredMajorScale[-1 + bassClef],
                    loweredMajorScale[2 + bassClef]),
                new Pause(eigth),
                new Pause(eigth*2)
                );

            MusicObject lBar3 = new SequentialMusicList
                (
                SimilarNotes(eigth, baseVelocity, 6,    //These notes should be sustained for the rest of the bar.
                    loweredMajorScale[-8 + bassClef],
                    loweredMajorScale[-1 + bassClef],
                    loweredMajorScale[1 + bassClef] + 1),
                new Pause(eigth),
                new Pause(eigth*2)
                );


			//lBar4 is already accounted for.

            //lBar5 is already accounted for.

            //The whole left hand.
            MusicObject leftHand = new SequentialMusicList(lBar0, lBar1And5, lBar2And4, lBar3, lBar2And4, lBar1And5);

            //----------------------------
            //   Ready to play:
            //----------------------------

            //The two hands should start playing at the same time:
            ParallelMusicCollection FurEliseIntro = new ParallelMusicCollection(leftHand, rightHand);

            //And then we just start it.
            piano.Play(FurEliseIntro);

			o.WaitForFinished();
	        Console.ReadLine();
        }
    }
}
