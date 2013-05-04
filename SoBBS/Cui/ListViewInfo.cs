using Mono.Terminal;

namespace Sobbs.Cui
{
    public class ListViewInfo : WidgetInfo
    {
        public ListViewInfo(int x, int y, int width, int height, IListProvider provider)
            : base(x, y, width, height)
        {
            Provider = provider;
        }

        public IListProvider Provider { get; private set; }
    }
}