using System.Collections.Generic;
using System.Linq;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    ///<summary>
    ///The Orchestra class creates and manages new instances of the instrument class.
    ///</summary>
   public class Orchestra : IOrchestra
    {
        private readonly IMidiOut _output;
        private readonly List<Instrument> _instruments = new List<Instrument>();
	    private readonly BeatScheduler _beatScheduler;
	    private IReadOnlyCollection<Instrument> _roInstrumentsCache;

		
        
        /// <summary>
		/// property of the instruments which is readonly
		/// </summary>
		public IReadOnlyCollection<Instrument> Instruments => _roInstrumentsCache ?? (_roInstrumentsCache = _instruments.AsReadOnly());


	    
        /// <summary>
        /// Creates a new instance of the orchestra class. 
        /// Needs a reference to an output class, which you can get by creating a new WinmmOut instance.
        /// </summary>
        /// <param name="output"> The output class to send all played music to. </param>
        public Orchestra (IMidiOut output)
        {
            _output = output;
			_beatScheduler = new BeatScheduler(this, _output);
			_output.SetSource(_beatScheduler);
        }


        
        /// <summary>
        /// Constructs a new instrument and adds it to the orchestra.
        /// </summary>
        /// <param name="instrumentType">Enum that represents an instrument.</param>
        /// <param name="ocatave">The instruments offset from the base octave. 
        /// If this number is negative, the instrument will have a deeper sound,
        ///  and if it is positive it will have a lighter sound. 
        /// It can be between -5 and 5, but this is outside normal human hearing. </param>
        /// <returns> Returns an instrument that plays in the chromatic scale. </returns>
        public Instrument AddInstrument(InstrumentType instrumentType, int ocatave = 0)
        {
            return AddInstrument(instrumentType, Scale.ChromaticScale, ocatave);
        }
        
        
        
        /// <summary>
        /// Constructs a new instrument with a specified scale and adds it to the orchestra
        /// </summary>
        /// <param name="instrumentType">Enum that represents an instrument</param>
        /// <param name="scale">In the scale you want the music to be played</param>
        /// <param name="octave">The instruments offset from the base octave. 
        /// If this number is negative, the instrument will have a deeper sound,
        ///  and if it is positive it will have a lighter sound. 
        /// It can be between -5 and 5, but this is outside normal human hearing. </param>
        /// <returns> Returns an instrument that plays in the specified scale. </returns>
        public Instrument AddInstrument (InstrumentType instrumentType, Scale scale, int octave=3)
        {
            Instrument instrument = new Instrument(this, instrumentType, scale, octave);
            _instruments.Add(instrument);

            return instrument;
        }



        void IOrchestra.CopyToOutput(List<SingleBeat> music)
        {
            _beatScheduler.AddToQueue(music);
        }



        Internal.SimpleMidiMessage IOrchestra.NextToPlay(bool block)
	    {
		    return _beatScheduler.GetNextMidiCommand(block);
	    }

		
        
        /// <summary>
        /// Returns the current time specified by the output class.
        /// </summary>
        /// <returns>The current time </returns>
        public double CurrentTime()
        {
			double d = _output.CurrentTime();
	        return d;
        }

		
        
        /// <summary>
		/// Blocks until the underlying IMidiOut have consumed all music
		/// </summary>
	    public void WaitForFinished()
		{
			_beatScheduler.EmptyWaitHandle.WaitOne();
		}
    }
}
