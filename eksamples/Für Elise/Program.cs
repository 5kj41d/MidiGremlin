using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidiGremlin;

namespace Für_Elise
{
    class Program
    {

        /// <summary>
        /// Plays the first bit of Für Elise, read from
        /// https://upload.wikimedia.org/wikipedia/commons/6/6b/Für_Elise_preview.svg
        /// </summary>
        static void Main (string[] args)
        {
            //Like most old western it is played in the A minor scale.
            Scale s = new Scale(Tone.A, Tone.B, Tone.C, Tone.D, Tone.E, Tone.F, Tone.G);

            double eigth = 1 / 8D;
            byte baseVelocity = 48;

            //Creating the right hand:

            //The treble clef shows the position of the G
            // so by counting a note's offset from the G 
            // and adding the value corresponding to G in the scale
            // you get the tone you want.
            int trebleClef = 7; 
            
            //Making all the six bars:

            MusicObject rBar0 = new SequentialMusicCollection
            (
                new Note(s[5 + trebleClef], eigth, baseVelocity),
                new Note(s[4 + trebleClef] + 1, eigth, baseVelocity)
            );
            
            MusicObject rBar1 = new SequentialMusicCollection
            (
                new Note(s[5 + trebleClef], eigth, baseVelocity),
                new Note(s[4 + trebleClef] + 1, eigth, baseVelocity),
                new Note(s[5 + trebleClef] + 1, eigth, baseVelocity),
                new Note(s[2 + trebleClef] + 1, eigth, baseVelocity),
                new Note(s[4 + trebleClef], eigth, baseVelocity),
                new Note(s[3 + trebleClef], eigth, baseVelocity)
            );
            
            MusicObject rBar2 = new SequentialMusicCollection
            (
                new Note(s[1 + trebleClef], eigth*2, baseVelocity),
                new Pause(eigth),
                new Note(s[-4 + trebleClef], eigth, baseVelocity),
                new Note(s[-2 + trebleClef], eigth, baseVelocity),
                new Note(s[1 + trebleClef], eigth, baseVelocity)
            );
            
            MusicObject rBar3 = new SequentialMusicCollection
            (
                new Note(s[2 + trebleClef], eigth*2, baseVelocity),
                new Pause(eigth),
                new Note(s[-2 + trebleClef], eigth, baseVelocity),
                new Note(s[0 + trebleClef] + 1, eigth, baseVelocity),
                new Note(s[2 + trebleClef] + 1, eigth, baseVelocity)
            );
            
            MusicObject rBar4 = new SequentialMusicCollection
            (
                new Note(s[3 + trebleClef], eigth*2, baseVelocity),
                new Pause(eigth),
                new Note(s[-2 + trebleClef], eigth, baseVelocity),
                new Note(s[5 + trebleClef], eigth, baseVelocity),
                new Note(s[4 + trebleClef] + 1, eigth, baseVelocity)
            );

            MusicObject rBar5 = rBar1;

            //The whole right hand.
            MusicObject rightHand = new SequentialMusicCollection
            (
                rBar0,
                rBar1,
                rBar2,
                rBar3,
                rBar4,
                rBar5
            );

            //Creating the left hand:

            //The bass clef shows the position of the F
            // so by counting a note's offset from the F 
            // and adding the value corresponding to F in the scale
            // you get the tone you want.
            int bassClef = 6;

            //Making all six bars:

            MusicObject lBar0 = new Pause(eigth*2);

            MusicObject lBar1 = new Pause(eigth*6);

            MusicObject lBar2 = new SequentialMusicCollection
            (
                new Note(s[-5 + bassClef], eigth, baseVelocity),
                new Note(s[-1 + bassClef], eigth, baseVelocity),
                new Note(s[2 + bassClef], eigth, baseVelocity),
                new Pause(eigth),
                new Pause(eigth*2)
            );

            MusicObject lBar3 = new SequentialMusicCollection
            (
                new Note(s[-8 + bassClef], eigth, baseVelocity),
                new Note(s[-1 + bassClef], eigth, baseVelocity),
                new Note(s[1 + bassClef] + 1, eigth, baseVelocity),
                new Pause(eigth),
                new Pause(eigth*2)
            );

            MusicObject lBar4 = lBar2;

            MusicObject lBar5 = lBar1;

            //The whole left hand.
            MusicObject leftHand = new SequentialMusicCollection
            (
                lBar0,
                lBar1,
                lBar2,
                lBar3,
                lBar4,
                lBar5
             );

            //Both hands:
            MusicObject furEliseIntro = new ParallelMusicCollection(rightHand, leftHand);

            //To play music, first we need an orchestra with access to a player
            Orchestra o = new Orchestra(new WinmmOut(0, 120));

            //It should be played on a grand piano. Let's just get one.
            Instrument piano = o.AddInstrument(InstrumentType.AccousticGrandPiano, s, 0);

            //And so we just start it.
            piano.Play(furEliseIntro);
        }
    }
}
