using System;

namespace Sobbs.Config.Ini
{
    class IniParserException : Exception
    {
        public IniParserException(int lineNumber, string error, string line)
            : base("Parse error at line " + lineNumber + ": " + error + Environment.NewLine + "Line was \"" + line + "\"")
        {
        }
    }

}
