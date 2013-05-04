using System;

namespace Sobbs.Functional.Data.Either
{
    class Left<TLeft, TRight> : IEither<TLeft, TRight>
    {
        private TLeft Value
        {
            get;
            set;
        }

        public Left(TLeft value)
        {
            Value = value;
        }

        public TOut Either<TOut>(Func<TLeft, TOut> fLeft, Func<TRight, TOut> fRight)
        {
            return fLeft(Value);
        }
    }
}
