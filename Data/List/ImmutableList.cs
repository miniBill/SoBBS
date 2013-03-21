using System;

namespace Sobbs.Data.List {
	public class ImmutableList<T> : IImmutableList<T> {
		private class EmptyImmutableList<U> : IImmutableList<U> {
			public bool IsEmpty {
				get {
					return true;
				}
			}

			public U Value {
				get {
					throw new InvalidOperationException();
				}
			}

			public IImmutableList<U> Tail {
				get {
					throw new InvalidOperationException();
				}
			}

			public EmptyImmutableList() {

			}

			public IImmutableList<U> Add(U value) {
				return new ImmutableList<U>(value, this);
			}
		}

		public static readonly IImmutableList<T> Empty = new EmptyImmutableList<T>();

		public bool IsEmpty {
			get {
				return false;
			}
		}

		public T Value {
			get;
			private set;
		}

		public IImmutableList<T> Tail { 
			get;
			private set;
		}

		private ImmutableList(T value, IImmutableList<T> tail) {
			Value = value;
			Tail = tail;
		}

		public IImmutableList<T> Add(T value) {
			return new ImmutableList<T>(value, this);
		}
	}
}

