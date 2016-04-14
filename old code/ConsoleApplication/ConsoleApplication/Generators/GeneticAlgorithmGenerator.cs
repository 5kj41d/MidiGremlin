using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libmusic;
using ConsoleApplication.Genetic_Algorithms;


namespace ConsoleApplication.Generators
{
    class GeneticAlgorithmGenerator: IMusicGenerator
    {
        GeneticAlgoritm algorithm;
        Midi.Pitch[] pitch = { Midi.Pitch.A3, Midi.Pitch.G3, Midi.Pitch.C3, Midi.Pitch.D3, Midi.Pitch.E3, Midi.Pitch.F3 };
        const int musicValueCount = 6;

        int _velocity = 80;
        int _channel = 0;
        float temporaryTime = 0;
        public GeneticAlgorithmGenerator()
        {
            algorithm = new GeneticAlgoritm(musicValueCount, 100, pitch.Count(), new GeneticObject(1, 4, 2, 5, 3, 6));
            algorithm.AddObject(new GeneticObject(1, 2, 3, 4, 5, 6));
            algorithm.AddObject(new GeneticObject(6, 5, 4, 3, 2, 1));
            algorithm.AddObject(new GeneticObject(2, 4, 6, 1, 3, 5));
            algorithm.AddObject(new GeneticObject(1, 3, 5, 2, 4, 6));
            algorithm.AddObject(new GeneticObject(5, 5, 5, 5, 5, 5));
            algorithm.AddObject(new GeneticObject(2, 2, 2, 2, 2, 2));
        }
        private List<LibNoteMessage> GeneticObjectArrayConverter(int velocity, int channel)
        {
            GeneticObject[] gObjects = algorithm.StartAlgorithm().ToArray();
            List<LibNoteMessage> tempList = new List<LibNoteMessage>();
            foreach (GeneticObject gObject in gObjects)
            {
                foreach(LibNoteMessage lib in GeneticObjectToLibNoteMessage(gObject, velocity, channel))
                {
                    tempList.Add(lib);
                }
            }
            return tempList;
        }
        private List<LibNoteMessage> GeneticObjectToLibNoteMessage(GeneticObject gObject, int velocity, int channel)
        {
            int[] tempValues = gObject.GetValues();
            List<LibNoteMessage> tempNotes = new List<LibNoteMessage>();
            foreach(int integer in tempValues)
            { 
                tempNotes.Add(new NoteOn(pitch[integer - 1], velocity, temporaryTime++, channel));
            }
            return tempNotes;
        }

        public void Setup(InfoObject infoObject)
        {

        }

        public float Deadline()
        {
            return temporaryTime;
        }

        public ICollection<LibNoteMessage> GenerateMusic()
        {
            return GeneticObjectArrayConverter(_velocity, _channel);
        }
    }
}
