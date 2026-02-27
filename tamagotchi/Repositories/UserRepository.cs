using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using tamagotchi.Data;
using tamagotchi.Models;

namespace tamagotchi.Repositories
{
    public interface IUserRepository
    {
        Task<ObservableCollection<User>> GetAll();
        Task<int> CreateAsync(string name);

    }

    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _factory;


        public UserRepository(IDbConnectionFactory factory) 
        {
            _factory = factory;
        }

        public async Task<ObservableCollection<User>> GetAll()
        {
            await using DbConnection db = _factory.Create();
            await db.OpenAsync();


            await using DbCommand cmd = db.CreateCommand();
            cmd.CommandText = "SELECT * FROM users;";

            ObservableCollection<User> users = new ObservableCollection<User>();
            await using DbDataReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Debug.WriteLine($"Firstname: {reader["name"]}");
                users.Add(new User() { Id = Convert.ToInt32(reader["id"]), Name = (string)reader["name"], CreatedAt = (string)reader["created_at"], UpdatedAt = (string)reader["updated_at"] });
            }
            return users;
        }

        public async Task<int> CreateAsync(string name)
        {
            await using DbConnection db = _factory.Create();
            await db.OpenAsync();

            await using DbCommand insertCmd = db.CreateCommand();
            insertCmd.CommandText = "INSERT INTO users (name) VALUES (@name);";
            AddParameter(insertCmd, "@name", name);
            await insertCmd.ExecuteNonQueryAsync();

            await using DbCommand idCmd = db.CreateCommand();
            idCmd.CommandText = "SELECT last_insert_rowid();";
            var result = await idCmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        private static void AddParameter(DbCommand cmd, string name, object value)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = value;
            cmd.Parameters.Add(param);
        }
    }
}
