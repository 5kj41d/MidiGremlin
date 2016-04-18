﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    ///<summary>
    ///The Orchestra class creates new instances of the instrument class
    ///It works as a compilation for these instruments.
    ///</summary>
    class Orchestra
    {
        private readonly IMidiOut _output;
        private List<Instrument> _instruments = new List<Instrument>();
        
        public Scale DefaultScale { get; set; } = new Scale(Tone.A, Tone.ASharp, Tone.B, Tone.C, Tone.CSharp, Tone.D, Tone.DSharp, Tone.E, Tone.F, Tone.FSharp, Tone.GSharp, Tone.GSharp);
        public IReadOnlyCollection<Instrument> Instruments => _instruments.AsReadOnly();


        public Orchestra (IMidiOut output)
        {
            _output = output;
        }


        public Instrument AddInstrument(InstrumentType instrumentType, int ocatave = 3)
        {
            return AddInstrument(instrumentType, DefaultScale, ocatave);
        }
        public Instrument AddInstrument (InstrumentType instrumentType, Scale scale, int octave=3)
        {
            Instrument instrument = new Instrument(this, instrumentType, scale, octave);
            _instruments.Add(instrument);

            return instrument;
        }

        internal void CopyToOutput(List<SingleBeat> music)
        {
            //TODO Allocate channel
            _output.QueueMusic(music.Select(x => new SingleBeatWithChannel(x.instrumentType, x.ToneOffset, x.ToneVelocity, x.ToneStartTime, x.ToneEndTime, 0)));
        }

        public int CurrentTime()
        {
            return _output.CurrentTime();
        }
    }
}
