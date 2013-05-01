using System;
using Sobbs.Config.Ini;
using Sobbs.Data.List;

namespace Sobbs.Config.Windows
{
    public interface IEither<A, B, C>
    {
        T Either<T>(Func<A, T> fLeft, Func<B, T> fMid, Func<C, T> fRight);
    }
}
