using System;
using System.Threading;
using Sobbs.Config.Windows;
using System.IO;
using System.Linq;
using Sobbs.Cui;
using Sobbs.Cui.Interfaces;
using Sobbs.Functional;
using Sobbs.Cui.Widgets;
using MinCurses;

namespace Sobbs
{
    public static class MainClass
    {
        private static void CreateWindow(WindowConfig config, IApplication application)
        {
            var frame = new Frame(config.Left, config.Top, config.Width, config.Height, config.Name);
            application.MainContainer.Add(frame);
        }

        public static void Main()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var loop = new EventLoop(cancellationTokenSource.Token);

            WindowsConfig config = LoadConfig();
            var maybeZones = config["Zones"];
            var maybeThreads = config["Threads"];
            var maybeMessages = config["Messages"];

            using (IApplication application = new Application("SoBBS"))
            {
                var creator = ((Action<WindowConfig, IApplication>)CreateWindow).Curry(application);
                maybeZones.Concat(maybeThreads).Concat(maybeMessages).ForEach(creator);

                loop.EnqueueLoop(application.Refresh);

                loop.EnqueueCancelableLoop(() =>
                    {
                        if (Curses.GetChar() == 'q')
                        {
                            cancellationTokenSource.Cancel();
                            return false;
                        }
                        return true;
                    });

                loop.Join();
            }
        }

        private static WindowsConfig LoadConfig()
        {
            var home = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(home, ".sobbs");
            if (!File.Exists(path))
                WindowsConfigParser.CreateDefaultConfig(path);
            return WindowsConfigParser.Parse(path);
        }
    }
}
