using Sobbs.Config.Sizes;

namespace Sobbs.Cui.Info
{
    public abstract class WidgetInfo
    {
        protected WidgetInfo(Size x, Size y, Size width, Size height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Size X { get; private set; }
        public Size Y { get; private set; }
        public Size Width { get; private set; }
        public Size Height { get; private set; }
    }
}