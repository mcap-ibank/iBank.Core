using System;
using System.Net;
using System.Net.Sockets;

namespace iBank.Core.Files
{
    public abstract class BaseConfigJsonFile : JsonFile
    {
        protected BaseConfigJsonFile(PCLExt.FileStorage.IFile file) : base(file) { }

        protected IPAddress GetIPAddressByMachineName(string machineName)
        {
            try
            {
                var hostEntry = Dns.GetHostEntry(machineName);
                return Array.Find(hostEntry.AddressList, ip => ip.AddressFamily == AddressFamily.InterNetwork);
            }
            catch (Exception ex) when (ex is SocketException)
            {
                return null;
            }
        }

        protected IPAddress GetFirstValidIPAddress(string[] endPoints, int port)
        {
            foreach (var endpoint in endPoints)
            {
                if (IPAddress.TryParse(endpoint, out var ipAddress))
                {
                    try
                    {
                        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                        {
                            ReceiveTimeout = 500,
                            SendTimeout = 500
                        };
                        var result = socket.BeginConnect(new IPEndPoint(ipAddress, port), null, null);
                        var success = result.AsyncWaitHandle.WaitOne(500, true);
                        if (!success)
                            socket.Close();
                        else
                        {
                            socket.Close();
                            return ipAddress;
                        }
                    }
                    catch (Exception ex) when (ex is SocketException)
                    {

                    }
                }
            }

            return null;
        }
    }
}