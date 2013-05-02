using Mono.Terminal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sobbs.Widgets
{
    public class SoFrame : Frame
    {
        public SoFrame(int x, int y, int w, int h, string title)
            : base(x, y, w, h, title)
        {
        }

        public delegate bool KeyPressedEventHandler(SoFrame sender,KeyPressedEventArgs e);

        private readonly List<KeyPressedEventHandler> _onProcessHotKey = new List<KeyPressedEventHandler>();

        public event KeyPressedEventHandler OnProcessHotKey
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

        public override bool ProcessHotKey(int key)
        {
            if(base.ProcessHotKey(key))
                return true;
            var args = new KeyPressedEventArgs(key);
            return _onProcessHotKey.Any(handler => handler(this, args));
        }

        public override void Add(Widget w)
        {
            base.Add(w);
            var frame = w as SoFrame;
            if (frame != null)
            {
                _onProcessHotKey.Add((sender, eventArgs) => frame.ProcessHotKey(eventArgs.Key));
            }
        }
    }
}
