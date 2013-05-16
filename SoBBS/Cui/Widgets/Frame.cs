using Sobbs.Config.Sizes;
using Sobbs.Cui.Interfaces;
using MinCurses;

namespace Sobbs.Cui.Widgets
{
    public class Frame : Container, IFrame
    {
        public Frame(Size x, Size y, Size width, Size height, string title)
            : base(x, y, width, height)
        {
            Title = title;
        }

        private string Title { get; set; }

        protected override void RefreshContainer(int x, int y, int width, int height)
        {
            Curses.DrawFrame(x, y, x + width - 1, y + height - 1);
            Curses.Put(y,x+1, " " + Title + " ");
        }
    }
}
