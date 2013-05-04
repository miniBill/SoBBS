using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Terminal;

namespace Sobbs.Cui.Curses
{
    public class CursesFrame : CursesContainer<Frame>, IFrame
    {
        public CursesFrame(FrameInfo info)
            : base(new Frame(info.X, info.Y, info.Width, info.Height, info.Title))
        {
        }

        private readonly List<KeyPressedEventHandler> _onProcessHotKey = new List<KeyPressedEventHandler>();

        public event EventHandler OnUpdate;
        public string Title
        {
            get { return Implementation.Title; }
        }

        public void Update()
        {
            if (OnUpdate != null)
                OnUpdate(this, EventArgs.Empty);
        }

        public override event KeyPressedEventHandler OnProcessHotKey
        {
            add
            {
                _onProcessHotKey.Add(value);
            }
            remove
            {
                _onProcessHotKey.Remove(value);
            }
        }

        private bool ProcessHotKey(int key)
        {
            if (Implementation.ProcessHotKey(key))
                return true;
            var args = new KeyPressedEventArgs(key);
            return _onProcessHotKey.Any(handler => handler(this, args));
        }

        public override IFrame Add(FrameInfo info)
        {
            var widget = base.Add(info);
            var frame = widget as CursesFrame;
            if (frame != null)
            {
                _onProcessHotKey.Add((sender, eventArgs) => frame.ProcessHotKey(eventArgs.Key));
            }
            return frame;
        }
    }

    public class CursesContainer<T> : CursesWidget<T>, IContainer where T : Container
    {
        private readonly List<IWidget> _children = new List<IWidget>();

        protected CursesContainer(T implementation)
            : base(implementation)
        {
        }

        public IListView Add(ListViewInfo info)
        {
            var widget = new CursesListView(info);
            Add(widget);
            return widget;
        }

        private void Add<TI>(CursesWidget<TI> widget) where TI : Widget
        {
            Implementation.Add(widget.Implementation);
            _children.Add(widget);
        }

        public virtual IFrame Add(FrameInfo info)
        {
            var widget = new CursesFrame(info);
            Add(widget);
            return widget;
        }

        public IEnumerator<IWidget> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class CursesListView : CursesWidget<ListView>, IListView
    {
        public CursesListView(ListViewInfo info)
            : base(new ListView(info.X, info.Y, info.Width, info.Height, info.Provider))
        {
        }
    }

    public class CursesWidget<T> : IWidget where T : Widget
    {
        public virtual event KeyPressedEventHandler OnProcessHotKey;
        public int w
        {
            get
            {
                return Implementation.w;
            }
        }
        public int h
        {
            get
            {
                return Implementation.h;
            }
        }
        public T Implementation { get; private set; }

        protected CursesWidget(T implementation)
        {
            Implementation = implementation;
        }
    }
}
