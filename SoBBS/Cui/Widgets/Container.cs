using System.Collections;
using System.Collections.Generic;
using Sobbs.Config.Sizes;
using Sobbs.Cui.Interfaces;

namespace Sobbs.Cui.Widgets
{
    public abstract class Container : Widget, IContainer
    {
        private readonly List<IWidget> _children = new List<IWidget>();

        protected Container(Size x, Size y, Size width, Size height)
            : base(x, y, width, height)
        {
        }

        public IEnumerator<IWidget> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        public void Add(IWidget widget)
        {
            _children.Add(widget);
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
            RefreshChildren(x1 + 1, y1 + 1, w - 2, h - 2);
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