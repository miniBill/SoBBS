using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using System.Net;

namespace Sobbs.Log
{
    class TcpServerLog : ILogProvider
    {
        private readonly TcpListener _listener;
        private readonly List<TcpClient> _clients = new List<TcpClient>();

        public TcpServerLog(int port)
        {
            _listener = new TcpListener(IPAddress.Loopback, port);
            _listener.Start();
            _listener.BeginAcceptTcpClient(Accepted, null);
        }

        void Accepted(IAsyncResult ar)
        {
            TcpClient client = _listener.EndAcceptTcpClient(ar);
            _clients.Add(client);
        }

        public void Log(LogLevel level, string message)
        {
            _clients.RemoveAll(client => !client.Connected);
            foreach (var client in _clients)
            {
                var writer = new StreamWriter(client.GetStream());
                writer.WriteLine(level.ToString() + ": " + message);
            }
        }
    }
}
