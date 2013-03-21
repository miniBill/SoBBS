using System;

namespace Sobbs {
	class MainClass {
		public static void Main(string[] args) {
			var path = "/home/leonardo/.sobbs";
			var conf = Config.Windows.Parser.Parse(path);
			Console.ReadKey();
		}
	}
}
