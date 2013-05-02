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

        public static Func<TIn, TOut> Constant<TIn, TOut>(TOut constant)
        {
            return val => constant;
        }
    }
}

