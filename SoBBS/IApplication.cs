using System;
using Sobbs.Cui;

namespace Sobbs
{
    public interface IApplication : IDisposable
    {
        IContainer MainContainer { get; }
    }
}
