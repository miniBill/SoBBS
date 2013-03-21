using System;
using System.Collections.Generic;
using Sobbs.Data.List;

namespace Sobbs.Config.Ini {
	public static class Parser {
		public static IImmutableList<IniSection> Parse(string path) {
			var sections = ImmutableList<IniSection>.Empty;
			var values = ImmutableList<KeyValuePair<string, string>>.Empty;
			string sectionName = null;
			using(var file = System.IO.File.OpenText(path)) {
				int lineNumber = 0;
				while(!file.EndOfStream) {
					lineNumber++;
					string line = file.ReadLine();
					int commentIndex = line.IndexOf('#');
					var decommented = commentIndex >= 0 ? line.Substring(commentIndex) : line;
					var trimmed = decommented.Trim();

					if(trimmed.Length == 0)
						continue;

					int equalIndex = trimmed.IndexOf('=');
					if(equalIndex > 0) { //Entry
						if(sectionName == null)
							throw new ParserException(lineNumber, "got an entry before first section header", line);
						string left = trimmed.Substring(0, equalIndex).Trim();
						string right = trimmed.Substring(equalIndex + 1).Trim();
						if(left.Length == 0)
							throw new ParserException(lineNumber, "left of '=' was empty", line);
						if(right.Length == 0)
							throw new ParserException(lineNumber, "right of '=' was empty", line);
						values = values.Add(new KeyValuePair<string, string>(left, right));
						continue;
					}
					if(trimmed[0] == '[' && trimmed[trimmed.Length - 1] == ']') {
						string newSectionName = trimmed.Substring(1, trimmed.Length - 2);
						if(sectionName != null)
							sections = sections.Add(new IniSection(sectionName, values));

						values = ImmutableList<KeyValuePair<string, string>>.Empty;
						sectionName = newSectionName;
					}
				}
			}
			return sections;
		}
	}
}
