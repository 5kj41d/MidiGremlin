namespace MidiGremlin
{
	public static class InstrumentTypeExtensions
	{
		public static bool IsDrum(this InstrumentType instrumentType)
		{
			return ((int)instrumentType & 0x7fffff80) == 0x100;
		}
	}
}