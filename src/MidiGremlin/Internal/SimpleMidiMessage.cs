using System;
using System.Diagnostics;

namespace MidiGremlin.Internal
{
	public struct SimpleMidiMessage
	{
		public readonly double Timestamp;
		public readonly int Data;

		public SimpleMidiMessage(int data, double timestamp)
		{
			//if((data & 0x808000) != 0) throw new ArgumentOutOfRangeException();
			//if ((data & 0x000080) == 0) throw new ArgumentOutOfRangeException();
			this.Data = data;
			this.Timestamp = timestamp;
		}

		public SimpleMidiMessage WithChannel(byte channel)
		{
			if(channel > 15 ) throw new ArgumentOutOfRangeException(nameof(channel));
			
			int data = Data;
			data &= 0x00fffff0;
			data |= channel;
			return new SimpleMidiMessage(data, this.Timestamp);
		}
		
		public byte Type
		{
			get { return (byte) ((Data & 0x000000f0) >> 4); }
		}
		public byte Channel
		{
			get { return (byte) (Data & 0x0000000f); }
		}
		public byte Pitch
		{
			get { return (byte) ((Data & 0x0000ff00) >> 8); }
		}
		public byte Velocity
        {
			get { return (byte) ((Data & 0x00ff0000) >> 16); }
		}

		public bool Equals(SimpleMidiMessage other)
		{
			return Timestamp == other.Timestamp && Data == other.Data;
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return (Timestamp.GetHashCode() * 397) ^ Data;
			}
		}

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <returns>
		/// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false. 
		/// </returns>
		/// <param name="obj">The object to compare with the current instance. </param>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is SimpleMidiMessage && Equals((SimpleMidiMessage) obj);
		}
		
		public static bool operator ==(SimpleMidiMessage lhs, SimpleMidiMessage rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(SimpleMidiMessage lhs, SimpleMidiMessage rhs)
		{
			return !lhs.Equals(rhs);
		}

		private static string[] types =
		{
			"  0",
			"  1",
			"  2",
			"  3",
			"  4",
			"  5",
			"  6",
			"  7",
			"Off",
			" On",
			" 10",
			" 11",
			" Ch",
			" 13",
			" 14",
			" 15"
		};

		/// <summary>
		/// Returns the fully qualified type name of this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> containing a fully qualified type name.
		/// </returns>
		public override string ToString()
		{
			int type =     (Data & 0x000000f0) >> 4;
			int channel =  (Data & 0x0000000f) + 1;
			int pitch =    (Data & 0x0000ff00) >> 8;
			int velocity = (Data & 0x00ff0000) >> 16;

			return $"{Timestamp.ToString("F")} {types[type]} Channel:{channel.ToString().PadRight(2)} V:{velocity} P:{pitch}";
		}
	}
}