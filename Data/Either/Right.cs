using System;
using Sobbs.Config.Ini;
using Sobbs.Data.List;

namespace Sobbs.Config.Windows
{
    public class Right<A, B, C> : IEither<A, B, C>
    {
        public C Value
        {
            get;
            private set;
        }

        public Right(C value)
        {
            Value = value;
        }

        public T Either<T>(Func<A, T> fLeft, Func<B, T> fMid, Func<C, T> fRight)
        {
            return fRight.Invoke(Value);
        }
    }
}
