using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace MssqlDatabaseAdminPanel.Models {
    public static class DbAnalyzer {
        private static List<string> ExecuteSelect(string sqlText) {
            var result = new List<string>();
            var connection = DbConnection.Connection;
            try {
                connection.Open();
                var command = new SqlCommand(sqlText, connection);
                command.CommandTimeout = 150;
                var dataReader = command.ExecuteReader();
                while (dataReader.Read()) {
                    result.Add(dataReader.GetValue(0).ToString());
                }
                dataReader.Close();
                command.Dispose();
                connection.Close();
            } catch (Exception ex) {
                result.Clear();
                result.Add(ex.Message);
            } finally {
                connection.Close();
            }
            return result;
        }

        private static List<DbIndex> ExecuteSelectForIndex(string sqlText, bool useName = false) {
            var result = new List<DbIndex>();
            var connection = DbConnection.Connection;
            try {
                connection.Open();
                var command = new SqlCommand(sqlText, connection);
                command.CommandTimeout = 150;
                var dataReader = command.ExecuteReader();
                while (dataReader.Read()) {
                    if (!useName)
                        result.Add(new DbIndex {
                            TableName = dataReader.GetValue(0).ToString(),
                            EqualityColumns = dataReader.GetValue(1).ToString(),
                            InequalityColumns = dataReader.GetValue(2).ToString(),
                            IncludedColumns = dataReader.GetValue(3).ToString()
                        });
                    else
                        result.Add(new DbIndex {
                            TableName = dataReader.GetValue(0).ToString(),
                            Name = dataReader.GetValue(1).ToString()
                        });
                }
                dataReader.Close();
                command.Dispose();
            } catch (Exception ex) {
                result.Clear();
            } finally {
                connection.Close();
            }
            return result;
        } 

        public static List<string> GetMostExpensiveRequest() {
            var sql = File.ReadAllText("./wwwroot/sql/MostExpensiveRequest.sql");
            return ExecuteSelect(sql);
        }

        public static List<string> GetReadableTables() {
            var sql = File.ReadAllText("./wwwroot/sql/ReadableTables.sql");
            return ExecuteSelect(sql);
        }

        public static List<string> GetWritableTables() {
            var sql = File.ReadAllText("./wwwroot/sql/WritableTables.sql");
            return ExecuteSelect(sql);
        }

        public static List<DbIndex> GetRequiredIndexes() {
            var sql = File.ReadAllText("./wwwroot/sql/RequiredIndexes.sql");
            return ExecuteSelectForIndex(sql);
        }

        public static List<DbIndex> GetUnusedIndexes() {
            var sql = File.ReadAllText("./wwwroot/sql/UnusedIndexes.sql");
            return ExecuteSelectForIndex(sql, true);
        }

        public static List<DbIndex> GetMostExpensiveIndexes() {
            var sql = File.ReadAllText("./wwwroot/sql/MostExpensiveIndexes.sql");
            return ExecuteSelectForIndex(sql, true);
        }

        public static string GetAlterIndexesSql() {
            var sql = File.ReadAllText("./wwwroot/sql/AlterIndexes.sql");
            return string.Join(";\n", ExecuteSelect(sql));
        }
    }
}
