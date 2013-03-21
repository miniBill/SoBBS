using System;
using Sobbs.Data.List;

namespace Sobbs.Config.Windows {
	public class Config {
		public IImmutableList<WindowConfig> Configurations {
			get;
			private set;
		}

		public Config(IImmutableList<WindowConfig> configurations) {
			Configurations = configurations;
		}
	}
}

