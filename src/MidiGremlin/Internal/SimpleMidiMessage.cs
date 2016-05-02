using System.Diagnostics;

namespace MidiGremlin.Internal
{
	public struct SimpleMidiMessage
	{
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

		public readonly double Timestamp;
		[DebuggerDisplay("{Data,h}")]
		public readonly int Data;

		public SimpleMidiMessage(int data, double timestamp)
		{
			this.Data = data;
			this.Timestamp = timestamp;
		}

		public static bool operator ==(SimpleMidiMessage lhs, SimpleMidiMessage rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(SimpleMidiMessage lhs, SimpleMidiMessage rhs)
		{
			return !lhs.Equals(rhs);
		}
	}
}