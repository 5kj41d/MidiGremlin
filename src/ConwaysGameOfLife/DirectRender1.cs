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

        protected CharInfo[] Buffer;
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
