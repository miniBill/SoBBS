using System;

namespace Sobbs.Widgets
{
    public class KeyPressedEventArgs : EventArgs
    {
        public int Key
        {
            get;
            private set;
        }

        public KeyPressedEventArgs(int key)
        {
            Key = key;
        }
    }
}
