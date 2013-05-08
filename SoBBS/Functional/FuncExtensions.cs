using System;

namespace Sobbs.Functional
{
    public static class FuncExtensions
    {
        public static Func<T, T> Identity<T>()
        {
            return val => val;
        }

        public static Func<T, Unit> ToFunc<T>(this Action<T> action)
        {
            return val =>
                {
                    action(val);
                    return Unit.Instance;
                };
        }

        public static Action<T> Curry<T, U>(this Action<T, U> action, U parameter)
        {
            return arg1 => action(arg1, parameter);
        }

        public static Func<TIn, TOut> Constant<TIn, TOut>(TOut constant)
        {
            return val => constant;
        }
    }
}

