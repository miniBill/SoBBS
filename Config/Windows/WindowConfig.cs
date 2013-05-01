using Sobbs.Config.Sizes;

namespace Sobbs.Config.Windows
{
    public class WindowConfig
    {
        public string Name
        {
            get;
            private set;
        }

        public Size Left
        {
            get;
            private set;
        }

        public Size Top
        {
            get;
            private set;
        }

        public Size Width
        {
            get;
            private set;
        }

        public Size Height
        {
            get;
            private set;
        }

        public WindowConfig(string name, Size left, Size top, Size width, Size height)
        {
            Name = name;
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }
    }
}
