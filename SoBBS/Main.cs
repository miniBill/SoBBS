using System;
using System.IO;
using System.Linq;
using Mono.Terminal;
using Sobbs.Config.Sizes;
using Sobbs.Config.Windows;
using Sobbs.Functional;
using Sobbs.Widgets;
using Sobbs.Log;
using System.Threading;

namespace Sobbs
{
    public static class MainClass
    {
        public static void Main()
        {
            var home = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(home, ".sobbs");
            if (!File.Exists(path))
                WindowsConfigParser.CreateDefaultConfig(path);
            var conf = WindowsConfigParser.Parse(path);

            try
            {
                bool refreshing = true;
                SoFrame container = InitCUI(conf);
                System.Action refresher = (()=>
                {
                    for(long i = 0; refreshing; i++)
                    {
                        if(i % 100 == 0) // Approximatively once per second
                        {
                            container.ForEach(w => 
                            {
                                var frame = w as SoFrame;
                                if(frame != null)
                                    frame.Update();
                            });
                        }
                        Thread.Sleep(10);
                        Curses.refresh();
                    }
                });
                var invocation = refresher.BeginInvoke(null, null);
                Application.Run(container);
                refreshing = false;
                refresher.EndInvoke(invocation);
            }
            catch (IndexOutOfRangeException e)
            {
                Application.Stop();
                Console.Error.WriteLine("Cannot find all the required windows configurations:");
                Console.Error.WriteLine(e.Message);
                Logger.Log(LogLevel.Info, "=== Application configuration error ===\n");
            }
            finally
            {
                Application.Stop();
                Logger.Log(LogLevel.Info, "=== Application end ===\n");
            }
        }

        private const string dbPath = "/home/public/sobbs";

        private static SoFrame InitCUI(WindowsConfig conf)
        {
            Application.Init(false);
            Logger.Log(LogLevel.Info, "=== Application start ===");
            var container = new SoFrame(0, 0, Application.Cols, Application.Lines, "SoBBS");

            SoFrame.KeyPressedEventHandler logHandler = (frame, eventArgs) => 
            {
                Logger.Log(LogLevel.Debug, frame.Title + ".OnProcessHotKey (" + (char)eventArgs.Key + ")");
                return false;
            };

            Func<string, SoFrame> create = (name) =>
            {
                var lowercase = name.ToLowerInvariant();
                var config = conf [lowercase];
                var width = Application.Cols - 2;
                var height = Application.Lines - 2;
                var frame = CreateContainer(config, name, width, height);
                container.Add(frame);
                frame.OnProcessHotKey += logHandler;
                var provider = new ListItemProvider();
                var listView = new ListView(-1, -1, frame.w - 2, frame.h - 2, provider);
                frame.Add(listView);
                return frame;
            };

            var zones = create("Zones");
            zones.OnUpdate += (sender, e) => 
            {
                var listView = zones.First() as ListView;
                var provider = listView.Provider as ListItemProvider;
                LoadItems(provider, dbPath);
                listView.ProviderChanged();
            };
            var threads = create("Threads");
            threads.OnUpdate += (sender, e) => 
            {
                var listView = zones.First() as ListView;
                var selectedIndex = listView.Selected;
                var provider = listView.Provider as ListItemProvider;
                IListItem selectedItem = provider[selectedIndex];
                var zone = selectedItem.ToString();
                var tlistView = threads.First() as ListView;
                var tprovider = tlistView.Provider as ListItemProvider;
                LoadItems(tprovider, Path.Combine(dbPath, zone));
                tlistView.ProviderChanged();
            };
            zones.OnProcessHotKey += (sender, e) => 
            {
                threads.Update();
                return false;
            };
            var messages = create("Messages");

            Application.Iteration += (sender, e) => Logger.Log(LogLevel.Debug, "* Application.Iteration\n");
            container.OnProcessHotKey += (frame, eventArgs) => 
            {
                if (eventArgs.Key == 'q')
                {
                    Application.Stop();
                    return true;
                }
                return false;
            };

            return container;
        }

        private static void LoadItems(ListItemProvider provider, string path)
        {
            provider.Clear();
            foreach(var dir in Directory.EnumerateDirectories(path))
                provider.Add(new StringItem(Path.GetFileName(dir)));
        }

        private static SoFrame CreateContainer(WindowConfig conf, string name, int width, int height)
        {
            Func<int, int> id = FuncExtensions.Identity<int>();
            Func<Star, int> zero = FuncExtensions.Constant<Star, int>(0);
            Func<Percent, int> widthPercent = perc => (int)Math.Round(width * perc.Value / 100.0);
            Func<Percent, int> heightPercent = perc => (int)Math.Round(height * perc.Value / 100.0);

            int x = conf.Left.Either(id, widthPercent, zero);
            int y = conf.Top.Either(id, heightPercent, zero);
            int w = conf.Width.Either(id, widthPercent, star => width - x);
            int h = conf.Height.Either(id, heightPercent, star => height - y);

            return new SoFrame(x + 1, y + 1, w, h, name);
        }
    }
}
