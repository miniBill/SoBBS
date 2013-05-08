using CursesSharp;
using Sobbs.Config.Sizes;
using Sobbs.Cui.Interfaces;

namespace Sobbs.Cui.Curses
{
    public class CursesApplication : IApplication
    {
        public IContainer MainContainer { get; private set; }

        public void Refresh()
        {
            CursesSharp.Curses.StdScr.Erase();
            MainContainer.Refresh(0, 0, CursesSharp.Curses.Cols, CursesSharp.Curses.Lines);
            if (CursesSharp.Curses.StdScr != null)
                CursesSharp.Curses.StdScr.Refresh();
        }

        public CursesApplication(string title)
        {
            CursesSharp.Curses.InitScr();


            short[] colorTable = { 0, Colors.RED, Colors.BLUE, Colors.GREEN, Colors.CYAN, Colors.RED,
                                     Colors.MAGENTA, Colors.YELLOW, Colors.WHITE };

            Stdscr.Blocking = false;
            CursesSharp.Curses.Echo = false;
            CursesSharp.Curses.CursorVisibility = 0;

            if (!CursesSharp.Curses.HasColors) return;
            CursesSharp.Curses.StartColor();
            for (short i = 1; i < 8; ++i)
                CursesSharp.Curses.InitPair(i, colorTable[i], Colors.BLACK);

            MainContainer = new CursesFrame(Size.Zero, Size.Zero, Size.Star, Size.Star, title);
        }

        public void Dispose()
        {
            CursesSharp.Curses.EndWin();
        }
    }
}
