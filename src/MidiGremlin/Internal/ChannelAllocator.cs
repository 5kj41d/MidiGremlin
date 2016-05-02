using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace MidiGremlin.Internal
{
	internal class ChannelAllocator
	{
		//Music in storage, not ready to be played yet. A queue with last to exit first in index to make modification to end fast
		private readonly List<SingleBeatWithChannel> _storage = new List<SingleBeatWithChannel>();
		//Music in storage that have begun playing but requires more events in the near future
		private readonly List<SimpleMidiMessage> _progressQueue = new List<SimpleMidiMessage>();  
		//What instrument are active on each channel
		readonly InstrumentType[] _channelInstruments = new InstrumentType[16];

		public SimpleMidiMessage GetNext()
		{
			if (_storage.Count == 0 && _progressQueue.Count == 0)
			{
				return new SimpleMidiMessage();  //TODO: Can be done better? Throw OutOfStuffException?
			}

			if (_storage.Count == 0 || _progressQueue[_progressQueue.Count - 1].Timestamp < _storage[_storage.Count - 1].ToneStartTime)
			{
				SimpleMidiMessage message = _progressQueue[_progressQueue.Count - 1];
				_progressQueue.RemoveAt(_progressQueue.Count - 1);
				return message;
			}
			else
			{
				SingleBeatWithChannel beatWithChannel = _storage[_storage.Count - 1];
				_storage.RemoveAt(_storage.Count - 1);

				if (beatWithChannel.instrumentType == _channelInstruments[beatWithChannel.Channel])
				{
					//TODO: sort
					_progressQueue.Add(new SimpleMidiMessage(MakeMidiEvent(0x8, beatWithChannel.Channel, beatWithChannel.Tone, beatWithChannel.ToneVelocity), beatWithChannel.ToneEndTime));
					return new SimpleMidiMessage(MakeMidiEvent(0x9, beatWithChannel.Channel, beatWithChannel.Tone, beatWithChannel.ToneVelocity), beatWithChannel.ToneStartTime);
				}
				else
				{
					//TODO: Store both start play and stop play in progressQueue, sort it
					return new SimpleMidiMessage(MakeMidiEvent(0xC, beatWithChannel.Channel, (byte)((int)beatWithChannel.instrumentType & 0x7F), 0), beatWithChannel.ToneStartTime);  //Emit the change channel message
				}
			}
		}

		public void Add(List<SingleBeat> input)
		{
			InstrumentType[] lastUsedInstruments = new InstrumentType[16];
			Array.Copy(_channelInstruments, lastUsedInstruments, 16);

			double[] finishTimes = CreateFinishTimesArray();
			int storageProgress = 0;

			foreach (SingleBeat singleBeat in input)
			{
				//Find the element in _storage that comes just before us
				while (_storage.Count > storageProgress && _storage[storageProgress].ToneStartTime > singleBeat.ToneStartTime)
				{
					int channel = _storage[storageProgress].Channel;
					finishTimes[channel] = Math.Max(_storage[storageProgress].ToneEndTime, finishTimes[channel]);
					lastUsedInstruments[channel] = _storage[storageProgress].instrumentType;
					storageProgress++;
				}

				int usedChannel = lastUsedInstruments.IndexOf(singleBeat.instrumentType);
				if (usedChannel == -1)  //If no free channel is found, find one or die
				{
					int maybeFreeChannel = finishTimes.IndexOfSmallest();
					if (finishTimes[maybeFreeChannel] > singleBeat.ToneStartTime)
					{
						//won't be free, do whatever
						throw new OutOfChannelsException();
					}
					else
					{
						usedChannel = maybeFreeChannel;
						lastUsedInstruments[usedChannel] = singleBeat.instrumentType;
					}
				}
				
				finishTimes[usedChannel] = Math.Max(singleBeat.ToneEndTime, finishTimes[usedChannel]);
				_storage.Insert(storageProgress, singleBeat.WithChannel((byte)usedChannel));
			}
		}

		public double NextTimeStamp
		{
			get
			{
				if (_storage.Count == 0 && _progressQueue.Count == 0)
				{
					return double.MaxValue;
				}

				if (_storage.Count == 0)
				{
					return _progressQueue[_progressQueue.Count - 1].Timestamp;
				}

				if (_progressQueue.Count == 0)
				{
					return _storage[_storage.Count - 1].ToneStartTime;
				}
				return Math.Min(_storage[_storage.Count - 1].ToneStartTime, _progressQueue[_progressQueue.Count - 1].Timestamp);
			}
		}

		private double[] CreateFinishTimesArray()
		{
			double[] ret = new double[16];

			foreach (SimpleMidiMessage message in _progressQueue)
			{
				ret[message.Channel] = Math.Max(ret[message.Channel], message.Timestamp);
			}

			return ret;
		}

		private int MakeMidiEvent(byte midiEventType, byte channel, byte tone, byte toneVelocity)
		{
			int data = 0;

			data |= channel << 0;
			data |= midiEventType << 4;
			data |= tone << 8;
			data |= toneVelocity << 16;

			return data;
		}
	}

	internal class OutOfChannelsException : Exception
	{
	}
}