using System;
using Sobbs.Config.Ini;
using Sobbs.Data.List;

namespace Sobbs.Config.Windows {
	class Mid<A,B,C> : IEither<A,B,C> {
		public B Value {
			get;
			private set;
		}

		public Mid(B value) {
			Value = value;
		}

		public T Either<T>(Func<A, T> fLeft, Func<B, T> fMid, Func<C, T> fRight) {
			return fMid.Invoke(Value);
		}
	}

}
