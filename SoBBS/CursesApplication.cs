using System;
using System.Collections;
using System.Collections.Generic;
using CursesSharp;
using Sobbs.Cui;

namespace Sobbs
{
    public class CursesApplication : IApplication
    {
        public IContainer MainContainer { get; private set; }

        public CursesApplication()
        {
            Curses.InitScr();
            MainContainer = new CursesFrame();
        }

        public void Dispose()
        {
            Curses.EndWin();
        }
    }

    public class CursesFrame : CursesWidget, IContainer
    {
        private readonly List<CursesWidget> _children = new List<CursesWidget>();

        public IEnumerator<IWidget> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        public void Add(FrameInfo info)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class CursesWidget : IWidget
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
    }
}
