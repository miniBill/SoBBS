using System;
using System.Collections;
using System.Collections.Generic;

namespace Sobbs.Functional.Data.Maybe
{
    public class Nothing<T> : IMaybe<T>
    {
        private Nothing()
        {
        }

        public static readonly IMaybe<T> Instance = new Nothing<T>();

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

        public IMaybe<TOut> Bind<TOut>(Func<T, IMaybe<TOut>> func)
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