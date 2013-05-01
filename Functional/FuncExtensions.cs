using System;

namespace Sobbs.Functional {
	public static class FuncExtensions {
		public static Func<T, T> Identity<T>() {
			return val => val;
		}

		public static Func<T, U> Constant<T, U>(U constant) {
			return val => constant;
		}
	}
}

