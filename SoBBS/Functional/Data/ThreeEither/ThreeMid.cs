using System;

namespace Sobbs.Functional.Data.ThreeEither
{
    public class ThreeMid<TLeft, TMid, TRight> : IThreeEither<TLeft, TMid, TRight>
    {
        private TMid Value
        {
            get; set;
        }

        public ThreeMid(TMid value)
        {
            Value = value;
        }

        public TOut Either<TOut>(Func<TLeft, TOut> fLeft, Func<TMid, TOut> fMid, Func<TRight, TOut> fRight)
        {
            return fMid.Invoke(Value);
        }

        public override string ToString()
        {
            return "Mid " + Value;
        }
    }
}
