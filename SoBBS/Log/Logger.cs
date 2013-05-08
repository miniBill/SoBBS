using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace Sobbs.Log
{
    public static class Logger
    {
        private static readonly List<ILogProvider> Providers = new List<ILogProvider>();

        static Logger()
        {
            //Providers.Add(new TcpClientLog(33123));
            Providers.Add(new TraceLog());
        }

        public static void Log(LogLevel level, string message)
        {
            DateTime now = DateTime.Now;
            foreach (var provider in Providers)
                provider.Log(level, Format(now) + message);
        }

        static string Format(DateTime time)
        {
            return time.Hour.ToString("D2") + time.Minute.ToString("D2")
                + time.Second.ToString("D2") + "." + time.Millisecond.ToString("D4");
        }
    }

    public class TraceLog : ILogProvider
    {
        public void Log(LogLevel level, string message)
        {
            Debug.WriteLine(level + ": " + message);
        }
    }
}
