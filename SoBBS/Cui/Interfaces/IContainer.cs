using System.Collections.Generic;
using Sobbs.Cui.Info;

namespace Sobbs.Cui.Interfaces
{
    public interface IContainer : IWidget, IEnumerable<IWidget>
    {
        IFrame Add(FrameInfo info);
    }
}
