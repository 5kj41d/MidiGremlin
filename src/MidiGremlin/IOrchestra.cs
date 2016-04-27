using System.Collections.Generic;
using MidiGremlin.Internal;

namespace MidiGremlin
{
	internal interface IOrchestra
	{
		Internal.SimpleMidiMessage NextToPlay(bool block = true);
		void CopyToOutput(List<SingleBeat> music);
		double CurrentTime();
	}
}