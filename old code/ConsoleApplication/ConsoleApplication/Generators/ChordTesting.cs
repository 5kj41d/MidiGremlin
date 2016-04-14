using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libmusic;
using Midi;

namespace ConsoleApplication.Generators
{
    class ChordTesting : libmusic.IMusicGenerator
    {
        Melody mel = new Melody();
        float _MaxTimeUsed = 100;

        public ChordTesting()
        {

        }

        public float Deadline()//Tid før næste kald i beats
        {
            return _MaxTimeUsed += 100;//Skal øges
        }

        public ICollection<LibNoteMessage> GenerateMusic()//Returner en mængde noder til afspilning
        {
            List<LibNoteMessage> tempList = mel.GetList();
            return tempList;
        }

        public void Setup(InfoObject infoObject)// Ignore for now
        {
        }
        
    }
    class Melody
    {
        List<Chord> _melodyArray = new List<Chord>();
        List<LibNoteMessage> _noteArray = new List<LibNoteMessage>();
        public int Length
        {
            get { return _melodyArray.Count; }
        }
        Chord[] CMajorScale = { new Chord("C"), new Chord("D"), new Chord("E"), new Chord("F"), new Chord("G"), new Chord("A"), new Chord("B") };
        Chord[] AMinorScale = { new Chord("Am"), new Chord("Bm"), new Chord("Cm"), new Chord("Dm"), new Chord("Em"), new Chord("Fm"), new Chord("Gm") };
        
        public List<LibNoteMessage> GetList()
        {
            return _noteArray;
        }
        public Melody()
        {
            FillArray();
            SetVariables(4, 1);
        }
        public void SetVariables(int octave, int channel)
        {
            int i = 0;
            foreach(Chord c in _melodyArray)
            {
                foreach (Note n in c.NoteSequence)
                {
                    _noteArray.Add(new NoteOn(n.PitchInOctave(octave),80, i, channel));
                }
                i++;
            }
        }
        private void FillArray()
        {
            GenerateStrophe();
            GenerateChorus();
            GenerateStrophe();
        }
        private void GenerateStrophe()
        {
            //How do we generate this?
        }
        private void GenerateChorus()
        {
            
        }
    }
}
