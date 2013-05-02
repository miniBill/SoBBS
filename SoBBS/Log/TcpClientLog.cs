using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;

namespace Sobbs.Log
{
    class TcpClientLog : ILogProvider
    {
        private TcpClient _client;
        private StreamWriter _writer;

        public TcpClientLog(int port)
        {
            try
            {
                _client = new TcpClient("localhost", port);
                _writer = new StreamWriter(_client.GetStream());
            }
            catch (SocketException)
            {
            }
        }

        public void Log(LogLevel level, string message)
        {
            if(_writer == null)
                return;
            _writer.WriteLine(level.ToString() + ": " + message);
            _writer.Flush();
        }
    }
}
