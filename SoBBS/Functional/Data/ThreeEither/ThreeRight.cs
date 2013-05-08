using System;

namespace Sobbs.Functional.Data.ThreeEither
{
    public class ThreeRight<TLeft, TMid, TRight> : IThreeEither<TLeft, TMid, TRight>
    {
        private TRight Value
        {
            get; set;
        }

        public ThreeRight(TRight value)
        {
            Value = value;
        }

        public TOut Either<TOut>(Func<TLeft, TOut> fLeft, Func<TMid, TOut> fMid, Func<TRight, TOut> fRight)
        {
            return fRight.Invoke(Value);
        }

        public override string ToString()
        {
            return "Right " + Value.ToString();
        }
    }
}
