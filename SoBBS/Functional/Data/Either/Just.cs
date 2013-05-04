using System;
using System.Collections;
using System.Collections.Generic;

namespace Sobbs.Functional.Data.Either
{
    public class Just<T> : Maybe<T>
    {
        public Just(T value)
        {
            Value = value;
        }

        private T Value
        {
            get;
            set;
        }

        public Maybe<TOut> Bind<TOut>(Func<T, Maybe<TOut>> func)
        {
            return func(Value);
        }

        public IEnumerator<T> GetEnumerator()
        {
            yield return Value;
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}