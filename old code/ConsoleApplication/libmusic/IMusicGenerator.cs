using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midi;

namespace libmusic
{
	public interface IMusicGenerator
	{
		/// <summary>
		/// Setup method called by framework while also passing needed info in an object.
		/// 
		/// If Setup gets called multiple times it is an error and the IMusicGenerator may 
		/// throw an Exception or fail to function correctly
		/// </summary>
		/// <param name="infoObject">An object to interact with the framework</param>
		void Setup(InfoObject infoObject);

		/// <summary>
		/// Deadline for next call to generatemusic. 
		/// </summary>
		/// <returns>Last time to call GenerateMusic</returns>
		float Deadline();

		/// <summary>
		/// Generates Music
		/// </summary>
		/// <returns>A collection of music notes</returns>
		ICollection<LibNoteMessage> GenerateMusic();
	}
}
