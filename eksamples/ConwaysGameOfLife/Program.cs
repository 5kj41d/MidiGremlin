using System;

namespace ConwaysGameOfLife
{
    internal class Program
    {
        /*
            This example will use the concept of Conway's Game of Life to create sound.
            The sound will be made using MIDI Gremlin.
        */
        private static void Main(string[] args)
        {
            bool[,] grid = new bool[40,40];
            // Setup some start values

            // Glider 1 : depicted as followed
            // 0 1 0
            // 0 0 1
            // 1 1 1
            grid[2, 1] = true;  
            grid[3, 2] = true;
            grid[3, 3] = true;
            grid[2, 3] = true;
            grid[1, 3] = true;

            // Glider 2
            // 1 1 1
            // 0 0 1
            // 0 1 0
            grid[12, 13] = true;
            grid[11, 11] = true;
            grid[12, 11] = true;
            grid[13, 11] = true;
            grid[13, 12] = true;


            // Start The Game of Life
            GameOfLife gol = new GameOfLife(grid);
            
            // Start Drawing
            gol.IsDrawing = true;

            // Play sounds using the Game of Life
            gol.MusicSetup();

            Console.ReadKey();
        }
    }

}
