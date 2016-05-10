using System;
using System.Diagnostics;

namespace MidiGremlin.Internal
{
	/// <summary>
	/// Represents a simple MIDI message and a timestamp. 
	/// </summary>
	public struct SimpleMidiMessage
	{
		/// <summary>
		/// The timestramp, in beats since the start of the play, that this <see cref="SimpleMidiMessage"/> is supposed to be executed.
		/// </summary>
		public readonly double Timestamp;

		/// <summary>
		/// The MIDI data that is supposed to be executed. To perserve compability with the WinMM API, this is stored with the least signifigant byte being the first to be played.
		/// </summary>
		public readonly int Data;

		
		
		/// <summary>
		/// Constructs a new <see cref="SimpleMidiMessage"/> with the following data and timestramp.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="timestamp"></param>
		public SimpleMidiMessage(int data, double timestamp)
		{
			//if((data & 0x808000) != 0) throw new ArgumentOutOfRangeException();
			//if ((data & 0x000080) == 0) throw new ArgumentOutOfRangeException();
			this.Data = data;
			this.Timestamp = timestamp;
		}

		
		
		/// <summary>
		/// Returns a copy of this <see cref="SimpleMidiMessage"/> with the channel changed to channel.
		/// </summary>
		/// <param name="channel">The channel to use in the copy of this message.</param>
		/// <returns>A copy of this <see cref="SimpleMidiMessage"/> with the channel changed.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If channel is above 15, outside the range of MIDI channels.</exception>
		public SimpleMidiMessage WithChannel(byte channel)
		{
			if(channel > 15 ) throw new ArgumentOutOfRangeException(nameof(channel));
			
			int data = Data;
			data &= 0x00fffff0;
			data |= channel;
			return new SimpleMidiMessage(data, this.Timestamp);
		}

		
		
		/// <summary>
		/// The MIDI event type this <see cref="SimpleMidiMessage"/> represents. 
		/// The upper 4 bits of this property will always be zero.
		/// For more info on what this property means <see href="https://www.midi.org/specifications/item/table-1-summary-of-midi-message">here</see>. 
		/// </summary>
		public byte Type
		{
			get { return (byte) ((Data & 0x000000f0) >> 4); }
		}

		
		
		/// <summary>
		/// The channel specified in this <see cref="SimpleMidiMessage"/>. 
		/// The upper 4 bits of this property will always be zero.
		/// </summary>
		public byte Channel
		{
			get { return (byte) (Data & 0x0000000f); }
		}

		
		
		/// <summary>
		/// The pitch part of this <see cref="SimpleMidiMessage"/>. 
		/// If the <see cref="Type"/> is not Note On (0x9) or Note Off(0x8) this property won't contain the Pitch of the inner MIDI message but the 
		/// related field at the same offset.
		/// </summary>
		public byte Pitch
		{
			get { return (byte) ((Data & 0x0000ff00) >> 8); }
		}

		
		/// <summary>
		/// The velocity of this <see cref="SimpleMidiMessage"/>. 
		/// If the <see cref="Type"/> is not Note On (0x9) or Note Off(0x8) this property won't contain the Veloctiy of the inner MIDI message but the 
		/// related field at the same offset or blank if it is a 2 byte message.
		/// </summary>
		public byte Velocity
        {
			get { return (byte) ((Data & 0x00ff0000) >> 16); }
		}



		/// <summary>
		/// Determines whether this instance and another specified <see cref="SimpleMidiMessage"/> object have the same value.
		/// </summary>
		/// <param name="other">The <see cref="SimpleMidiMessage"/> to compare with the current instance. </param>
		/// <returns>
		/// true if <paramref name="other"/> and this instance represent the same value; otherwise, false. 
		/// </returns>
		public bool Equals(SimpleMidiMessage other)
		{
			return Timestamp.Equals(other.Timestamp) && Data == other.Data;
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
			return obj is SimpleMidiMessage && Equals((SimpleMidiMessage)obj);
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
		/// Compares the equality of two <see cref="SimpleMidiMessage"/>.
		/// </summary>
		/// <param name="lhs">The left hand side of the comparison.</param>
		/// <param name="rhs">The right hand side of the comparison.</param>
		/// <returns>True if <paramref name="lhs"/> and <paramref name="rhs"/> are equal; Otherwise false.</returns>
		public static bool operator ==(SimpleMidiMessage lhs, SimpleMidiMessage rhs)
		{
			return lhs.Equals(rhs);
		}

		/// <summary>
		/// Compares the inequality of two <see cref="SimpleMidiMessage"/>.
		/// </summary>
		/// <param name="lhs">The left hand side of the comparison.</param>
		/// <param name="rhs">The right hand side of the comparison.</param>
		/// <returns>True if <paramref name="lhs"/> and <paramref name="rhs"/> are unequal; Otherwise false.</returns>
		public static bool operator !=(SimpleMidiMessage lhs, SimpleMidiMessage rhs)
		{
			return !lhs.Equals(rhs);
		}

		private static readonly string[] Types =
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

			return $"{Timestamp.ToString("F")} {Types[type]} Channel:{channel.ToString().PadRight(2)} V:{velocity} P:{pitch}";
		}
	}
}