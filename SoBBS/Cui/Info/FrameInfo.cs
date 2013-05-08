using Sobbs.Config.Sizes;

namespace Sobbs.Cui.Info
{
    public class FrameInfo : WidgetInfo
    {
        /*
         Func<int, int> id = FuncExtensions.Identity<int>();
            Func<Star, int> zero = FuncExtensions.Constant<Star, int>(0);
            Func<Percent, int> widthPercent = perc => (int)Math.Round(width * perc.Value / 100.0);
            Func<Percent, int> heightPercent = perc => (int)Math.Round(height * perc.Value / 100.0);

            int x = conf.Left.Either(id, widthPercent, zero);
            int y = conf.Top.Either(id, heightPercent, zero);
            int w = conf.Width.Either(id, widthPercent, star => width - x);
            int h = conf.Height.Either(id, heightPercent, star => height - y);
            */

        public FrameInfo(Size x, Size y, Size width, Size height, string title)
            : base(x, y, width, height)
        {
            Title = title;
        }

        public string Title { get; private set; }
    }
}