using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;
using tamagotchi.Data;
using tamagotchi.Models;

namespace tamagotchi.Repositories
{
    public interface IPetRepository
    {
        Task<List<Pet>> GetAllByOwnerIdAsync(int ownerId);
        Task<Pet?> GetByIdAsync(int id);
        Task<int> CreateAsync(string name, int typeId, int statusId, int colorId, int ownerId, int stageId);
        Task UpdateStatsAsync(int id, int hunger, int sleepiness, int happiness);
        Task UpdateStatusAsync(int id, int statusId);
        Task UpdateStageAsync(int id, int stageId);
        Task DeleteAsync(int id);
    }

    public class PetRepository : IPetRepository
    {
        private readonly IDbConnectionFactory _factory;


        public PetRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }


        public async Task<List<Pet>> GetAllByOwnerIdAsync(int ownerId)
        {
            var pets = new List<Pet>();

            await using DbConnection db = _factory.Create();
            await db.OpenAsync();
            await using DbCommand cmd = db.CreateCommand();
            cmd.CommandText = "SELECT * FROM pet WHERE owner_id = @ownerId";

            AddParameter(cmd, "@ownerId", ownerId);

            await using DbDataReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                pets.Add(MapPet(reader));
            }

            return pets;
        }

        public async Task<Pet?> GetByIdAsync(int id)
        {
            await using DbConnection db = _factory.Create();
            await db.OpenAsync();
            await using DbCommand cmd = db.CreateCommand();
            cmd.CommandText = "SELECT * FROM pet WHERE id = @id";

            AddParameter(cmd, "@id", id);

            await using DbDataReader reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return MapPet(reader);

            return null;
        }

        public async Task<int> CreateAsync(string name, int typeId, int statusId, int colorId, int ownerId, int stageId)
        {
            await using DbConnection db = _factory.Create();
            await db.OpenAsync();

            await using DbCommand insertCmd = db.CreateCommand();
            insertCmd.CommandText = "INSERT INTO pet (name, type_id, status_id, color_id, owner_id, stage_id) VALUES (@name, @typeId, @statusId, @colorId, @ownerId, @stageId);";
            AddParameter(insertCmd, "@name", name);
            AddParameter(insertCmd, "@typeId", typeId);
            AddParameter(insertCmd, "@statusId", statusId);
            AddParameter(insertCmd, "@colorId", colorId);
            AddParameter(insertCmd, "@ownerId", ownerId);
            AddParameter(insertCmd, "@stageId", stageId);
            await insertCmd.ExecuteNonQueryAsync();

            await using DbCommand idCmd = db.CreateCommand();
            idCmd.CommandText = "SELECT last_insert_rowid();";
            var result = await idCmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }


        public async Task UpdateStatsAsync(int id, int hunger, int sleepiness, int happiness)
        {
            await using DbConnection db = _factory.Create();
            await db.OpenAsync();
            await using DbCommand cmd = db.CreateCommand();
            cmd.CommandText = @"
                UPDATE pet 
                SET hunger = @hunger, sleepiness = @sleepiness, happiness = @happiness, updated_at = datetime('now')
                WHERE id = @id";

            AddParameter(cmd, "@id", id);
            AddParameter(cmd, "@hunger", hunger);
            AddParameter(cmd, "@sleepiness", sleepiness);
            AddParameter(cmd, "@happiness", happiness);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateStatusAsync(int id, int statusId)
        {
            await using DbConnection db = _factory.Create();
            await db.OpenAsync();
            await using DbCommand cmd = db.CreateCommand();
            cmd.CommandText = "UPDATE pet SET status_id = @statusId, updated_at = datetime('now') WHERE id = @id";

            AddParameter(cmd, "@id", id);
            AddParameter(cmd, "@statusId", statusId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateStageAsync(int id, int stageId)
        {
            await using DbConnection db = _factory.Create();
            await db.OpenAsync();
            await using DbCommand cmd = db.CreateCommand();
            cmd.CommandText = "UPDATE pet SET stage_id = @stageId, updated_at = datetime('now') WHERE id = @id";

            AddParameter(cmd, "@id", id);
            AddParameter(cmd, "@stageId", stageId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await using DbConnection db = _factory.Create();
            await db.OpenAsync();
            await using DbCommand cmd = db.CreateCommand();
            cmd.CommandText = "DELETE FROM pet WHERE id = @id";

            AddParameter(cmd, "@id", id);

            await cmd.ExecuteNonQueryAsync();
        }

        private static Pet MapPet(DbDataReader reader)
        {
            return new Pet()
            {
                Id = Convert.ToInt32(reader["id"]),
                Name = reader["name"].ToString()!,
                TypeId = Convert.ToInt32(reader["type_id"]),
                ColorId = Convert.ToInt32(reader["color_id"]),
                StatusId = Convert.ToInt32(reader["status_id"]),
                StageId = Convert.ToInt32(reader["stage_id"]),
                OwnerId = Convert.ToInt32(reader["owner_id"]),
                Hunger = Convert.ToInt32(reader["hunger"]),
                Sleepiness = Convert.ToInt32(reader["sleepiness"]),
                Happiness = Convert.ToInt32(reader["happiness"]),
                CreatedAt = reader["created_at"].ToString()!,
                UpdatedAt = reader["updated_at"].ToString()!
            };
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
