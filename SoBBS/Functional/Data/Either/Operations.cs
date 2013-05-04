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

    public class Nothing<T> : Maybe<T>
    {
        public override bool Equals(object obj)
        {
            return obj is Nothing<T>;
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }

        public Maybe<TOut> Bind<TOut>(Func<T, Maybe<TOut>> func)
        {
            return new Nothing<TOut>();
        }

        public override string ToString()
        {
            return "Nothing";
        }
    }

    public class Just<T> : Maybe<T>
    {
        public Just(T value)
        {
            Value = value;
        }

        public T Value
        {
            get;
            private set;
        }

        public Maybe<TOut> Bind<TOut>(Func<T, Maybe<TOut>> func)
        {
            return func(Value);
        }

        public override string ToString()
        {
            return "Just " + Value;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Just<T>;
            return other != null && other.Value.Equals(Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }


    // ReSharper disable InconsistentNaming
    public interface Maybe<out T>
    // ReSharper restore InconsistentNaming
    {
        Maybe<TOut> Bind<TOut>(Func<T, Maybe<TOut>> func);
    }
}
