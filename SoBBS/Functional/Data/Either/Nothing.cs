using System;
using System.Collections;
using System.Collections.Generic;

namespace Sobbs.Functional.Data.Either
{
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Maybe<TOut> Bind<TOut>(Func<T, Maybe<TOut>> func)
        {
            return new Nothing<TOut>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            yield break;
        }

        public override string ToString()
        {
            return "Nothing";
        }
    }
}