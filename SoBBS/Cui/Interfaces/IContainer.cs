using System.Collections.Generic;

namespace Sobbs.Cui.Interfaces
{
    public interface IContainer : IWidget, IEnumerable<IWidget>
    {
        void Add(IWidget widget);
    }
}
