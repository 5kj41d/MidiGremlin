using System;
using System.Collections.Generic;
using System.Threading;

namespace MidiGremlin.Internal
{
    /// <summary>
    /// The BeatScheduler is responsible for scheduling MIDI events to the 16 MIDI channels 
    /// and the handling of the actual timing of when to play 
    /// </summary>
    public class BeatScheduler
	{
		private /*const*/ static readonly SimpleMidiMessage EmptyMessage = new SimpleMidiMessage(0,0);
		
		private readonly object _syncRoot = new object();
		private readonly EventWaitHandle _newDataAdded = new AutoResetEvent(false);
		private readonly EventWaitHandle _emptyHandle = new ManualResetEvent(false);
		private readonly Orchestra _orchestra;
		private readonly IMidiOut _output;
		private readonly ChannelAllocator _channelAllocator = new ChannelAllocator();



		internal BeatScheduler(Orchestra orchestra, IMidiOut output)
		{
			this._orchestra = orchestra;
			_output = output;
		}
		


		/// <summary>
		/// Returns the next simple midi event to be played from the queue. 
		/// It can either block it until it is time to actually play it, or return the current timestamp immediately.
		/// </summary>
		/// <param name="block">Force the BeatScheduler to return immediately</param>
		/// <returns>A simple MIDI event and a timestamp. If nothing exists in the queue, it either 
		/// returns an empty message or blocks until one is available.</returns>
		public SimpleMidiMessage GetNextMidiCommand(bool block = true)
		{
			//Try to find a new tone 10 times. Should only run once in most conditions, but
			//we need recycle our wait if the queue gets new things added while we wait. 
			//could be while(true) but this way we don't risk an infinite loop
			//which is bad.
			//Probably also possible to do a simpler reset but cannot quite see how
			for (int i = 0; i < 10; i++)
			{
				double nextTime;
				lock (_syncRoot)
				{
					nextTime = _channelAllocator.NextTimeStamp;
				}

				if (nextTime == ChannelAllocator.NoMessageTime)
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



				//If we were not interrupted, we assume we arrived in time
				//Console.Write($"{!block} || {GetWaitTimeMs(_channelAllocator.NextTimeStamp):D4}  ~  ");

                //If not set to block or if no new data arrives
			    if (!block || !_newDataAdded.WaitOne(GetWaitTimeMs(_channelAllocator.NextTimeStamp)))
			    {
			        SimpleMidiMessage message = _channelAllocator.GetNext();
			        //Console.WriteLine($"Fin {message}");

			        if (_channelAllocator.Empty)
			        {
			            //If the emptyHandle is not set, notify that we have finished everything
			            if (!_emptyHandle.WaitOne(0))
			            {
			                _emptyHandle.Set();
			            }
			        }

			        return message;
			    }
			}

			//If working in debug mode knowing that something went wrong is probably preferable
			//but in release we would just as well like it failing silently
			//This being only a single MIDI event it would not impact the overall piece overmuch
#if DEBUG
			throw new TimeoutException("Unable to return anything after too many iterupts. This should (probably) never happen");
#else
			return new SimpleMidiMessage(0,0);
#endif
		}



		/// <summary>
		/// A wait handle that is set once the underlying collection is empty. It will remain set until new content is added to the queue.
		/// </summary>
		public EventWaitHandle EmptyWaitHandle => _emptyHandle;



		internal void AddToQueue(List<SingleBeat> beats)
		{
			lock (_syncRoot)
			{
				if (_emptyHandle.WaitOne(0))
				{
					_emptyHandle.Reset();
				}
				
				_channelAllocator.Add(beats);

				_newDataAdded.Set();
			}
		}



		private int GetWaitTimeMs(double timestamp)
		{
			double current = _orchestra.CurrentTime();
			double remaining = timestamp - current;

			return remaining < 0 ? 0 : BeatsToMs(remaining);
		}



		private int BeatsToMs(double remaining)
		{
			double msPerBeat = (60.0 / _output.BeatsPerMinute) * 1000;

			return (int) (remaining*msPerBeat);
		}
	}
}
