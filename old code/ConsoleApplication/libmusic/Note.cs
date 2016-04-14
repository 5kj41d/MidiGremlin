using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midi;

namespace libmusic
{
	public abstract class LibNoteMessage
	{
		public Pitch Pitch { get;  }
		public int Velocity { get; }
		public float Time { get; }
		public int Channel { get; }
		internal MessageType Type { get; }
		internal LibNoteMessage Chain { get; set; }

		internal enum MessageType
		{
			On, Off
		}

		internal LibNoteMessage(Pitch pitch, int velocity, float time, MessageType type, int channel)
		{
			Pitch = pitch;
			Velocity = velocity;
			Time = time;
			Type = type;
			Channel = channel;
		}
	}

	public class NoteOn : LibNoteMessage
	{
		public NoteOn(Pitch pitch, int velocity, float time, int channel = 0) : base(pitch, velocity, time, MessageType.On, channel)
		{
			
		}
	}

	public class NoteOff : LibNoteMessage
	{
		public NoteOff(Pitch pitch, int velocity, float time, int channel = 0) : base(pitch, velocity, time, MessageType.Off, channel)
		{

		}
	}

	public class NoteOnOff : LibNoteMessage
	{
		public NoteOnOff(Pitch pitch, int velocity, float time, float duration, int channel = 0) : base(pitch, velocity, time, MessageType.On, channel)
		{
			Chain = new NoteOff(pitch, velocity, time + duration, channel);
		}
	}
}
