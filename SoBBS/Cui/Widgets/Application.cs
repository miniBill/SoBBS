using Sobbs.Config.Sizes;
using Sobbs.Cui.Interfaces;
using MinCurses;

namespace Sobbs.Cui.Widgets
{
    public class Application : IApplication
    {
        public IContainer MainContainer { get; private set; }

        public void Refresh()
        {
            Curses.Erase();
            MainContainer.Refresh(0, 0, Curses.Cols, Curses.Lines);
            Curses.Refresh();
        }

        public Application(string title)
        {
            Curses.Init();

            MainContainer = new Frame(Size.Zero, Size.Zero, Size.Star, Size.Star, title);
        }

        public void Dispose()
        {
            Curses.EndWin();
        }
    }
}
