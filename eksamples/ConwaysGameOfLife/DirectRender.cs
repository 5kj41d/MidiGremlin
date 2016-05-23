using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace ConwaysGameOfLife
{
    class DirectRender
    {
        internal int ConsoleSizeXValue;
        internal int ConsoleSizeYValue;

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] uint fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] int flags,
            IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
          SafeFileHandle hConsoleOutput,
          CharInfo[] lpBuffer,
          Coord dwBufferSize,
          Coord dwBufferCoord,
          ref SmallRect lpWriteRegion);

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        protected SafeFileHandle Handle;
        protected CharInfo[] Buffer;

        public DirectRender(int xSize, int ySize) 
        {
            //Create file handle to the console buffer
            //Basicaly lets us writes bytes to the console buffer, accces is
            //trough a file handle
            Handle = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            Buffer = new CharInfo[xSize * ySize];
            ConsoleSizeXValue = xSize;
            ConsoleSizeYValue = ySize;
        }

        public int Update(bool[,] array, bool forceAll = false)
        {
            if (!Handle.IsInvalid)
            {
                Console.SetWindowSize(ConsoleSizeXValue, ConsoleSizeYValue);
                for (int x = 0; x < ConsoleSizeXValue; x++)
                {
                    for (int y = 0; y < ConsoleSizeYValue; y++)
                    {
                        ConsoleColor tempColor = array[x, y] ? ConsoleColor.Blue : ConsoleColor.Black;
                        Buffer[x + (y * ConsoleSizeXValue)] = new CharInfo('#', tempColor, ConsoleColor.Black);
                    }
                }

                SmallRect rect = new SmallRect() { Bottom = 37, Left = 0, Right = 79, Top = 0 };
                bool Success = WriteConsoleOutput(Handle, Buffer,
                    new Coord((short)ConsoleSizeXValue, (short)ConsoleSizeYValue),
                    new Coord(0, 0),
                    ref rect);

                if (!Success)
                {
                    Trace.WriteLine("Error drawing console");
                }

            }
            else
            {
                Trace.WriteLine("Console handle invalid");
                throw new Exception();
            }

            return 79 * 37;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct CharInfo
    {
        const String HexStr = "0123456789ABCDEF";

        [FieldOffset(0)]
        public CharUnion Char;
        [FieldOffset(2)]
        public short Attributes;


        public ConsoleColor ForegroundColor
        {
            get { return (ConsoleColor)(Attributes & 0xF); }
            set
            {
                SetForeground(value);
            }
        }
        public ConsoleColor BackgroundColor
        {
            get { return (ConsoleColor)((Attributes & 0xF0) >> 4); }
            set
            {
                SetBackground(value);
            }
        }


        public override string ToString()
        {
            return String.Format("{0} {1}{2}", Char.UnicodeChar, HexStr[(int)Attributes & 0xF], HexStr[((int)Attributes & 0xF0) >> 4]);
        }

        public CharInfo(char c)
        {
            Char.AsciiChar = 0;
            Char.UnicodeChar = c;
            Attributes = 0;

        }
        public CharInfo(char c, ConsoleColor fg, ConsoleColor bg)
        {
            Attributes = 0;
            Char.AsciiChar = 0;
            Char.UnicodeChar = c;
            SetForeground(fg);
            SetBackground(bg);
        }

        public CharInfo(char c, short attrib)
        {
            Attributes = attrib;
            Char.AsciiChar = 0;
            Char.UnicodeChar = c;
        }

        public CharInfo(byte c, short attrib)
        {
            Char.UnicodeChar = ' ';
            Char.AsciiChar = c;
            Attributes = attrib;
        }

        private void SetBackground(ConsoleColor bg)
        {
            short existing = (short)(Attributes & 0xFF0F);
            short newAtrib = (short)(((int)bg << 4) & 0x00F0);

            Attributes = (short)(existing | newAtrib);
        }

        private void SetForeground(ConsoleColor fg)
        {
            short existing = (short)(Attributes & 0xFFF0);
            short newAtrib = (short)((int)fg & 0x000F);

            Attributes = (short)(existing | newAtrib);
        }

        public static implicit operator char(CharInfo t)
        {
            return t.Char.UnicodeChar;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct CharUnion
    {
        [FieldOffset(0)]
        public char UnicodeChar;
        [FieldOffset(0)]
        public byte AsciiChar;
    }
}
