using System;

namespace Sobbs.Functional.Data.Either
{
    public static class Operations
    {
        public static void DoEither<TLeft, TRight>(this IEither<TLeft, TRight> either, Action
           <TLeft> actLeft, Action<TRight> actRight)
        {
            either.Either(actLeft.ToFunc(), actRight.ToFunc());
        }

        public static Maybe<TC> SelectMany<TA, TB, TC>(this Maybe<TA> a, Func<TA, Maybe<TB>> func, Func<TA, TB, TC> select)
        {
            return a.Bind(
                    aval => func(aval).Bind(
                        bval => select(aval, bval).ToMaybe()
                    )
                   );
        }

        public static Maybe<T> ToMaybe<T>(this T value)
        {
            return value == null ? (Maybe<T>)new Nothing<T>() : new Just<T>(value);
        }
    }
}
