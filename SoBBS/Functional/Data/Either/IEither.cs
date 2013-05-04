using System;

namespace Sobbs.Functional.Data.Either
{
    public interface IEither<out TLeft, out TRight>
    {
        TOut Either<TOut>(Func<TLeft, TOut> fLeft, Func<TRight, TOut> fRight);
    }
}
