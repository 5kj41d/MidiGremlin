using NUnit.Framework;
using MidiGremlin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidiGremlin.Internal;
using NSubstitute;

namespace MidiGremlin.Tests
{
    [TestFixture()]
    public class ParallelMusicCollectionTests
    {
        [Test()]
        public void ParallelMusicCollectionTest()
        {
            MusicObject[] testObjects = new MusicObject[5];
            testObjects[0] = new Keystroke(Tone.A, 2);
            testObjects[1] = new Keystroke(Tone.C, 4);
            testObjects[2] = new Keystroke(Tone.B, 3);
            testObjects[3] = new Keystroke(Tone.E, 1);
            testObjects[4] = new Keystroke(Tone.D, 5);

            ParallelMusicCollection pM = new ParallelMusicCollection(testObjects);
            Assert.IsTrue(pM.Contains(testObjects[0]) && pM.Contains(testObjects[1]) && pM.Contains(testObjects[2]) && pM.Contains(testObjects[3]) && pM.Contains(testObjects[4]));
        }

        [Test()]
        public void ParallelGetChildrenTest()
        {
            MusicObject[] testObjects = new MusicObject[5];
            testObjects[0] = new Keystroke(Tone.A, 2);
            testObjects[1] = new Pause(1);
            testObjects[2] = new Keystroke(Tone.B, 3);
            testObjects[3] = new Pause(1);
            testObjects[4] = new Keystroke(Tone.D, 5);
            List<SingleBeat> sTestList = new List<SingleBeat>();

            ParallelMusicCollection sTestCollection = new ParallelMusicCollection(testObjects);
            IOrchestra testOrc = Substitute.For<IOrchestra>();
            Instrument i = new Instrument(testOrc, InstrumentType.AcousticBass, new Scale());
            sTestList.AddRange(sTestCollection.GetChildren(i, 1));

            bool testBool = true;
            double testDouble = 1;
            foreach (SingleBeat sb in sTestList)
            {
                if ((sb.ToneStartTime != testDouble))
                {
                    testBool = false;
                }
            }

            Assert.IsTrue(testBool);
        }

        
    }
}