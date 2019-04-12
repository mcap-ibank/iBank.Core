using iBank.Core.Folders;

using PCLExt.FileStorage;

using System;
using System.Data.Common;
using System.Net;
using System.Net.Sockets;

namespace iBank.Core.Files
{
    public class ConfigJsonFile : JsonFile
    {
        #region Main SQL Database

        private string _SQL_MachineName = "BANK-MS";
        public string SQL_MachineName { get => _SQL_MachineName; set => SetValueIfChangedAndSave(ref _SQL_MachineName, value); }

        private string[] _SQL_Endpoints = new[] { "192.168.137.1", "10.0.0.100" };
        public string[] SQL_Endpoints { get => _SQL_Endpoints; set => SetValueIfChangedAndSave(ref _SQL_Endpoints, value); }

        private ushort _SQL_Port = 1435;
        public ushort SQL_Port { get => _SQL_Port; set => SetValueIfChangedAndSave(ref _SQL_Port, value); }

        private string _SQL_Database = "UNKNOWN";
        public string SQL_Database { get => _SQL_Database; set => SetValueIfChangedAndSave(ref _SQL_Database, value); }

        private string _SQL_User = "UNKNOWN";
        public string SQL_User { get => _SQL_User; set => SetValueIfChangedAndSave(ref _SQL_User, value); }

        private string _SQL_Password = "UNKNOWN";
        public string SQL_Password { get => _SQL_Password; set => SetValueIfChangedAndSave(ref _SQL_Password, value); }

        private string _SQL_ExtraArgs = "Connection Timeout=3";
        public string SQL_ExtraArgs { get => _SQL_ExtraArgs; set => SetValueIfChangedAndSave(ref _SQL_ExtraArgs, value); }

        #endregion

        #region Bank MS Access Database

        private string _Bank_Provider_MachineName = "UNKNOWN";
        public string Bank_Provider_MachineName { get => _Bank_Provider_MachineName; set => SetValueIfChangedAndSave(ref _Bank_Provider_MachineName, value); }

        private string[] _Bank_Provider_Endpoints = new string[] { };
        public string[] Bank_Provider_Endpoints { get => _Bank_Provider_Endpoints; set => SetValueIfChangedAndSave(ref _Bank_Provider_Endpoints, value); }

        private string _Bank_Provider_MS_Access_Provider = "Microsoft.ACE.OLEDB.15.0";
        public string Bank_Provider_MS_Access_Provider { get => _Bank_Provider_MS_Access_Provider; set => SetValueIfChangedAndSave(ref _Bank_Provider_MS_Access_Provider, value); }

        private string _Bank_Provider_MS_Access_Mode = "Read";
        public string Bank_Provider_MS_Access_Mode { get => _Bank_Provider_MS_Access_Mode; set => SetValueIfChangedAndSave(ref _Bank_Provider_MS_Access_Mode, value); }

        private string _Bank_Provider_MS_Access_File_Path = "directory/file.accdb";
        public string Bank_Provider_MS_Access_File_Path { get => _Bank_Provider_MS_Access_File_Path; set => SetValueIfChangedAndSave(ref _Bank_Provider_MS_Access_File_Path, value); }

        #endregion

        public ConfigJsonFile() : base(new ConfigFolder().CreateFile("Config.json", CreationCollisionOption.OpenIfExists)) { }

        public string GetMainSQLConnectionString()
        {
            string host = null;
            if (host == null && GetIPAddressByMachineName(SQL_MachineName) != null)
                host = SQL_MachineName;
            if (host == null)
                host = GetFirstValidIPAddress(SQL_Endpoints)?.ToString();
            if (host == null)
                throw new Exception("SQL Сервер недоступен!");

            var builder = new DbConnectionStringBuilder
            {
                { "Data Source", $"{host},{SQL_Port}" },
                { "Initial Catalog", SQL_Database },
                { "User ID", SQL_User },
                { "Password", SQL_Password }
            };
            return $"{builder.ConnectionString}; {SQL_ExtraArgs}";
        }

        public string GetBankProviderConnectionString()
        {
            string host = null;
            if (host == null && GetIPAddressByMachineName(Bank_Provider_MachineName) != null)
                host = Bank_Provider_MachineName;
            if (host == null)
                host = GetFirstValidIPAddress(Bank_Provider_Endpoints)?.ToString();
            if (host == null)
                throw new Exception("Не удалось найти файл!!");

            var builder = new DbConnectionStringBuilder
            {
                { "Provider", Bank_Provider_MS_Access_Provider },
                { "Data Source", $"{host}\\{Bank_Provider_MS_Access_File_Path}" },
                { "Mode", _Bank_Provider_MS_Access_Mode },
            };
            return $"{builder.ConnectionString}; {SQL_ExtraArgs}";
        }

        private IPAddress GetIPAddressByMachineName(string machineName)
        {
            try
            {
                var hostEntry = Dns.GetHostEntry(machineName);
                return Array.Find(hostEntry.AddressList, ip => ip.AddressFamily == AddressFamily.InterNetwork);
            }
            catch(Exception ex) when(ex is SocketException)
            {
                return null;
            }
        }

        private IPAddress GetFirstValidIPAddress(string[] endPoints)
        {
            foreach(var endpoint in endPoints)
            {
                if(IPAddress.TryParse(endpoint, out var ipAddress))
                {
                    try
                    {
                        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                        {
                            ReceiveTimeout = 500,
                            SendTimeout = 500
                        };
                        var result = socket.BeginConnect(new IPEndPoint(ipAddress, SQL_Port), null, null);
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
