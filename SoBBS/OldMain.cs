using System;
using System.IO;
using System.Linq;
using Mono.Terminal;
using Sobbs.Config.Sizes;
using Sobbs.Config.Windows;
using Sobbs.Cui;
#if __MONO_CS__
using Sobbs.Cui.Curses;
#else
using Sobbs.Cui.Forms;
#endif
using Sobbs.Functional;
using Sobbs.Functional.Data.Maybe;
using Sobbs.Log;
using System.Threading;

namespace Sobbs
{
    public static class OldMainClass
    {
        private class BoolWrapper
        {
            public bool Value
            {
                get;
                set;
            }
        }

        public static void OldMain()
        {
            /*

            try
            {
                var refreshing = new BoolWrapper { Value = true };
#if __MONO_CS__
                var factory = new CursesFactory();
                CursesFrame container = InitCUI(factory, conf);
#else
                var factory = new FormFactory();
                FormFrame container = InitCUI(factory, conf);
#endif
                Action<BoolWrapper> refresher = wrapper =>
                    {
                        for (long i = 0; wrapper.Value; i++)
                        {
                            if (i % 100 == 0) // Approximatively once per second
                            {
                                (from widget in container
                                 from frame in widget.MaybeCast<IFrame>()
                                 select frame).ForEach(frame => frame.UpdateData());
                            }
                            Thread.Sleep(10);
#if __MONO_CS__
                            Curses.refresh();
#endif
                        }
                    };
                var invocation = refresher.BeginInvoke(refreshing, null, null);
                SoApplication.Run(container);
                refreshing.Value = false;
                //refresher.EndInvoke(invocation);
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

*/
        }

#if __MONO_CS__
        private const string DbPath = "/home/public/sobbs";
#else
        private static readonly string DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SoBBS");
#endif

#if __MONO_CS__
        private static CursesFrame InitCUI(ICuiFactory factory, WindowsConfig conf)
#else
        private static FormFrame InitCUI(ICuiFactory factory, WindowsConfig conf)
#endif
        {
            SoApplication.Init();
            Logger.Log(LogLevel.Info, "=== Application start ===");
            var info = new FrameInfo(0, 0, SoApplication.Cols, SoApplication.Lines, "SoBBS");
#if __MONO_CS__
            var container = new CursesFrame(info);
#else
            var container = new FormFrame(info);
#endif

            KeyPressedEventHandler<IFrame> logHandler = (frame, eventArgs) =>
            {
                Logger.Log(LogLevel.Debug, frame.Title + ".OnProcessHotKey (" + (char)eventArgs.Key + ")");
                return false;
            };

            Func<string, IFrame> create = name =>
            {
                /*var lowercase = name.ToLowerInvariant();
                var config = (conf[lowercase] as Just<WindowConfig>).Value;
                var width = SoApplication.Cols - 2;
                var height = SoApplication.Lines - 2;
                var frameInfo = CreateContainer(config, name, width, height);
                var frame = container.Add(frameInfo);
                frame.OnProcessHotKey += logHandler;
                var provider = new ListItemProvider();
                var listViewInfo = new ListViewInfo(-1, -1, frame.W - 2, frame.H - 2, provider);
                frame.Add(listViewInfo);
                return frame;*/
                return null;
            };

            var zones = create("Zones");
            zones.OnUpdateData += (sender, e) =>
                {
                    var maybeCast = from listView in zones.First().MaybeCast<ListView>()
                                    from provider in listView.Provider.MaybeCast<ListItemProvider>()
                                    select new { ListView = listView, Provider = provider };
                    foreach (var result in maybeCast)
                    {
                        LoadItems(result.Provider, DbPath);
                        result.ListView.ProviderChanged();
                    }
                };
            var threads = create("Threads");
            threads.OnUpdateData += (sender, e) =>
                {
                    var maybeCast = from zonesList in zones.First().MaybeCast<ListView>()
                                    let selectedZoneIndex = zonesList.Selected
                                    from zonesProvider in zonesList.Provider.MaybeCast<ListItemProvider>()
                                    let selectedZone = zonesProvider[selectedZoneIndex]

                                    from threadsList in threads.First().MaybeCast<ListView>()
                                    from threadsProvider in threadsList.Provider.MaybeCast<ListItemProvider>()
                                    select new
                                        {
                                            ThreadsProvider = threadsProvider,
                                            ZoneName = selectedZone.ToString(),
                                            ThreadsList = threadsList
                                        };
                    foreach (var result in maybeCast)
                    {
                        LoadItems(result.ThreadsProvider, Path.Combine(DbPath, result.ZoneName));
                        result.ThreadsList.ProviderChanged();
                    }
                };
            zones.OnProcessHotKey += (sender, args) =>
                {
                    threads.UpdateData();
                    return false;
                };
            /*var messages =*/
            create("Messages");

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
            foreach (var dir in Directory.EnumerateDirectories(path))
                provider.Add(new StringItem(Path.GetFileName(dir)));
        }

        private static FrameInfo CreateContainer(WindowConfig conf, string name, int width, int height)
        {
            Func<int, int> id = FuncExtensions.Identity<int>();
            Func<Star, int> zero = FuncExtensions.Constant<Star, int>(0);
            Func<Percent, int> widthPercent = perc => (int)Math.Round(width * perc.Value / 100.0);
            Func<Percent, int> heightPercent = perc => (int)Math.Round(height * perc.Value / 100.0);

            int x = conf.Left.Either(id, widthPercent, zero);
            int y = conf.Top.Either(id, heightPercent, zero);
            int w = conf.Width.Either(id, widthPercent, star => width - x);
            int h = conf.Height.Either(id, heightPercent, star => height - y);

            return new FrameInfo(x + 1, y + 1, w, h, name);
        }
    }
}
