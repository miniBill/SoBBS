using System;
using System.Collections.Generic;

namespace MinCurses
{
    public static class Curses
    {
        static void InitScr()
        {
            StdScr = Native.initscr();
        }

        private static IntPtr StdScr
        {
            get;
            set;
        }

        static bool Echo
        {
            set
            {
                if (value)
                {
                    Native.echo();
                }
                else
                {
                    Native.noecho();
                }
            }
        }

        static Visibility CursorVisibility
        {
            set
            {
                Native.curs_set((int)value);
            }
        }

        static bool HasColors
        {
            get
            {
                return Native.has_colors();
            }
        }

        public static void Erase()
        {
            Native.erase();
        }

        public static void Refresh()
        {
            Native.refresh();
        }

        public static void EndWin()
        {
            Native.endwin();
        }

        static void StartColor()
        {
            Native.start_color();
        }

        static void InitPair(short index, Color fg, Color bg)
        {
            Native.init_pair(index, (short)fg, (short)bg);
        }

        public static void Init()
        {
            InitScr();

            CBreak = true;
            Echo = false;
            CursorVisibility = 0;

            if (!HasColors) return;
            StartColor();
            ColorPairs.Add(GetColorKey(Color.White, Color.Black), 0);
        }

        public static int GetChar()
        {
            return Native.wgetch(StdScr);
        }

        public static int Lines
        {
            get
            {
                return Native.get_lines();
            }
        }

        public static int Cols
        {
            get
            {
                return Native.get_cols();
            }
        }

        static void Insert(int y, int x, uint c)
        {
            Native.mvinswch(y, x, c, _currentColors);
        }

        static void Add(int y, int x, uint c)
        {
            Native.mvaddwch(y, x, c, _currentColors);
        }

        private static void Put(int y, int x, uint c)
        {
            if (y < 0 || y >= Lines)
                throw new ArgumentOutOfRangeException("y", "y (" + y + " is out of range: [0, " + Lines + "[");
            if (x < 0 || x >= Cols)
                throw new ArgumentOutOfRangeException("x", "x (" + x + " is out of range: [0, " + Cols + "[");
            if (y == Lines - 1)
                Insert(y, x, c);
            else
                Add(y, x, c);
        }

        public static void Put(int y, int x, string value, Color foreground = Color.White, Color background = Color.Black)
        {
            SetColors(foreground, background);
            for (int xi = 0; xi + x < Cols && xi < value.Length; xi++)
                Put(y, xi + x, value[xi]);
            SetDefaultColors();
        }

        private static readonly Dictionary<int, short> ColorPairs = new Dictionary<int, short>();
        private static short _currentColors;

        private static void SetDefaultColors()
        {
            SetColors(Color.White, Color.Black);
        }

        private static void SetColors(Color foreground, Color background)
        {
            var key = GetColorKey(foreground, background);
            if (ColorPairs.ContainsKey(key))
                _currentColors = ColorPairs[key];
            else
            {
                _currentColors = (short)ColorPairs.Count;
                ColorPairs.Add(key, _currentColors);
                InitPair(_currentColors, foreground, background);
            }
        }

        private static int GetColorKey(Color foreground, Color background)
        {
            return (short)foreground << 16 + (short)background;
        }

        public static void DrawFrame(int x1, int y1, int x2, int y2, bool @double = false)
        {
            if (@double)
                DrawFrame(x1, y1, x2, y2, DoubleH, DoubleV, DoubleTl, DoubleTr, DoubleBl, DoubleBr);
            else
                DrawFrame(x1, y1, x2, y2, H, V, Tl, Tr, Bl, Br);
        }

        private static void DrawFrame(int x1, int y1, int x2, int y2, uint h, uint v, uint tl, uint tr, uint bl, uint br)
        {
            for (int x = x1 + 1; x < x2; x++)
            {
                Put(y1, x, h);
                Put(y2, x, h);
            }
            for (int y = y1 + 1; y < y2; y++)
            {
                Put(y, x1, v);
                Put(y, x2, v);
            }
            Put(y1, x1, tl);
            Put(y1, x2, tr);
            Put(y2, x1, bl);
            Put(y2, x2, br);
        }

        private const uint DoubleH = 0x2550u;
        private const uint DoubleV = 0x2551u;
        private const uint DoubleTl = 0x2554u;
        private const uint DoubleTr = 0x2557u;
        private const uint DoubleBl = 0x255Au;
        private const uint DoubleBr = 0x255Du;

        private const uint H = 0x2500u;
        private const uint V = 0x2502u;
        private const uint Tl = 0x250Cu;
        private const uint Tr = 0x2510u;
        private const uint Bl = 0x2514u;
        private const uint Br = 0x2518u;

        private static bool CBreak
        {
            set
            {
                if (value)
                {
                    Native.cbreak();
                }
                else
                {
                    Native.nocbreak();
                }
            }
        }
    }
}

