using System;
using System.IO;
using Mono.Terminal;
using Sobbs.Config.Windows;

namespace Sobbs {
	class MainClass {
		public static void Main(string[] args) {
			var home = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var path = Path.Combine(home, ".sobbs");
			var conf = Parser.Parse(path);

			Application.Init(false);

			var container = new Frame(0, 0, Application.Cols, Application.Lines, "SoBBS");
			var zones = CreateContainer(conf["zones"], "Zones", Application.Cols - 2, Application.Lines - 2);
			container.Add(zones);
			var threads = CreateContainer(conf["threads"], "Threads", Application.Cols - 2, Application.Lines - 2);
			container.Add(threads);
			var messages = CreateContainer(conf["messages"], "Messages", Application.Cols - 2, Application.Lines - 2);
			container.Add(messages);

			Application.Run(container);
		}

		private	static Frame CreateContainer(WindowConfig conf, string name, int width, int height) {
			int x = conf.Left.Either<int>(val => val, perc => (int)Math.Round(width * perc.Value / 100.0), (star) => 0);
			int y = conf.Top.Either<int>(val => val, perc => (int)Math.Round(height * perc.Value / 100.0), (star) => 0);
			int w = conf.Width.Either<int>(val => val, perc => (int)Math.Round(width * perc.Value / 100.0), (star) => width - x);
			int h = conf.Height.Either<int>(val => val, perc => (int)Math.Round(height * perc.Value / 100.0), (star) => height - y);
			return new Frame(x + 1, y + 1, w, h, name);
		}
	}
}
