using System;
using System.Collections.Generic;

namespace Sobbs.Log
{
    public static class Logger
    {
        private static readonly List<ILogProvider> _providers = new List<ILogProvider>();

        static Logger()
        {
            _providers.Add(new TcpClientLog(33123));
        }

        public static void Log(LogLevel level, string message)
        {
            foreach(var provider in _providers)
                provider.Log(level, message);
        }
    }
}
