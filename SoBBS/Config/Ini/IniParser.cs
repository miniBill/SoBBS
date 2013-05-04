using System;
using System.Collections.Generic;
using Sobbs.Functional.Data.List;

namespace Sobbs.Config.Ini
{
    public static class IniParser
    {
        public static IImmutableList<IniSection> Parse(string path)
        {
            var sections = ImmutableList<IniSection>.Empty;
            var values = ImmutableList<KeyValuePair<string, string>>.Empty;
            string sectionName = null;
            using (var file = System.IO.File.OpenText(path))
            {
                int lineNumber = 0;
                while (!file.EndOfStream)
                {
                    lineNumber++;
                    string line = file.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    int commentIndex = line.IndexOf('#');
                    var decommented = commentIndex >= 0 ? line.Substring(commentIndex) : line;
                    var trimmed = decommented.Trim();

                    if (trimmed.Length == 0)
                        continue;

                    if (trimmed.Contains("=")) //Entry
                    {
                        try
                        {
                            if (sectionName == null)
                                throw new InternalIniParserException("got an entry before first section header");
                            var newEntry = ReadEntry(trimmed);
                            values = values.Add(newEntry);
                        }
                        catch (InternalIniParserException e)
                        {
                            throw new IniParserException(lineNumber, e.Message, line);
                        }
                        continue;
                    }
                    if (trimmed[0] == '[' && trimmed[trimmed.Length - 1] == ']') //Section
                    {
                        string newSectionName = trimmed.Substring(1, trimmed.Length - 2);
                        if (sectionName != null)
                            sections = sections.Add(new IniSection(sectionName, values));

                        values = ImmutableList<KeyValuePair<string, string>>.Empty;
                        sectionName = newSectionName;
                    }
                }
            }
            if (!values.IsEmpty)
            {
                if (sectionName == null)
                    throw new IniParserException(-1, "Something is wrong in your config file", "???");
                sections = sections.Add(new IniSection(sectionName, values));
            }
            else
                throw new IniParserException(-1, "Something is wrong in your config file, is it empty?", "???");
            return sections;
        }

        private static KeyValuePair<string, string> ReadEntry(string trimmed)
        {
            int equalIndex = trimmed.IndexOf('=');
            string left = trimmed.Substring(0, equalIndex).Trim();
            string right = trimmed.Substring(equalIndex + 1).Trim();
            if (left.Length == 0)
                throw new InternalIniParserException("left of '=' was empty");
            if (right.Length == 0)
                throw new InternalIniParserException("right of '=' was empty");
            return new KeyValuePair<string, string>(left, right);
        }
    }

    class InternalIniParserException : Exception
    {
        public InternalIniParserException(string message)
            : base(message)
        {
        }
    }
}
