using System;
using System.Threading;
using System.Threading.Tasks;
using CursesSharp;
using Sobbs.Config.Windows;
using System.IO;
using Sobbs.Cui;
using Sobbs.Log;
using System.Linq;
using Sobbs.Functional;

namespace Sobbs
{
    public static class MainClass
    {
        private static void CreateWindow(WindowConfig config, IApplication application)
        {
            var info = new FrameInfo(config.Left, config.Top, config.Height, config.Width, config.Name);
            //application.MainContainer.Add(info);
        }

        public static void Main()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var loop = new EventLoop(cancellationTokenSource.Token);

            loop.EnqueueLoop(() => Logger.Log(LogLevel.Debug, "--Beat--"), period: 1/*000*/);

            WindowsConfig config = LoadConfig();
            var maybeZones = config["zones"];
            var maybeThreads = config["threads"];
            var maybeMessages = config["messages"];

            using (IApplication application = new CursesApplication())
            {
                var creator = ((Action<WindowConfig, IApplication>)CreateWindow).Curry(application);
                maybeZones.Concat(maybeThreads).Concat(maybeMessages).ForEach(creator);

                loop.Enqueue(async () =>
                    {
                        await Task.Delay(4000);
                        cancellationTokenSource.Cancel();
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
