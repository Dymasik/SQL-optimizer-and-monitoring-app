using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MssqlDatabaseAdminPanel.Models {
    public static class DbConnection {
        private static string _connectionString;
        private static SqlConnection _connection;

        public static SqlConnection Connection { get => _connection; }

        public static void SetConnectionString(int type, string serverName, string dbName, string userName, string password) {
            if (type == (int) AuthTypeEnum.SERVER) {
                _connectionString = $"Data Source={serverName};Initial Catalog={dbName};User ID={userName};Password={password}";
            } else {
                _connectionString = $"Server={serverName}; Database={dbName};Integrated Security = SSPI;"; 
            }
        }

        public static bool EstablishConnection() {
            _connection = new SqlConnection(_connectionString);
            try {
                _connection.Open();
                _connection.Close();
                return true;
            } catch (Exception e) {
                return false;
            }
        }
    }
}
