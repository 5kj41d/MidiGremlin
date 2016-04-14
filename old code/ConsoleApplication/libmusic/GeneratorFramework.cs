using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Midi;

namespace libmusic
{
	public class GeneratorFramework
	{
		#region Static
		static readonly List<Type> _knownGeneratorTypes = new List<Type>(); 
		 
		static GeneratorFramework()
		{
			KnownSimpleGenerators = _knownGeneratorTypes.AsReadOnly();

			AddAssembly(Assembly.GetExecutingAssembly());
		}

		public static void AddAssembly(Assembly assembly)
		{
			Type t = typeof (IMusicGenerator);
			_knownGeneratorTypes.AddRange(
				from type in assembly.DefinedTypes
				where t.IsAssignableFrom(type) &&
				type.GetConstructor(Type.EmptyTypes) != null && 
				!type.IsAbstract
				select type);
		}

		public static IReadOnlyList<Type> KnownSimpleGenerators { get; }
		#endregion

		private readonly object _syncRoot = new object();
		private readonly List<Tuple<IMusicGenerator, InfoObject, PrivateInfoObject>> _generators = new List<Tuple<IMusicGenerator, InfoObject, PrivateInfoObject>>();
		private readonly Clock _clock;
		private readonly OutputDevice _outputDevice;

		private Channel _channels = Channel.Channel1;

		public float BeatsPerMinute
		{
			get { return _clock.BeatsPerMinute; }
			set { _clock.BeatsPerMinute = value; }
		}

		public GeneratorFramework(float beatsPerMinute)
		{
			_clock = new Clock(beatsPerMinute);

			char c = 'a';
			Dictionary<char, OutputDevice> _outdevices = new Dictionary<char, OutputDevice>();

			Console.WriteLine("Select output device");
			foreach (OutputDevice device in OutputDevice.InstalledDevices)
			{
				Console.WriteLine($"{c} - {device.Name}");
				_outdevices.Add(c++, device);

			}

			do
			{
				c = Console.ReadKey(true).KeyChar;
			} while (!_outdevices.ContainsKey(c));

			_outputDevice = _outdevices[c];
			Console.WriteLine("Using {0} as output", _outputDevice.Name);
			_outputDevice.Open();
		}

		public void Add(Type generatorType)
		{
			if (!typeof(IMusicGenerator).IsAssignableFrom(generatorType))
			{
				throw new InvalidOperationException($"GeneratorType needs to implement {nameof(IMusicGenerator)}");
			}

			Add((IMusicGenerator)Activator.CreateInstance(generatorType));
		}

		public void Add(IMusicGenerator generator)
		{
			lock (_syncRoot)
			{
				InfoObject info = new InfoObject(this, _outputDevice, _clock);
				generator.Setup(info);
				_generators.Add(Tuple.Create(generator, info, new PrivateInfoObject()));
			}
		}

		public void Start()
		{
			lock (_syncRoot)
			{
				_clock.Schedule(new CallbackMessage(CallBack, _clock.Time));
				_clock.Start();
			}
		}

		public void Stop()
		{
			lock (_syncRoot)
			{
				_clock.Stop();
			}
		}

		private void CallBack(float time)
		{
			lock (_syncRoot)
			{
				float lastCall = _clock.Time + 0.1f;
				foreach (Tuple<IMusicGenerator, InfoObject, PrivateInfoObject> generator in _generators)
				{
					if (generator.Item3.Time < lastCall)
					{
						ICollection<LibNoteMessage> newMessages = generator.Item1.GenerateMusic();
						foreach (LibNoteMessage note in newMessages)
						{
							_clock.Schedule(MakeNote(note, generator));
							if (note.Chain != null)
							{
								_clock.Schedule(MakeNote(note.Chain, generator));
							}
						}

						generator.Item3.Time = generator.Item1.Deadline();
					}
					else
					{
						break;
					}
				}

				_generators.Sort((x, y) => x.Item3.Time.CompareTo(y.Item3.Time));

				_clock.Schedule(new CallbackMessage(CallBack, _generators[0].Item3.Time - 0.09f));
			}
		}

		private Message MakeNote(LibNoteMessage note, Tuple<IMusicGenerator, InfoObject, PrivateInfoObject> context)
		{
			if (note.Type == LibNoteMessage.MessageType.Off)
			{
				return new NoteOffMessage(_outputDevice, context.Item2.Channel, note.Pitch, note.Velocity, note.Time);
			}
			else
			{
				return new NoteOnMessage(_outputDevice, context.Item2.Channel, note.Pitch, note.Velocity, note.Time);
			}
		}

		/// <summary>
		/// Private housekeeping tired to each IMusicGenerator.
		/// </summary>
		class PrivateInfoObject
		{
			internal float Time; //Next time it should be called
		}

		internal Channel GetWorkingChannel(InfoObject info, Instrument instrument, bool single)
		{
			if (_channels == Channel.Channel16) throw new OutOfChannelsException();

			lock (_syncRoot)
			{
				_outputDevice.SendProgramChange(_channels, instrument);
			}
			
			return _channels++;
		}
	}

	internal class OutOfChannelsException : Exception
	{
	}
}
