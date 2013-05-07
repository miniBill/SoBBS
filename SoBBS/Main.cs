using System;
using Microsoft.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Sobbs.Config.Windows;
using System.IO;
using Sobbs.Log;

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

            loop.Enqueue(async delegate {
                for(;;){
                    Logger.Log(LogLevel.Debug, "Tick");
                    await Task.Delay(3000);
                }
            });

            Thread.Sleep(Timeout.Infinite); // Do androids dream of electric sheep?
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
