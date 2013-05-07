using System;
using System.Collections.Generic;

namespace Sobbs.Functional.Data.Maybe
{
    public interface IMaybe<out T> : IEnumerable<T>
    {
        IMaybe<TOut> Bind<TOut>(Func<T, IMaybe<TOut>> func);
    }
}