using System;
using Sobbs.Config.Ini;
using Sobbs.Data.List;

namespace Sobbs.Config.Windows {
	public class Left<A, B, C> : IEither<A, B, C> {
		public int Value {
			get;
			private set;
		}

		public Left(int value) {
			Value = value;
		}
	}

}
