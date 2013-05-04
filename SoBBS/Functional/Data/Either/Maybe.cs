using System;
using System.Collections.Generic;

namespace Sobbs.Functional.Data.Either
{
    // ReSharper disable InconsistentNaming
    public interface Maybe<out T>
        // ReSharper restore InconsistentNaming
        : IEnumerable<T>
    {
        Maybe<TOut> Bind<TOut>(Func<T, Maybe<TOut>> func);
    }
}