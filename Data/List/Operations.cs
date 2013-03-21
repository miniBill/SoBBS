using System;

namespace Sobbs.Data.List {
	public static class Operations {
		public static IImmutableList<U> Map<T,U>(this IImmutableList<T> list, Func<T,U> f) {
			if(list.IsEmpty)
				return ImmutableList<U>.Empty;

			return list.Tail.Map(f).Add(f.Invoke(list.Value));
		}
	}
}

