using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace tamagotchi.Data
{
    public interface IDbConnectionFactory
    {
        DbConnection Create();
    }

    public sealed class SqliteConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqliteConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbConnection Create()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}
