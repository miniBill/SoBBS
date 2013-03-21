using System;

namespace Sobbs.Config.Windows
{
	public static class Parser
	{
		public static Config Parse (string path)
		{
			var tree = Sobbs.Config.Ini.Parser.Parse (path);
			throw new NotImplementedException ();
		}
	}
}
