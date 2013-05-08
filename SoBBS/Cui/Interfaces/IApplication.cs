using System;

namespace Sobbs.Cui.Interfaces
{
    public interface IApplication : IDisposable
    {
        IContainer MainContainer { get; }
        void Refresh();
    }
}
