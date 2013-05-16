using Sobbs.Config.Sizes;
using Sobbs.Cui.Interfaces;

namespace Sobbs.Cui.Widgets
{
    public abstract class Widget : IWidget
    {
        protected Size X { get; private set; }
        protected Size Y { get; private set; }
        protected Size Width { get; private set; }
        protected Size Height { get; private set; }

        protected Widget(Size x, Size y, Size width, Size height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public abstract void Refresh(int px, int py, int pwidth, int pheight);
    }
}