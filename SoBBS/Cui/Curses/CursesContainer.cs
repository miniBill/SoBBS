using System.Collections;
using System.Collections.Generic;
using Sobbs.Config.Sizes;
using Sobbs.Cui.Info;
using Sobbs.Cui.Interfaces;

namespace Sobbs.Cui.Curses
{
    public abstract class CursesContainer : CursesWidget, IContainer
    {
        private readonly List<CursesWidget> _children = new List<CursesWidget>();

        protected CursesContainer(Size x, Size y, Size width, Size height, IContainer parent)
            : base(x, y, width, height, parent)
        {
        }

        public IEnumerator<IWidget> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        public IFrame Add(FrameInfo info)
        {
            var frame = new CursesFrame(info.X, info.Y, info.Width, info.Height, info.Title, this);
            _children.Add(frame);
            return frame;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Refresh(int px, int py, int pwidth, int pheight)
        {
            int x1 = Sizer.X(X, pwidth) + px;
            int y1 = Sizer.Y(Y, pheight) + py;
            var x2 = Sizer.X(X, Width, pwidth) + px;
            var y2 = Sizer.Y(Y, Height, pheight) + py;
            int w = x2 - x1 + 1;
            int h = y2 - y1 + 1;
            RefreshContainer(x1, y1, w, h);
            RefreshChildren(x1, y1, w, h);
        }

        protected abstract void RefreshContainer(int x, int y, int width, int height);

        private void RefreshChildren(int x, int y, int width, int height)
        {
            foreach (var child in _children)
            {
                child.Refresh(x, y, width, height);
            }
        }
    }
}