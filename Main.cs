using System;

namespace Sobbs
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var conf = Config.Windows.Parser.Parse("~/.sobbs");
			Console.ReadKey();
		}
	}
}
