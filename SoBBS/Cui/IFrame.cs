using System;

namespace Sobbs.Cui
{
    public interface IFrame : IContainer
    {
        event EventHandler OnUpdate;
        string Title { get; }
    }
}