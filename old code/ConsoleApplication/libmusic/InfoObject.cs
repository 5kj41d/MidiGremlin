using System;
using Midi;

namespace libmusic
{
	public class InfoObject
	{
		private Channel _channel = (Channel)100;
		private readonly GeneratorFramework _generatorFramework;
		
		internal InfoObject(GeneratorFramework generatorFramework, OutputDevice device, Clock clock)
		{
			_generatorFramework = generatorFramework;
		}

		public Channel Channel
		{
			get
			{
				if (_channel == (Channel)100) //100 used as bogeous value, *should* never leave class
				{
					_channel = GetWorkingChannel(Instrument.AcousticGrandPiano);
				}

				return _channel;
			}
		}

		public Channel GetWorkingChannel(Instrument instrument, bool single = false)
		{
			Channel c = _generatorFramework.GetWorkingChannel(this, instrument, single);

			if (_channel == (Midi.Channel) 100)
			{
				_channel = c;
			}

			return c;
		}
	}
}