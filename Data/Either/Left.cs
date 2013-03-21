using System;
using Sobbs.Config.Ini;
using Sobbs.Data.List;

namespace Sobbs.Config.Windows {
	public class Left<A, B, C> : IEither<A, B, C> {
		public A Value {
			get;
			private set;
		}

		public Left(A value) {
			Value = value;
		}

		public T Either<T>(Func<A, T> fLeft, Func<B, T> fMid, Func<C, T> fRight) {
			return fLeft.Invoke(Value);
		}
	}

}
