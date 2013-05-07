using System;
using Microsoft.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Sobbs.Config.Windows;
using System.IO;

namespace Sobbs
{
    public static class MainClass
    {
        public static void Main()
        {
            WindowsConfig config = LoadConfig();
            var zones = config["zones"];

            var threads = config["threads"];

            var messages = config["messages"];

            var loop = new EventLoop();
            loop.Start();

            // Do androids dream of electric sheep?
            Thread.Sleep(Timeout.Infinite);
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
