using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiGremlin.Internal
{
    class VariableBpmCounter
    {
        /// <summary> IThe amount of beats that have passed until last time bpm was changed. </summary>
        private double _oldTime = 0;
        private Stopwatch _time;

        public VariableBpmCounter()
        {
			_time = new Stopwatch();
        }

        private int _beatsPerMinute;

        /// <summary> How many beats corresponds to 60 seconds. If the value is set to 60, 1 beat will be the same as 1 second. </summary>
        public int BeatsPerMinute
        {
            get { return _beatsPerMinute; }
            //Saves the amount of beats that have passed so far and resets the timer before changing bpm.
            //Total amount of beats that have passed is 
            // value of the current timer(CurrentTimeScale) 
            // plus the time that passed before reset(_oldTime).
            set
            {
                if (_beatsPerMinute != value)
                {
                    _oldTime += CurrentScaleTime();
                    _time.Restart();
                    _beatsPerMinute = value;
                }
            }
        }



        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            _time.Start();
        }


        /// <summary> Conversion constant between minutes and milliseconds. </summary>
        private static double _minutesToMilliseconds = 60000;

        /// <summary>
        /// The duration of 1 beat in milliseconds.
        /// </summary>
        /// <returns>The duration of 1 beat in milliseconds.</returns>
        public double BeatDuratinInMilliseconds
        {
            get
            {
                double durationInMinutes = 1.0 / BeatsPerMinute;
                double durationInMilliseconds = durationInMinutes * _minutesToMilliseconds;
                return durationInMilliseconds;
            }
        }



        /// <summary>
        /// The amount of beats that have passed since this class was instantiated.
        /// </summary>
        /// <returns>The amount of beats that have passed since this class was instantiated.</returns>
        public double CurrentTime
        {
            get
            {
                return _oldTime + CurrentScaleTime();
            }
        }

        private double CurrentScaleTime ()
        {
            return _time.Elapsed.TotalMilliseconds / BeatDuratinInMilliseconds;
        }
    }
}
