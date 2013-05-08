using Sobbs.Config.Sizes;
using Sobbs.Cui.Interfaces;

namespace Sobbs.Cui.Curses
{
    public abstract class CursesWidget : IWidget
    {
        public Size X { get; private set; }
        public Size Y { get; private set; }
        public Size Width { get; private set; }
        public Size Height { get; private set; }
        public IContainer Parent { get; private set; }

        protected CursesWidget(Size x, Size y, Size width, Size height, IContainer parent)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Parent = parent;
        }

        public abstract void Refresh(int px, int py, int pwidth, int pheight);
    }
}