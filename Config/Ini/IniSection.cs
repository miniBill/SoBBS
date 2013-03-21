using System;
using System.Collections.Generic;
using Sobbs.Data.List;

namespace Sobbs.Config.Ini {
	public class IniSection {
		public string this[string key] {
			get {
				return Values.Lookup( key);
			}
		}

		public string Name { 
			get;
			private set;
		}

		public IImmutableList<KeyValuePair<string,string>> Values {
			get;
			private set;
		}

		public IniSection(string name, IImmutableList<KeyValuePair<string, string>> values) {
			Name = name;
			Values = values;
		}
	}
}
