using System;

namespace Sobbs.Cui
{
    public interface IFrame : IContainer
    {
        event EventHandler OnUpdateData;
        string Title { get; }
        void UpdateData();
    }
}