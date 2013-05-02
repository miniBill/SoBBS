using System;

namespace Sobbs.Data.List
{
    public class ImmutableList<T> : IImmutableList<T>
    {
        private class EmptyImmutableList<TU> : IImmutableList<TU>
        {
            public bool IsEmpty
            {
                get
                {
                    return true;
                }
            }

            public TU Value
            {
                get
                {
                    throw new InvalidOperationException();
                }
            }

            public IImmutableList<TU> Tail
            {
                get
                {
                    throw new InvalidOperationException();
                }
            }

            public IImmutableList<TU> Add(TU value)
            {
                return new ImmutableList<TU>(value, this);
            }
        }

        public static readonly IImmutableList<T> Empty = new EmptyImmutableList<T>();

        public bool IsEmpty
        {
            get
            {
                return false;
            }
        }

        public T Value
        {
            get;
            private set;
        }

        public IImmutableList<T> Tail
        {
            get;
            private set;
        }

        private ImmutableList(T value, IImmutableList<T> tail)
        {
            Value = value;
            Tail = tail;
        }

        public IImmutableList<T> Add(T value)
        {
            return new ImmutableList<T>(value, this);
        }
    }
}

