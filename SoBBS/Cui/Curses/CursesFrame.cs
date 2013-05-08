namespace Sobbs.Cui.Curses
{
    /*public class InternalCursesFrame : Frame
    {
        public InternalCursesFrame(int x, int y, int width, int height, string title)
            : base(x, y, width, height, title)
        {
        }

        public override bool ProcessHotKey(int key)
        {
            return base.ProcessHotKey(key) || (OnProcessHotKey != null && OnProcessHotKey(this, new KeyPressedEventArgs(key)));
        }

        public event KeyPressedEventHandler<InternalCursesFrame> OnProcessHotKey;
    }

    public class CursesFrame : CursesContainer<InternalCursesFrame>, IFrame
    {
        public CursesFrame(FrameInfo info)
            : base(new InternalCursesFrame(info.X, info.Y, info.Width, info.Height, info.Title))
        {
            Implementation.OnProcessHotKey += (sender, e) => ProcessHotKey(e.Key);
        }

        private readonly List<KeyPressedEventHandler<IFrame>> _onProcessHotKey = new List<KeyPressedEventHandler<IFrame>>();

        public string Title
        {
            get { return Implementation.Title; }
        }

        public void UpdateData()
        {
            if (OnUpdateData != null)
                OnUpdateData(this, EventArgs.Empty);
        }

        public event EventHandler OnUpdateData;

        public override event KeyPressedEventHandler<IFrame> OnProcessHotKey
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
        public virtual event KeyPressedEventHandler<IFrame> OnProcessHotKey;
        public int W
        {
            get
            {
                return Implementation.w;
            }
        }
        public int H
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
    }*/
}
