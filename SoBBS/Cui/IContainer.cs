using System.Collections.Generic;

namespace Sobbs.Cui
{
    public interface IContainer : IWidget, IEnumerable<IWidget>
    {
    }
}