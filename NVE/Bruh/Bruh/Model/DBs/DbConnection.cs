using Bruh.VMTools;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bruh.Model.DBs
{
    public class DbConnection
    {
        MySqlConnection _connection;

        static DbConnection dbConnection;
        private DbConnection() { }
        public static DbConnection GetDbConnection()
        {
            if (dbConnection == null)
                dbConnection = new DbConnection();
            return dbConnection;
        }

        public void Config()
        {
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.UserID = "student";
            sb.Password = "student";
            sb.Server = "192.168.200.13";
            sb.Database = "Bruhgalter";
            sb.CharacterSet = "utf8mb4";

            _connection = new MySqlConnection(sb.ToString());
        }

        public bool OpenConnection()
        {
            if (_connection == null)
                Config();

            return ExeptionHandler.Try(_connection.Open);
        }

        internal void CloseConnection()
        {
            if (_connection == null)
                return;

            ExeptionHandler.Try(_connection.Close);
        }

        internal MySqlCommand CreateCommand(string sql)
        {
            return new MySqlCommand(sql, _connection);
        }
    }
}
