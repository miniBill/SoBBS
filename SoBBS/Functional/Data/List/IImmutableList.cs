using System.Collections.Generic;

namespace Sobbs.Functional.Data.List
{
    public interface IImmutableList<T> : IEnumerable<T>
    {
        bool IsEmpty
        {
            get;
        }

        T Value
        {
            get;
        }

        IImmutableList<T> Tail
        {
            get;
        }

        IImmutableList<T> Add(T value);
    }
}
