using System;
using System.Collections.Generic;

namespace Sobbs.Config.Ini {
	class ParserException : Exception {
		public ParserException(int lineNumber, string error, string line) 
			: base("Parse error at line " + lineNumber + ": " + error + Environment.NewLine + "Line was \"" + line + "\"") {
		}
	}

}
