using System;
using Sobbs.Config.Ini;
using Sobbs.Data.List;

namespace Sobbs.Config.Windows {
	public static class Parser {
		public static Config Parse(string path) {
			IImmutableList<IniSection> tree = Sobbs.Config.Ini.Parser.Parse(path);
			return new Config(tree.Map(sec => ToWindowSection(sec)));
		}

		static WindowConfig ToWindowSection(IniSection sec) {
			var left = Size.Parse(sec["left"]);
			var top = Size.Parse(sec["top"]);
			var width = Size.Parse(sec["width"]);
			var height = Size.Parse(sec["height"]);
			return new WindowConfig(sec.Name, left, top, width, height);
		}
	}
}
