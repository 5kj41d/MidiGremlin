using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ConwaysGameOfLife
{
    class DirectRender
    {
        //The thread we draw at, need to keep a reference when comparing. Also theoretically disposing it later.
        private readonly Thread _drawThread;

        //Queue of actions to perform on drawthread. Rarely used.
        private readonly ConcurrentQueue<Action> _queue = new ConcurrentQueue<Action>();

        //List of every fucking box
        private readonly bool[] _cells;

        //Height/width of the bitmap
        private readonly int _height;

        //px * px of a square
        private readonly int _sizeOfSqure;

        //Number of elements per side
        private readonly int _nbrElementsX;
        private readonly int _nbrElementsY;

        //The bitmap it is all written to
        private readonly WriteableBitmap _image;


        internal DirectRender(int elementSize, int nbrElementsX, int nbrElementsY)
        {
            _nbrElementsX = nbrElementsX;
            _nbrElementsY = nbrElementsY;
            _image = new WriteableBitmap(_height * nbrElementsX, _height * nbrElementsY, 96, 96, PixelFormats.Bgr32, null);
        }

        private int MakeColor(byte red, byte green, byte blue)
        {
            int colorData = red << 16; // R
            colorData |= green << 8;   // G
            colorData |= blue << 0;   // B

            return colorData;
        }

        //Drawthread starts here
        internal void Draw(bool[,] cells)
        {
            IntPtr pointer = IntPtr.Zero;
            int stride = 0;
            bool gotLock = false;
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                gotLock = _image.TryLock(new Duration(TimeSpan.FromMilliseconds(100)));

                pointer = _image.BackBuffer;
                stride = _image.BackBufferStride;
            });

            for (int x = 0; x < _nbrElementsX; x++)
            {
                for (int y = 0; y < _nbrElementsY; y++)
                {
                    int tempColor = cells[x, y] ? MakeColor(0, 0, 150) : MakeColor(0, 0, 0);
                    Render(pointer, stride, tempColor, x, y);
                }
            }
        }

        private void Render(IntPtr pointer, int stride, int color, int xValue, int yValue)
        {
            //Calculate the size of upper and lower half. One full sized one double can always cover the entire arear
            Int32Rect render = new Int32Rect(xValue, yValue,_sizeOfSqure,_sizeOfSqure);

            //Make boxes smaller (by 1 px in all directions)
            render.Height -= 2;
            render.Width -= 2;
            render.X += 1;
            render.Y += 1;

            //Fill em
            FillSquare(pointer, stride, render, color);
        }

        //Fill a square
        private void FillSquare(IntPtr pointer, int stride, Int32Rect arear, int color)
        {
            if (arear.X < 0) throw new ArgumentOutOfRangeException();
            if (arear.Y < 0) throw new ArgumentOutOfRangeException();

            if (arear.X + arear.Width > _height) throw new ArgumentOutOfRangeException();
            if (arear.Y + arear.Height > _height) throw new ArgumentOutOfRangeException();

            unsafe  //unsafe = AWESOME
            {
                int pBackbuffer = (int)pointer;

                for (int y = arear.Y; y < arear.Y + arear.Height; y++)
                {
                    //x in inner loop is theoretically easier on the cache, no observable real world impact
                    for (int x = arear.X; x < arear.X + arear.Width; x++)
                    {
                        int offset = y * stride;  //calc point and do magic, ect ect
                        offset += x * 4;

                        *((int*)(pBackbuffer + offset)) = color;
                    }
                }
            }
        }
    }
}
