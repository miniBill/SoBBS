using System.Collections.Generic;

namespace Sobbs.Cui
{
    public interface IContainer : IWidget, IEnumerable<IWidget>
    {
        void Add(FrameInfo info);
    }
}
