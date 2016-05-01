using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MidiGremlin.Internal
{
	internal class ChannelAllocator
	{
		//Music in storage, not ready to be played yet
		private List<SingleBeatWithChannel> storage = new List<SingleBeatWithChannel>();
		//Music in storage that have begun playing but requires more events in the near future
		private Queue<SimpleMidiMessage> progressQueue = new Queue<SimpleMidiMessage>();  
		//What instrument are active on each channel
		InstrumentType[] channelInstruments = new InstrumentType[16];


		public SimpleMidiMessage GetNext()
		{
			return new SimpleMidiMessage();
		}
	}

	
}