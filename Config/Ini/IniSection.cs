using System;
using System.Collections.Generic;
using Sobbs.Data.List;

namespace Sobbs.Config.Ini {
	public class IniSection {
		public string this[string key] {
			get {
				return Lookup(Values, key);
			}
		}

		private static string Lookup(IImmutableList<KeyValuePair<string, string>> values, string key) {
			if(values.IsEmpty)
				throw new IndexOutOfRangeException("Key " + key + " not found");

			if(values.Value.Key == key)
				return values.Value.Value;

			return Lookup(values.Tail, key);
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
