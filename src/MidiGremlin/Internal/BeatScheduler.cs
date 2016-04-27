using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MidiGremlin.Internal
{
	/// <summary>
	/// The BeatScheduler is responsible for scheduling MIDI events to the 16 MIDI channels and the handling of 
	/// the acctual timing of when to play 
	/// </summary>
	public class BeatScheduler
	{
		private /*const*/ static readonly SimpleMidiMessage EmptyMessage = new SimpleMidiMessage(0,0);

		//TODO: Make this a real concurrent priority queue. Well, this works, but muh performance
		//Last [Count - 1] is first as it is cheaper to remove / insert there
		private readonly List<SimpleMidiMessage> _priorityQueue = new List<SimpleMidiMessage>(); 

		private readonly object _syncRoot = new object();
		private readonly EventWaitHandle _newDataAdded = new AutoResetEvent(false);
		private readonly Orchestra _orchestra;
		private readonly IMidiOut _output;

		internal BeatScheduler(Orchestra orchestra, IMidiOut output)
		{
			this._orchestra = orchestra;
			_output = output;
		}


		/// <summary>
		/// Returns the next simple midi event to be played from the queue. 
		/// Can either block until it is time to actually play it or return the current next imediatly
		/// </summary>
		/// <param name="block">Force the BeatScheduler to return imediatly</param>
		/// <returns>A simple MIDI event and a timestamp. If nothing exists in the queue, it either 
		/// returns an empty message or blocks until one is available</returns>
		public SimpleMidiMessage GetNextMidiCommand(bool block = true)
		{
			//Try to find a new tone 10 times. Should only run once in most conditions, but
			//we need recycle our wait if the queue gets new things added while we wait. 
			//could be while(true) but this way we don't risk an infinite loop
			//which is bad.
			for (int i = 0; i < 10; i++)
			{
				SimpleMidiMessage message;
				lock (_syncRoot)
				{
					if (_priorityQueue.Count != 0)
					{
						message = _priorityQueue[_priorityQueue.Count - 1];
					}
					else
					{
						message = EmptyMessage;
					}
				}

				if (message == EmptyMessage)
				{
					if (block)
					{
						_newDataAdded.WaitOne();
						continue;
					}
					else
					{
						return EmptyMessage;
					}
				}

				//If we was not interupted we assume we arrived at time
				if (!block || !_newDataAdded.WaitOne(GetWaitTimeMs(message)))
				{
					return message;
				}
			}


			//If working in debug mode knowing that something went wrong is probably perfeable
			//but in release we would just as well like it failing silently
			//This being only a single MIDI event it would not impact the overall piece overmuch
#if DEBUG
			throw new TimeoutException("Unable to return anything after too many iterupts. This should never happen");
#else
			return new SimpleMidiMessage(0,0);
#endif
		}

		private int GetWaitTimeMs(SimpleMidiMessage message)
		{
			double current = _orchestra.CurrentTime();
			double remaining = message.Timestamp - current;

			return remaining < 0 ? 0 : BeatsToMs(remaining);
		}

		private int BeatsToMs(double remaining)
		{
			double msPerBeat = (60.0 / _output.BeatsPerMinute) * 1000;

			return (int) (remaining*msPerBeat);
		}


		internal void AddToQueue(IEnumerable<SingleBeat> beats)
		{
			lock (_syncRoot)
			{

				var allTheBeats = beats.ToList();
				var v = allTheBeats.SelectMany(TransformFunction).ToList();
				_priorityQueue.AddRange(v);
				_priorityQueue.Sort((lhs, rhs) => (int) (rhs.Timestamp - lhs.Timestamp));   //Beat to play next is the last in the list and so on.

				_newDataAdded.Set();
			}
		}

		private IEnumerable<SimpleMidiMessage> TransformFunction(SingleBeat arg)
		{
			yield return new SimpleMidiMessage(
				MakeMidiEvent(0x9, 0, arg.ToneOffset, arg.ToneVelocity), arg.ToneStartTime);  //Key down.

			yield return new SimpleMidiMessage(
				MakeMidiEvent(0x8, 0, arg.ToneOffset, arg.ToneVelocity), arg.ToneEndTime); //Key up.
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
}
