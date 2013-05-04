namespace Sobbs.Cui
{
    public class FrameInfo : WidgetInfo
    {
        public FrameInfo(int x, int y, int width, int height, string title)
            : base(x, y, width, height)
        {
            Title = title;
        }

        public string Title { get; private set; }
    }
}