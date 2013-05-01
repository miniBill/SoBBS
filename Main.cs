using System;
using System.IO;
using Mono.Terminal;
using Sobbs.Config.Windows;
using Sobbs.Functional;

namespace Sobbs
{
    public static class MainClass
    {
        public static void Main()
        {
            var home = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(home, ".sobbs");
            if (!File.Exists(path))
                CreateDefaultConfig(path);
            var conf = Parser.Parse(path);

            try
            {
                Application.Init(false);

                var container = new Frame(0, 0, Application.Cols, Application.Lines, "SoBBS");

                var zoneConfig = conf["zones"];
                var threadsConfig = conf["threads"];
                var messagesConfig = conf["messages"];

                var zones = CreateContainer(zoneConfig, "Zones", Application.Cols - 2, Application.Lines - 2);
                container.Add(zones);
                var threads = CreateContainer(threadsConfig, "Threads", Application.Cols - 2, Application.Lines - 2);
                container.Add(threads);
                var messages = CreateContainer(messagesConfig, "Messages", Application.Cols - 2, Application.Lines - 2);
                container.Add(messages);

                Application.Run(container);
            }
            catch (IndexOutOfRangeException e)
            {
                Application.Stop();
                Console.Error.WriteLine("Cannot find all the required windows configurations:");
                Console.Error.WriteLine(e.Message);
            }
            finally
            {
                Application.Stop();
            }
        }

        private static void CreateDefaultConfig(string path)
        {
            using (var file = File.Open(path, FileMode.Create))
            using (var writer = new StreamWriter(file))
            {
                writer.WriteLine("[zones]");
                writer.WriteLine("top   =  0");
                writer.WriteLine("left  =  0");
                writer.WriteLine("width = 30%");
                writer.WriteLine("height=  *");
                writer.WriteLine("[threads]");
                writer.WriteLine("top   =  0");
                writer.WriteLine("left  = 30%");
                writer.WriteLine("width =  *");
                writer.WriteLine("height= 50%");
                writer.WriteLine("[messages]");
                writer.WriteLine("top   = 50%");
                writer.WriteLine("left  = 30%");
                writer.WriteLine("width =  *");
                writer.WriteLine("height=  *");
            }
        }

        private static Frame CreateContainer(WindowConfig conf, string name, int width, int height)
        {
            Func<int, int> id = FuncExtensions.Identity<int>();
            Func<Star, int> zero = FuncExtensions.Constant<Star, int>(0);
            Func<Percent, int> widthPercent = perc => (int)Math.Round(width * perc.Value / 100.0);
            Func<Percent, int> heightPercent = perc => (int)Math.Round(height * perc.Value / 100.0);

            int x = conf.Left.Either(id, widthPercent, zero);
            int y = conf.Top.Either(id, heightPercent, zero);
            int w = conf.Width.Either(id, widthPercent, star => width - x);
            int h = conf.Height.Either(id, heightPercent, star => height - y);

            return new Frame(x + 1, y + 1, w, h, name);
        }
    }
}