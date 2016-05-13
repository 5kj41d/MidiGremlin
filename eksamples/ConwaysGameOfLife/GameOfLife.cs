using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MidiGremlin;

namespace ConwaysGameOfLife
{

    internal class GameOfLife
    {
        //MIDI Gremlin Variables
        private Orchestra _o;
        private Instrument _instrument;
        // A scale representing tones that harmonize well with eachother
        private Scale _quintScale = new Scale(Tone.C, Tone.G, Tone.D, Tone.A, Tone.E, Tone.B, Tone.FSharp);

        // Class-specific valuables
        public bool[,] Cells { get; private set; }
        private bool[,] _cellsTemporaryState;
        private readonly int _xValue;
        private readonly int _yValue;
        private Timer _timer;
        internal bool IsDrawing;
        private bool _isPlaying;
        private int _musicCounter = 0;


        internal GameOfLife(bool[,] gameGrid)
        {
            // This represents each generation of the cells.
            Cells = gameGrid;
            // This represents the stage in between each generation.
            _cellsTemporaryState = new bool[gameGrid.GetLength(0), gameGrid.GetLength(1)];

            // These represent the max values of the x an y values in the grid.
            _xValue = gameGrid.GetLength(0);
            _yValue = gameGrid.GetLength(1);

            // This timer represents how often the update method is called.
            _timer = new Timer(UpdateMethod, null, TimeSpan.Zero, TimeSpan.FromSeconds(0.75));
        }

        
        // This method initiates the music components.
        internal void MusicSetup()
        {
            // bpm is a constant deciding the tempo of the music.
            // when bpm is set to 60, one beat will last 1 second. 120 for half a second and so forth.
            const int bpm = 60;
            // Here we create a new orchestra using the output type WinmmOut which plays to the MIDI player in windows.
            _o = new Orchestra(new WinmmOut(0, bpm));
            // Here we use the orchestra to create a new Instrument, in this case a choir.
            // We chose to use the chromatic scale from the Scale class, it is however unneccesary since it uses this scale by default.
            _instrument = _o.AddInstrument(InstrumentType.ChoirAahs, Scale.ChromaticScale, 0);
            _isPlaying = true;
        }

        private void UpdateMethod(object state)
        {
            // When IsDrawing is true, the Game will be drawn.
            if (IsDrawing)
                Draw();

            // When the player has asked for the game to play music this will call the PlayMidiMusic method.
            // We set this method to be called every fifth iteration to prevent too much overlap in the music.
            _musicCounter++;
            if (_isPlaying && _musicCounter == 5)
            {
                PlayMidiMusic();
                _musicCounter = 0;
            }

            // This part updates the values of each cell.
            for (int y = 0; y < _yValue; y++)
            {
                for (int x = 0; x < _xValue; x++)
                {
                    _cellsTemporaryState[x, y] = IsAlive(x, y);
                }
            }
            // This method simply copies all values from one multidimensional array to another.
            CopyCells();
        }

        // This method plays different sounds at different times based on the coordinates of all living cells in the game.
        // The x coordinates decides which tones are used.
        // The y-coordinates decides when sounds are played.
        private void PlayMidiMusic()
        {
            SequentialMusicList sMusic = new SequentialMusicList();
            int localInt;

            // This nested for-loop iterates over all cells in the game.
            for (int y = 0; y < _yValue; y++)
            {
                for (int x = 0; x < _xValue; x++)
                {
                    // This if statement checks whether a cell is still alive
                    if (Cells[x, y])
                    {
                        localInt = x < _xValue/2 ? x : _xValue - x;
                        // This creates a new keystroke with an offset based on where in the grid the cell is, and adds it to the SequentailMusicList.
                        sMusic.Add(new Keystroke(Tone.C, 1).OffsetBy(_quintScale, localInt));
                    }
                }
                // We add a pause here to play sounds later if they have a greater y-coordinate.
                sMusic.Add(new Pause(1));
            }
            // This plays all the sounds in the list, as longs it contains any ,and as long as the instrument isn't null.
            if(sMusic.Count != 0)
                _instrument?.Play(sMusic);
        }

        private void CopyCells()
        {
            for (int y = 0; y < _cellsTemporaryState.GetLength(1); y++)
            {
                for (int x = 0; x < _cellsTemporaryState.GetLength(0); x++)
                {
                    Cells[x, y] = _cellsTemporaryState[x, y];
                }
            }
        }

        private void Draw()
        {
            DirectRender1 render = new DirectRender1(_xValue, _yValue);
            render.Update(Cells);
        }
        
        internal bool IsAlive(int xValue, int yValue)
        {
            // This value depicts the x-value 1 lower than that of the cell being operated on. 
            // If the cell being operated has an x-value of 0, this will instead become the highest possible x-value.
            int xValueIfLow = xValue == 0 ? _xValue - 1 : xValue - 1;

            // This value depicts the x-value 1 higher than that of the cell being operated on. 
            //If the cell being operated has the highest possible x-value , this will instead become 0.
            int xValueIfHigh = xValue == _xValue - 1 ? 0 : xValue + 1;

            // This value depicts the y-value 1 lower than that of the cell being operated on. 
            //If the cell being operated has an y-value of 0, this will instead become the highest possible y-value.
            int yValueIfLow = yValue == 0 ? _yValue - 1 : yValue - 1;

            // This value depicts the y-value 1 higher than that of the cell being operated on. 
            //If the cell being operated has the highest possible y-value , this will instead become 0.
            int yValueIfHigh = yValue == _yValue - 1 ? 0 : yValue + 1;
            
            /*
                The code is depicted excectly like this
                x  x  x
                x  o  x
                x  x  x
                where o is the point we are working on, and x is the points surrounding it.
            */
            bool[] neighBours =
            {
                Cells[ xValueIfLow, yValueIfLow],  Cells[xValue, yValueIfLow],  Cells[xValueIfHigh, yValueIfLow],
                Cells[ xValueIfLow, yValue],                                    Cells[xValueIfHigh, yValue],
                Cells[ xValueIfLow, yValueIfHigh], Cells[xValue, yValueIfHigh], Cells[xValueIfHigh, yValueIfHigh]
            };

            int liveNeighBours = neighBours.Count(b => b);

            bool isAlive = Cells[xValue, yValue];
            if (liveNeighBours < 2 && isAlive) // Any live cell with fewer than two live neighbours dies, as if caused by under-population.
            {
                isAlive = false;
            }
            else if ((liveNeighBours == 2 || liveNeighBours == 3) && isAlive) // Any live cell with two or three live neighbours lives on to the next generation.
            { }
            else if (liveNeighBours > 3 && isAlive) // Any live cell with more than three live neighbours dies, as if by over-population.
            {
                isAlive = false;
            }
            else if (liveNeighBours == 3 && !isAlive) // Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
            {
                isAlive = true;
            }
            return isAlive;
        }
    }
}
