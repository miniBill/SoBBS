using System;
using Sobbs.Config.Ini;
using Sobbs.Data.List;

namespace Sobbs.Config.Windows {
	class SizeParseException : Exception {
		public SizeParseException(string message) :base(message) {
		}
	}
}
