using System;
using System.Collections.Generic;
using System.Linq;
using MidiGremlin;

namespace Für_Elise
{
    class Program
    {
        private const int bpm = 30;

        /// <summary>
        /// Creates a list of notes that all have the same duration and velocity.
        /// This can be used to make notation simpler when reading from a note-sheet full of barred notes.
        /// </summary>
        /// <param name="duration">The duration of all the notes. </param>
        /// <param name="velocity">The velocity of all the notes.</param>
        /// <param name="tones">The tones, in the order they should appear in the list.</param>
        /// <returns>A list of notes that all have the same duration and velocity.</returns>
        private static SequentialMusicList SimilarNotes(double duration, byte velocity, params Tone[] tones)
        {
            List<MusicObject> result = new List<MusicObject>(tones.Length);
            result.AddRange(tones
                .Select(tone => new Note(tone, duration, velocity)));

            return new SequentialMusicList(result);
        } 



        /// <summary>
        /// Plays the first bit of Für Elise, read from
        /// https://upload.wikimedia.org/wikipedia/commons/6/6b/Für_Elise_preview.svg
        /// Please take a look if you wish to follow the explanations given by the comments.
        /// For a full reference on note notaition, please refer to the most convenient university degree on the subject.
        /// </summary>
        static void Main ()
        {
            //Like most old western music, this is played in the A minor scale.
            //Note that as the tone enum starts at C, tones A and B are part of the lower octave.
            Tone[] majorScaleTones = {Tone.A - 12, Tone.B - 12, Tone.C, Tone.D, Tone.E, Tone.F, Tone.G};
            Scale majorScale = new Scale(majorScaleTones); 
            //The left hand is lowered by 1 octave.
            Scale loweredMajorScale = new Scale(majorScaleTones.Select(x => x - 12).ToArray());
            double eigth = 1 / 8D;
            byte baseVelocity = 50;

            //Creating the right hand:

            //The treble clef shows the position of the G
            //(which has index 6 in majorScale)
            // so by counting a note's offset from the G 
            // and adding the value corresponding to G in the scale
            // you get the tone you want.
            int trebleClef = 6;

			//Making all the six bars:

            MusicObject rBar0 =
                SimilarNotes(eigth, baseVelocity,
                    majorScale[5 + trebleClef],
                    majorScale[4 + trebleClef] + 1);

            MusicObject rBar1And5 =
                SimilarNotes(eigth, baseVelocity,
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
                SimilarNotes(eigth, baseVelocity,
                    majorScale[-4 + trebleClef],
                    majorScale[-2 + trebleClef],
                    majorScale[1 + trebleClef])
                );

            MusicObject rBar3 = new SequentialMusicList
                (
                new Note(majorScale[2 + trebleClef], eigth*2, baseVelocity),
                new Pause(eigth),
                SimilarNotes(eigth, baseVelocity,
                    majorScale[-2 + trebleClef],
                    majorScale[0 + trebleClef] + 1,
                    majorScale[2 + trebleClef])
                );

            MusicObject rBar4 = new SequentialMusicList
                (
                new Note(majorScale[3 + trebleClef], eigth*2, baseVelocity),
                new Pause(eigth),
                SimilarNotes(eigth, baseVelocity,
                    majorScale[-2 + trebleClef],
                    majorScale[5 + trebleClef],
                    majorScale[4 + trebleClef] + 1)
                );

			//rBar5 is already accounted for

            //The whole right hand.
            MusicObject rightHand = new SequentialMusicList(rBar0, rBar1And5, rBar2, rBar3, rBar4, rBar1And5);

            //Creating the left hand:

            //The bass clef shows the position of the F
            //(which has index 5 in majorScale)
            // so by counting a note's offset from the F 
            // and adding the value corresponding to F in the scale
            // you get the tone you want.
            int bassClef = 5;

            //Making all six bars:

            MusicObject lBar0 = new Pause(eigth*2);

            MusicObject lBar1And5 = new Pause(eigth*6);

            MusicObject lBar2And4 = new SequentialMusicList
                (
                SimilarNotes(eigth, baseVelocity,
                    loweredMajorScale[-5 + bassClef],
                    loweredMajorScale[-1 + bassClef],
                    loweredMajorScale[2 + bassClef]),
                new Pause(eigth),
                new Pause(eigth*2)
                );

            MusicObject lBar3 = new SequentialMusicList
                (
                SimilarNotes(eigth, baseVelocity,
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

            MusicObject leftHand2 = leftHand.Select<Note>(x => x);

            //To play music, first we need an orchestra with access to a player
            Orchestra o = new Orchestra(new WinmmOut(0, bpm));

            //It should be played on a grand piano. Let's just get one.
            Instrument piano = o.AddInstrument(InstrumentType.AccousticGrandPiano, majorScale, 0);

            //And so we just start it.
            piano.Play(leftHand2);
            piano.Play(rightHand);

			o.WaitForFinished();
	        Console.ReadLine();
        }
    }
}
