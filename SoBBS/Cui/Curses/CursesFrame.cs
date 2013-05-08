using Sobbs.Config.Sizes;
using Sobbs.Cui.Interfaces;

namespace Sobbs.Cui.Curses
{
    public class CursesFrame : CursesContainer, IFrame
    {
        public CursesFrame(Size x, Size y, Size width, Size height, string title, IContainer parent)
            : base(x, y, width, height, parent)
        {
            Title = title;
        }

        public string Title { get; private set; }

        protected override void RefreshContainer(int x, int y, int width, int height)
        {
            SoCurses.DrawFrame(x, y, x + width - 1, y + height - 1);
            CursesSharp.Curses.StdScr.Add(y, x + 1, " " + Title + " ");
        }
    }
}