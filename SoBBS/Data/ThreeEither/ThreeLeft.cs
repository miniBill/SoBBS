using System;

namespace Sobbs.Data.ThreeEither
{
    public class ThreeLeft<TLeft, TMid, TRight> : IThreeEither<TLeft, TMid, TRight>
    {
        private TLeft Value
        {
            get; set;
        }

        public ThreeLeft(TLeft value)
        {
            Value = value;
        }

        public TOut Either<TOut>(Func<TLeft, TOut> fLeft, Func<TMid, TOut> fMid, Func<TRight, TOut> fRight)
        {
            return fLeft.Invoke(Value);
        }
    }

}
