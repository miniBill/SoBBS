using System;
using System.Threading;
using CursesSharp;
using Sobbs.Config.Windows;
using System.IO;
using System.Linq;
using Sobbs.Cui.Curses;
using Sobbs.Cui.Info;
using Sobbs.Cui.Interfaces;
using Sobbs.Functional;
using Sobbs.Loop;

namespace Sobbs
{
    public static class MainClass
    {
        private static void CreateWindow(WindowConfig config, IApplication application)
        {
            var info = new FrameInfo(config.Left, config.Top, config.Width, config.Height, config.Name);
            application.MainContainer.Add(info);
        }

        public static void Main()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var loop = new EventLoop(cancellationTokenSource.Token);

            WindowsConfig config = LoadConfig();
            var maybeZones = config["Zones"];
            var maybeThreads = config["Threads"];
            var maybeMessages = config["Messages"];

            using (IApplication application = new CursesApplication("SoBBS"))
            {
                var creator = ((Action<WindowConfig, IApplication>)CreateWindow).Curry(application);
                maybeZones.Concat(maybeThreads).Concat(maybeMessages).ForEach(creator);

                loop.EnqueueLoop(application.Refresh);

                loop.EnqueueCancelableLoop(() =>
                    {
                        if (Curses.StdScr.GetChar() == 'q')
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
