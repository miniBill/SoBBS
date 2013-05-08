using System;
using System.Threading;
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
            application.MainContainer.Add(info);
        }

        public static void Main()
        {
            var loop = new EventLoop();
            loop.Start();

            loop.EnqueueLoop(() => Logger.Log(LogLevel.Debug, "--Beat--"), period: 1000);

            WindowsConfig config = LoadConfig();
            var maybeZones = config["zones"];
            var maybeThreads = config["threads"];
            var maybeMessages = config["messages"];

            IApplication application = new CursesApplication();

            var creator = ((Action<WindowConfig, IApplication>)CreateWindow).Curry(application);
            maybeZones.Concat(maybeThreads).Concat(maybeMessages).ForEach(creator);

            _mainThread = Thread.CurrentThread;

            Thread.Sleep(Timeout.Infinite); // Do androids dream of electric sheep?
        }

        private static Thread _mainThread;

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
