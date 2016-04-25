﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    ///<summary>
    ///The Orchestra class creates new instances of the instrument class.
    ///It works as a compilation for these instruments.
    ///</summary>
   public class Orchestra : IOrchestra
    {
        private readonly IMidiOut _output;
        private List<Instrument> _instruments = new List<Instrument>();
        
        
        public IReadOnlyCollection<Instrument> Instruments => _instruments.AsReadOnly();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        /// <param name="beatsPerMinutes">The amount of beats per 60 seconds.
        /// If you set beats per min to 60 then you can consider it as seconds</param>
        public Orchestra (IMidiOut output, int beatsPerMinutes=60)
        {
            _output = output;

            _bpm = beatsPerMinutes;
        }
        private int _bpm;
        public int BeatsPerMinutes { get { return _bpm; } set { _bpm = value; } }

        /// <summary> Conversion constant between minutes and milliseconds. </summary>
        private static double _minutesToMilliseconds = (5 / 3) * Math.Pow(10, -5);
        /// <summary>
        /// The duration of 1 beat in milliseconds.
        /// </summary>
        /// <returns>The duration of 1 beat in milliseconds.</returns>
        public double BeatDuratinInMilliseconds()
        {
            double durationInMinutes = 1 / BeatsPerMinutes;
            double durationInMilliseconds = durationInMinutes * _minutesToMilliseconds;
            return durationInMilliseconds;
        }


        /// <summary>
        /// Constructs a new instrument and adds it to the orchestra.
        /// </summary>
        /// <param name="instrumentType">Enum that represents an instrument</param>
        /// <param name="ocatave">The interval between two tones.Default is 3</param>
        /// <returns> Returns an overloaded AddInstrument,
        /// where the the scale parameter is set to the ChromaticScale </returns>
        public Instrument AddInstrument(InstrumentType instrumentType, int ocatave = 3)
        {
            return AddInstrument(instrumentType, Scale.ChromaticScale, ocatave);
        }
        /// <summary>
        /// Constructs a new instrument and adds it to the orchestra. With scale as a parameter
        /// </summary>
        /// <param name="instrumentType">Enum that represents an instrument</param>
        /// <param name="scale">In the scale you want the music to be played</param>
        /// <param name="octave">The interval between two tones.Default is 3</param>
        /// <returns></returns>
        public Instrument AddInstrument (InstrumentType instrumentType, Scale scale, int octave=3)
        {
            Instrument instrument = new Instrument(this, instrumentType, scale, octave);
            _instruments.Add(instrument);

            return instrument;
        }

        void IOrchestra.CopyToOutput(List<SingleBeat> music)
        {
            //TODO Allocate channel
            _output.QueueMusic(music.Select(x => new SingleBeatWithChannel(x.instrumentType, x.ToneOffset, x.ToneVelocity, x.ToneStartTime, x.ToneEndTime, 0)));
        }
        /// <summary>
        /// Returns the current time 
        /// </summary>
        /// <returns>Returns the current time </returns>
        public int CurrentTime()
        {
            return _output.CurrentTime();
        }

        

    }

    internal interface IOrchestra
	{
		void CopyToOutput(List<SingleBeat> music);
		int CurrentTime();
	}
}
