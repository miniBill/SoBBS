using System;
using System.Collections.Generic;

namespace Sobbs.Functional.Data.Maybe
{
    public interface IMaybe<out T> : IEnumerable<T>
    {
        IMaybe<T> Bind<TOut>(Func<T, IMaybe<TOut>> func);
    }
}