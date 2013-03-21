using System;
using Mono.Terminal;
using Sobbs.Config.Windows;

namespace Sobbs {
	class MainClass {
		public static void Main(string[] args) {
			var path = "/home/leonardo/.sobbs";
			var conf = Parser.Parse(path);

			var container = new Frame("SoBBS");
			var zones = CreateContainer(conf["zones"], Application.Cols, Application.Lines);
			container.Add(zones);

			Application.Run(container);
		}

		private	static Frame CreateContainer(WindowConfig conf, int width, int height) {
			int left = conf.Left.Either<int>(val => val, perc => (int)Math.Floor(perc.Value), (star) => 0);
			throw new NotImplementedException();
		}		

	}
}
