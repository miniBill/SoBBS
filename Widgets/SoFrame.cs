using Mono.Terminal;

namespace Sobbs.Widgets
{
    public class SoFrame : Frame
    {
        public SoFrame(int x, int y, int w, int h, string title)
            : base(x, y, w, h, title)
        {
        }

        public delegate bool KeyPressedEventHandler(SoFrame sender,KeyPressedEventArgs e);

        public event KeyPressedEventHandler OnProcessHotKey;

        public override bool ProcessHotKey(int key)
        {
            if (OnProcessHotKey == null)
                return false;
            return OnProcessHotKey.Invoke(this, new KeyPressedEventArgs(key));
        }
    }
}
