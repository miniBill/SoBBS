namespace Sobbs.Cui.Curses
{
    public static class SoCurses
    {
        public static void DrawFrame(int x1, int y1, int x2, int y2, bool @double = false)
        {
            if (@double)
                DrawFrame(x1, y1, x2, y2, DoubleH, DoubleV, DoubleTl, DoubleTr, DoubleBl, DoubleBr);
            else
                DrawFrame(x1, y1, x2, y2, H, V, Tl, Tr, Bl, Br);
        }

        private static void DrawFrame(int x1, int y1, int x2, int y2, uint h, uint v, uint tl, uint tr, uint bl, uint br)
        {
            var stdscr = CursesSharp.Curses.StdScr;
            for (int x = x1 + 1; x < x2; x++)
            {
                stdscr.Add(y1, x, h);
                stdscr.Add(y2, x, h);
            }
            for (int y = y1 + 1; y < y2; y++)
            {
                stdscr.Add(y, x1, v);
                stdscr.Add(y, x2, v);
            }
            stdscr.Add(y1, x1, tl);
            stdscr.Add(y1, x2, tr);
            stdscr.Add(y2, x1, bl);
            if (y2 == CursesSharp.Curses.Lines - 1 && x2 == CursesSharp.Curses.Cols - 1)
                stdscr.Insert(y2, x2, br);
            else
                stdscr.Add(y2, x2, br);
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