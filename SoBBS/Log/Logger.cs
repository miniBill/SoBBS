using System.Collections.Generic;

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
            foreach(var provider in Providers)
                provider.Log(level, message);
        }
    }
}
