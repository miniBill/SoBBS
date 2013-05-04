using System;

namespace Sobbs.Functional.Data.ThreeEither
{
    public interface IThreeEither<out TLeft, out TMid, out TRight>
    {
        TOut Either<TOut>(Func<TLeft, TOut> fLeft, Func<TMid, TOut> fMid, Func<TRight, TOut> fRight);
    }
}
