using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libmusic;
using Midi;

namespace ConsoleApplication.Generators
{
    class ElementaryCellularAutomata : IMusicGenerator
    {
        public int rule = 45;
        
        int velocity = 64;
        float beat = 0;

        bool[] _rules;
        bool[] _currentState;
        bool[] _previousState;

        Pitch[] toneScale;
        bool[] initialState = new bool[Console.BufferWidth-1];

        public ElementaryCellularAutomata ()
        {

            if (!(0 <= rule && rule <= 255))
                throw new ArgumentException("Choose rule between 0 and 255.");
            
            //Read the rules
            _rules = new bool[8];
            for (int i = 0; i < 8; i++)
            {
                _rules[i] = (0 != (rule & (int) Math.Pow(2, i)));
            }

            //Set the toneScale
            toneScale = new Pitch[] { Pitch.C3, Pitch.D3, Pitch.E3, Pitch.F3, Pitch.A4, Pitch.B4 };

            //Random initial state
            Random random = new Random();
            for (int i = 0; i < initialState.Length; i++)
            {
                bool state = random.Next(0, 2) == 1 ? true : false;
                initialState[i] = state;
            }
            _previousState = new bool[initialState.Length];
            initialState.CopyTo(_previousState, 0);
            _currentState = new bool[_previousState.Length];
        }


        public void Step ()
        {
            for (int i = 0; i < _currentState.Length; i++)
            {
                if (_rules[_findRuleID(i)] == true)
                    _currentState[i] = true;
                else
                    _currentState[i] = false;
            }

            _currentState.CopyTo(_previousState, 0);
        }


        int _findRuleID (int index)
        {
            int ruleID = 0;
            int c1, c2, c3;
            if (0 < index && index < (_previousState.Length - 1))
            {
                c1 = _previousState[index - 1] ? 1 : 0;
                c2 = _previousState[index] ? 1 : 0;
                c3 = _previousState[index + 1] ? 1 : 0;
            } else if (index == 0)
            {
                c1 = _previousState.Last() ? 1 : 0;
                c2 = _previousState[index] ? 1 : 0;
                c3 = _previousState[index + 1] ? 1 : 0;
            } else /* if this is the last cell */
            {
                c1 = _previousState[index - 1] ? 1 : 0;
                c2 = _previousState[index] ? 1 : 0;
                c3 = _previousState.First() ? 1 : 0;
            }

            ruleID = (c1 << 2) + (c2 << 1) + (c3 << 0);

            return ruleID;
        }


        public void Print ()
        {
            _print(_currentState);
            Console.WriteLine();
        }


        public void _print (bool[] toPrint, char fill = '▒', char blank = ' ', ConsoleColor color = ConsoleColor.Green)
        {
            ConsoleColor previousColor = Console.ForegroundColor;
            ConsoleColor cellColor = color;

            Console.ForegroundColor = cellColor;
            for (int i = 0; i < toPrint.Length; i++)
            {
                Console.Write(toPrint[i] ? fill : blank);
            }

            Console.ForegroundColor = previousColor;
        }


        public List<LibNoteMessage> PrintAndPlay ()
        {
            int nodesStart = (_currentState.Length / 2) - (toneScale.Length / 2) - (toneScale.Length % 2);
            bool[] firstBit = new bool[nodesStart + (toneScale.Length % 2)];
            bool[] middleBit = new bool[toneScale.Length];
            bool[] lastBit = new bool[nodesStart];

            List<Pitch> toPlay = new List<Pitch>();
            List<Pitch> toEnd = new List<Pitch>();

            List<LibNoteMessage> toSend = new List<LibNoteMessage>();

            Array.Copy(_currentState, 0, firstBit, 0, firstBit.Length);
            Array.Copy(_currentState, firstBit.Length, middleBit, 0, middleBit.Length);
            Array.Copy(_currentState, firstBit.Length + middleBit.Length, lastBit, 0, lastBit.Length);

            Console.BackgroundColor = ConsoleColor.White;
            _print(firstBit, '█', ' ', ConsoleColor.Black);
            _print(middleBit, '█', ' ', ConsoleColor.Blue);
            _print(lastBit, '█', ' ', ConsoleColor.Black);
            Console.BackgroundColor = ConsoleColor.Black;

            for (int i = 0; i < middleBit.Length; i++)
            {
                if (middleBit[i] == true)
                    toPlay.Add(toneScale[i]);
                else
                    toEnd.Add(toneScale[i]);
            }

            foreach (Pitch tone in toEnd)
            {
                toSend.Add(new NoteOff(tone, velocity, beat));
            }
            foreach (Pitch tone in toPlay)
            {
                toSend.Add(new NoteOn(tone, velocity, beat));
            }
            Console.WriteLine();

            beat++;

            return toSend;
        }

        public void Setup (InfoObject infoObject)
        {

        }

        public float Deadline ()
        {
            return beat;
        }

        public ICollection<LibNoteMessage> GenerateMusic ()
        {
            Step();
            return PrintAndPlay();
        }
    }
}
