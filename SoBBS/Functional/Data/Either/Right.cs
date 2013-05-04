using System;

namespace Sobbs.Functional.Data.Either
{
    public class Right<TLeft, TRight> : IEither<TLeft, TRight>
    {
        private TRight Value
        {
            get;
            set;
        }

        public Right(TRight value)
        {
            Value = value;
        }

        public TOut Either<TOut>(Func<TLeft, TOut> fLeft, Func<TRight, TOut> fRight)
        {
            return fRight.Invoke(Value);
        }
    }
}
