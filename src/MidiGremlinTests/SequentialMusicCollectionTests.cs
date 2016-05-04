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
    public class SequentialMusicCollectionTests
    {
        [Test]
        public void SequentialMusicCollectionTest()
        {
            MusicObject[] testObjects = new MusicObject[5];
            testObjects[0] = new Keystroke(Tone.A, 2);
            testObjects[1] = new Keystroke(Tone.C, 4);
            testObjects[2] = new Keystroke(Tone.B, 3);
            testObjects[3] = new Keystroke(Tone.E, 1);
            testObjects[4] = new Keystroke(Tone.D, 5);

            SequentialMusicList sTest = new SequentialMusicList(testObjects);
            Assert.IsTrue(sTest.Contains(testObjects[0]) && sTest.Contains(testObjects[1]) && sTest.Contains(testObjects[2]) && sTest.Contains(testObjects[3]) && sTest.Contains(testObjects[4]));
        }
        [Test]
        public void SequentialGetChildrenTest()
        {
            MusicObject[] testObjects = new MusicObject[5];
            testObjects[0] = new Keystroke(Tone.A, 2);
            testObjects[1] = new Pause(1);
            testObjects[2] = new Keystroke(Tone.B, 3);
            testObjects[3] = new Pause(1);
            testObjects[4] = new Keystroke(Tone.D, 5);
            List<SingleBeat> sSBList = new List<SingleBeat>();

            SequentialMusicList sTestList = new SequentialMusicList(testObjects);
            IOrchestra testOrc = Substitute.For<IOrchestra>();
            Instrument i = new Instrument(testOrc, InstrumentType.AcousticBass, new Scale());
            sSBList.AddRange(sTestList.GetChildren(i, 1));

            
            double testDouble = 0;
            foreach (SingleBeat sb in sSBList)
            {
                if (!(sb.ToneStartTime >= testDouble))
                {
                    Assert.Fail();
                }
                testDouble = sb.ToneStartTime;
            }
        }
        [Test]
        public void SequentialGetChildrenTest2()
        {
            MusicObject[] testObjects = new MusicObject[5];
            testObjects[0] = new Keystroke(Tone.A, 2);
            testObjects[1] = new Pause(1);
            testObjects[2] = new ChordVariety(1, 4, 7).WithBaseTone(Tone.E, 2);
            testObjects[3] = new Pause(1);
            testObjects[4] = new Keystroke(Tone.D, 2);
            List<SingleBeat> sSBList = new List<SingleBeat>();

            SequentialMusicList sTestList = new SequentialMusicList(testObjects);
            IOrchestra testOrc = Substitute.For<IOrchestra>();
            Instrument i = new Instrument(testOrc, InstrumentType.AcousticBass, new Scale());
            sSBList.AddRange(sTestList.GetChildren(i, 1));

            int testInt = 0 ;
            double testDouble = 0;
            foreach (SingleBeat sb in sSBList)
            {
                if ((sb.ToneStartTime > testDouble))
                {
                    testInt++;
                }
                testDouble = sb.ToneStartTime;
            }

            Assert.IsTrue(testInt == 3);
        }
    }
}