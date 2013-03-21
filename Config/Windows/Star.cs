using System;
using Sobbs.Config.Ini;
using Sobbs.Data.List;

namespace Sobbs.Config.Windows {
	public class Star {
		private Star() {
		}

		public static readonly Star Instance = new Star();
	}
}
