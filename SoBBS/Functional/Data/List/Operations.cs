using System;
using System.Collections.Generic;
using Sobbs.Functional.Data.List;

namespace Sobbs.Data.List
{
    public static class Operations
    {
        public static IImmutableList<TOut> Map<TIn, TOut>(this IImmutableList<TIn> list, Func<TIn, TOut> f)
        {
            if (list.IsEmpty)
                return ImmutableList<TOut>.Empty;

            return list.Tail.Map(f).Add(f.Invoke(list.Value));
        }

        public static TVal Lookup<TVal, TKey>(this IImmutableList<TVal> list, TKey key, Func<TVal, TKey> keyExtractor)
        {
            if (list.IsEmpty)
                throw new IndexOutOfRangeException("Key " + key + " not found");

            if (keyExtractor.Invoke(list.Value).Equals(key))
                return list.Value;

            return list.Tail.Lookup(key, keyExtractor);
        }

        public static TVal Lookup<TKey, TVal>(this IImmutableList<KeyValuePair<TKey, TVal>> list, TKey key)
        {
            if (list.IsEmpty)
                throw new IndexOutOfRangeException("Key " + key + " not found");

            if (list.Value.Key.Equals(key))
                return list.Value.Value;

            return list.Tail.Lookup(key);
        }
    }
}

