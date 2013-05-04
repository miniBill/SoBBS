using System.Collections.Generic;

namespace Sobbs.Cui
{
    public interface IContainer : IWidget, IEnumerable<IWidget>
    {
        IListView Add(ListViewInfo info);
        IFrame Add(FrameInfo info);
    }
}