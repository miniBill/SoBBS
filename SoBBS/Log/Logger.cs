using System.Collections.Generic;
using System;

namespace Sobbs.Log
{
    public static class Logger
    {
        private static readonly List<ILogProvider> Providers = new List<ILogProvider>();

        static Logger()
        {
            Providers.Add(new TcpClientLog(33123));
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
}
