using System;

namespace MinCurses
{
    public enum Colors:short{
        BLACK,
        RED,
        BLUE,
        GREEN,CYAN,YELLOW,WHITE,MAGENTA
    }

    public static class Curses
    {
        static void InitScr()
        {
            throw new NotImplementedException();
        }

        static bool Blocking
        {
            get{ throw new NotImplementedException();}
            set{ throw new NotImplementedException();}
        }

        static bool Echo
        {
            get{ throw new NotImplementedException();}
            set{ throw new NotImplementedException();}
        }

        static int CursorVisibility
        {
            get{ throw new NotImplementedException();}
            set{ throw new NotImplementedException();}
        }

        static bool HasColors
        {
            get{ throw new NotImplementedException();}
            set{ throw new NotImplementedException();}
        }

        static void Erase()
        {
            throw new NotImplementedException();
        }

        static void Refresh()
        {
            throw new NotImplementedException();
        }

        static void EndWin()
        {
            throw new NotImplementedException();
        }

        static void StartColor()
        {
            throw new NotImplementedException();
        }

        static void InitPair(short i, Colors colors, Colors bLACK)
        {
            throw new NotImplementedException();
        }

        public static void Init()
        {
            InitScr();

            Colors[] colorTable = { 0, Colors.RED, Colors.BLUE, Colors.GREEN, Colors.CYAN, Colors.RED,
                Colors.MAGENTA, Colors.YELLOW, Colors.WHITE };

            Blocking = false;
            Echo = false;
            CursorVisibility = 0;

            if (!HasColors) return;
            StartColor();
            for (short i = 1; i < 8; ++i)
                InitPair(i, colorTable[i], Colors.BLACK);
        }

        public static char GetChar()
        {
            throw new NotImplementedException();
        }

     public  static int Lines
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public static int Cols
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        static void Insert(int y, int x, uint c)
        {
            throw new NotImplementedException();
        }

        static void Add(int y, int x, uint c)
        {
            throw new NotImplementedException();
        }

        private static void Put(int y, int x, uint c)
        {
            if (y < 0 || y >= Lines)
                throw new ArgumentOutOfRangeException("y", "y (" + y + " is out of range: [0, " + Lines + "[");
            if (x < 0 || x >= Cols)
                throw new ArgumentOutOfRangeException("x", "x (" + x + " is out of range: [0, " + Cols + "[");
            try
            {
                if (y == Lines - 1 /*&& x == Cols - 1*/)
                    Insert(y, x, c);
                else
                    Add(y, x, c);
            }
            catch (Exception e)
            {
                throw new Exception(":( ", e);
            }
        }

        public static void Put(int y, int x, string value)
        {
            for(int xi = x; xi < Cols && (xi - x) < value.Length; xi++)
                Put(y, xi, value[xi - x]);
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
                Curses.Put(y1, x, h);
                Curses.Put(y2, x, h);
            }
            for (int y = y1 + 1; y < y2; y++)
            {
                Curses.Put(y, x1, v);
                Curses.Put(y, x2, v);
            }
            Curses.Put(y1, x1, tl);
            Curses.Put(y1, x2, tr);
            Curses.Put(y2, x1, bl);
            Curses.Put(y2, x2, br);
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
    }
}

