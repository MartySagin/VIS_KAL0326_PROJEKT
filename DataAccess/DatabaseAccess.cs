using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DataAccess.Interfaces;

namespace DataAccess
{
    public class DatabaseAccess : IDatabaseAccess
    {
        private readonly string _connectionString;

        public DatabaseAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string sql, object parameters = null) where T : class
        {
            using var connection = CreateConnection();

            return await connection.QueryAsync<T>(sql, parameters);
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, object parameters = null)
        {
            using var connection = CreateConnection();

            return await connection.ExecuteAsync(sql, parameters);
        }
    }
}
