using System;
using System.Runtime.InteropServices;

namespace MidiGremlin.Internal
{
    /// <summary>
    /// Internal data type that represents a single musical tune with a beginning and an end.
    /// Contains information of a single tone to play in a format not entirely different from that of the MIDI standard.
    /// </summary>
    public struct SingleBeat
    {
	    /// <summary>
	    /// Returns the fully qualified type name of this instance.
	    /// </summary>
	    /// <returns>
	    /// A <see cref="T:System.String"/> containing a fully qualified type name.
	    /// </returns>
	    public override string ToString()
	    {
		    return $"{ToneStartTime.ToString().PadRight(7)} -> {ToneEndTime.ToString().PadRight(7)} ({Tone}, {ToneVelocity}) ({instrumentType})";
	    }

	    /// <summary>
        /// Creates a new instance of SingleBeat with the specified properties.
        /// </summary>
        /// <param name="instrumentType">The type of instrument that should be playing the music</param>
        /// <param name="tone"> MIDI note number. See this chart:http://www.tonalsoft.com/pub/news/pitch-bend.aspx </param>
        /// <param name="toneVelocity">Velocity of the note. </param>
        /// <param name="toneStartTime">Start time of the note.</param>
        /// <param name="toneEndTime">End of the note.</param>
        public SingleBeat(InstrumentType instrumentType, byte tone, byte toneVelocity, double toneStartTime, double toneEndTime)
        {
            this.instrumentType = instrumentType;
            Tone = tone;
            ToneVelocity = toneVelocity;
            ToneStartTime = toneStartTime;
            ToneEndTime = toneEndTime;
        }

		/// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false. 
        /// </returns>
        /// <param name="obj">The object to compare with the current instance. </param><filterpriority>2</filterpriority>
        public override bool Equals (object obj)
        {
            if (obj is SingleBeat)
            {
                SingleBeat other = (SingleBeat) obj;

                return
                    other.instrumentType == instrumentType &&
                    other.Tone == Tone &&
                    other.ToneVelocity == ToneVelocity &&
                    Math.Abs(other.ToneStartTime - ToneStartTime) < 0.000001 &&
                    Math.Abs(other.ToneEndTime - ToneEndTime) < 0.000001;
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode ()
        {
            unchecked
            {
                var hashCode = (int) instrumentType;
                hashCode = (hashCode * 397) ^ Tone.GetHashCode();
                hashCode = (hashCode * 397) ^ ToneVelocity.GetHashCode();
                hashCode = (hashCode * 397) ^ ToneStartTime.GetHashCode();
                hashCode = (hashCode * 397) ^ ToneEndTime.GetHashCode();
                return hashCode;
            }
        }
		
        
		/// <summary> The type of instrument the tone is played with.  </summary>
        public InstrumentType instrumentType{ get; }
        
		
		/// <summary> Represents the tone as it is saved in the MIDI standard. http://www.tonalsoft.com/pub/news/pitch-bend.aspx  </summary>
        public byte Tone{ get; }
        
		
		/// <summary> The velocity of the tone. </summary>
        public byte ToneVelocity{ get; }
        
		
		/// <summary> The time in beats at which the tone starts. </summary>
        public double ToneStartTime{ get; }
        
		
		/// <summary> The time in beats at which the tone end. </summary>
        public double ToneEndTime{ get; }


		/// <summary>
		/// Returns a <see cref="SingleBeatWithChannel"/> that is based on this <see cref="SingleBeat"/> using <paramref name="usedChannel"/> as channel.
		/// </summary>
		/// <param name="usedChannel">A MIDI channel to use.</param>
		/// <returns>A new <see cref="SingleBeatWithChannel"/> constructed using <paramref name="usedChannel"/> and this</returns>
	    public SingleBeatWithChannel WithChannel(byte usedChannel)
	    {
		    return new SingleBeatWithChannel(instrumentType, Tone, ToneVelocity, ToneStartTime, ToneEndTime, usedChannel);
	    }
    }
    /// <summary>
    /// A Singlebeat that also contains a channel.
    /// </summary>
    public struct SingleBeatWithChannel
    {
        /// <summary>
        /// Creates a new instance of SingleBeat with the specified properties.
        /// </summary>
        /// <param name="instrumentType">The type of instrument that should be playing the music</param>
        /// <param name="tone"> MIDI note number. See this chart:http://www.tonalsoft.com/pub/news/pitch-bend.aspx </param>
        /// <param name="toneVelocity">Velocity of the note. </param>
        /// <param name="toneStartTime">Start time of the note.</param>
        /// <param name="toneEndTime">End of the note.</param>
        /// <param name="channel">The channel to play from.</param>
        public SingleBeatWithChannel (InstrumentType instrumentType, byte tone, byte toneVelocity, double toneStartTime, double toneEndTime, byte channel)
        {
            this.InstrumentType = instrumentType;
            Tone = tone;
            ToneVelocity = toneVelocity;
            ToneStartTime = toneStartTime;
            ToneEndTime = toneEndTime;
            Channel = channel;
        }

	    
		/// <summary>Returns the fully qualified type name of this instance.</summary>
	    /// <returns>A <see cref="T:System.String" /> containing a fully qualified type name.</returns>
	    public override string ToString()
		{ 
			return $"{ToneStartTime.ToString().PadRight(7)} -> {ToneEndTime.ToString().PadRight(7)} ({Tone}, {ToneVelocity}) ({Channel}) ({InstrumentType})";
		}

		/// <summary> The type of instrument the tone is played with.  </summary>
		public InstrumentType InstrumentType { get; }
        
		
		/// <summary> Represents the tone as it is saved in the MIDI standard. http://www.tonalsoft.com/pub/news/pitch-bend.aspx  </summary>
        public byte Tone { get; }
        
		
		/// <summary> The velocity of the tone. </summary>
        public byte ToneVelocity { get; }
        
		
		/// <summary> The time in beats at which the tone starts. </summary>
        public double ToneStartTime { get; }
        
		
		/// <summary> The time in beats at which the tone end. </summary>
        public double ToneEndTime { get; }
        
		
		/// <summary> The channel to play from. </summary>
        public  byte Channel { get; }
    }
}