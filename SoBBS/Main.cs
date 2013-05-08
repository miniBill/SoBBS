using System;
using Microsoft.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Sobbs.Config.Windows;
using System.IO;
using Sobbs.Log;
using System.Linq;
using Sobbs.Functional;
using Sobbs.Cui;
using Sobbs.Cui.Curses;

namespace Sobbs
{
    public static class MainClass
    {
        public static void CreateWindow(WindowConfig config, IApplication application)
        {
            var frame = application.WidgetFactory.CreateFrame(config);
        }

        public static void Main()
        {
            var loop = new EventLoop();
            loop.Start();

            loop.EnqueueLoop(() => Logger.Log(LogLevel.Debug, "--Beat--"), period: 1000);

            WindowsConfig config = LoadConfig();
            var maybeZones = config ["zones"];
            var maybeThreads = config ["threads"];
            var maybeMessages = config ["messages"];

            IApplication application = new CursesApplication();
            IWidgetFactory factory = application.WidgetFactory;

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
