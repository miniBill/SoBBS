using System;
using System.Collections.Generic;

namespace Sobbs.Data.List {
	public static class Operations {
		public static IImmutableList<U> Map<T,U>(this IImmutableList<T> list, Func<T,U> f) {
			if(list.IsEmpty)
				return ImmutableList<U>.Empty;

			return list.Tail.Map(f).Add(f.Invoke(list.Value));
		}

		public static T Lookup<T,U>(this IImmutableList<T> list, U key, Func<T,U> keyExtractor) {
			if(list.IsEmpty)
				throw new IndexOutOfRangeException("Key " + key + " not found");

			if(keyExtractor.Invoke(list.Value) .Equals(key))
				return list.Value;

			return list.Tail.Lookup(key, keyExtractor);
		}

		public static U Lookup<T,U>(this IImmutableList<KeyValuePair<T,U>> list, T key) {
			if(list.IsEmpty)
				throw new IndexOutOfRangeException("Key " + key + " not found");

			if(list.Value.Key.Equals(key))
				return list.Value.Value;

			return list.Tail.Lookup(key);
		}
	}
}

