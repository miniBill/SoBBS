using System;
using System.Net.Sockets;
using System.IO;

namespace Sobbs.Log
{
    class TcpClientLog : ILogProvider, IDisposable
    {
        private readonly TcpClient _client;
        private readonly StreamWriter _writer;
        private bool _disposed;

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
            if (_writer == null)
                return;
            _writer.WriteLine(level.ToString() + ": " + message);
            _writer.Flush();
        }


        ~TcpClientLog()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _client.Close();
                _writer.Close();
            }

            _disposed = true;
        }
    }
}
