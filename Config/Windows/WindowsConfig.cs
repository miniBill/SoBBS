using System;
using Sobbs.Data.List;

namespace Sobbs.Config.Windows {
	public class WindowsConfig {
		public IImmutableList<WindowConfig> Configurations {
			get;
			private set;
		}

		public WindowConfig this[string key] {
			get {
				return Configurations.Lookup(key, conf => conf.Name);
			}
		}

		public WindowsConfig(IImmutableList<WindowConfig> configurations) {
			Configurations = configurations;
		}
	}
}

