using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace iBank.Core
{
    public class BankProviderClient : IDisposable
    {
        public Socket TcpClient { get; }
        public BankProviderClient(IPAddress ipAddress, ushort port)
        {
            TcpClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
            TcpClient.Connect(new IPEndPoint(ipAddress, port));
        }

        public string GetExecutedByDate(DateTime date)
        {
            var stream = new NetworkStream(TcpClient);
            using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                writer.Write((byte) 1);
                writer.Write(date.Ticks);
                writer.Flush();
                while (TcpClient.Available == 0) { Thread.Sleep(10); }
                return reader.ReadString();
            }
        }

        public string GetAll()
        {
            var stream = new NetworkStream(TcpClient);
            using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                writer.Write(2);
                writer.Flush();
                while (TcpClient.Available == 0) { Thread.Sleep(10); }
                return reader.ReadString();
            }
        }

        public void Dispose()
        {
            TcpClient?.Dispose();
        }
    }
}