using iBank.Core.Files;

using System.Data.Odbc;
using System.Data.SqlClient;

namespace iBank.Core
{
    public static class ConnectionManager
    {
        public static string MSSQL_ConnectionString => new ConfigJsonFile().GetMainSQLConnectionString();

#if KEEPSQLOPEN
        public static SqlConnection Connection { get; set; } = new SqlConnection(new ConfigJsonFile().GetConnectionString());

        static ConnectionManager()
        {
            // Нам не нужно держать соединение постоянно открытым, открытие\закрытие очнь быстрое
            if (!ConnectionManager.Connection.TryOpen())
                MessageBox.Show("Не удалось подключиться к БД!", "Ошибка!");
            ConnectionManager.Connection.StateChange += Connection_StateChange;
        }

        private void Connection_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            if (e.CurrentState == System.Data.ConnectionState.Closed)
            {
            reconnect:
                if (!ConnectionManager.Connection.TryOpen())
                {
                    switch(MessageBox.Show("Не удалось переподключиться к БД!", "Ошибка!", MessageBoxButton.YesNo))
                    {
                        case MessageBoxResult.Yes:
                            goto reconnect;
                    }
                }
            }
        }
#else
        public static SqlConnection MSSQL_Connection => new SqlConnection(MSSQL_ConnectionString);
#endif

        public static string MSAccess_ConnectionString => new ConfigJsonFile().GetBankProviderConnectionString();
        public static OdbcConnection MSAccess_Connection => new OdbcConnection(MSAccess_ConnectionString);
    }
}