using System;
using System.Collections.Generic;

namespace Sobbs.Config.Ini
{
	public static class Parser
	{
		public static IniTree Parse (string path)
		{
			List<IniSection> sections = new List<IniSection> ();
			Dictionary<string, string> values = new Dictionary<string, string> ();
			string sectionName = null;
			using (var file = System.IO.File.OpenText(path)) {
				int lineNumber = 0;
				while (!file.EndOfStream) {
					lineNumber++;
					string line = file.ReadLine ();
					string decommented = line.Substring (line.IndexOf ('#')).Trim ();

					if (decommented.Length == 0)
						continue;

					int equalIndex = decommented.IndexOf ('=');
					if (equalIndex > 0) { //Entry
						if (sectionName == null)
							throw new ParserException (lineNumber, "got an entry before first section header", line);
						string left = decommented.Substring (0, equalIndex).Trim ();
						string right = decommented.Substring (equalIndex + 1).Trim ();
						if (left.Length == 0)
							throw new ParserException (lineNumber, "left of '=' was empty", line);
						if (right.Length == 0)
							throw new ParserException (lineNumber, "right of '=' was empty", line);
						values.Add (left, right);
						continue;
					}
					if (decommented [0] == '[' && decommented [decommented.Length - 1] == ']') {
						string newSectionName = decommented.Substring (1, decommented.Length - 2);
						if (sectionName != null)
							sections.Add (new IniSection (sectionName, values));

						values = new Dictionary<string, string> ();
						sectionName = newSectionName;
					}
				}
			}
			return new IniTree (sections);
		}
	}
}
