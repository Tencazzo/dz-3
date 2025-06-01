using dz3.Logging;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace dz3.Data.IMigration
{
    public class MigrationManager
    {
        private readonly string connectionString;
        private readonly ILogger logger;

        public MigrationManager(string connectionString, ILogger logger)
        {
            this.connectionString = connectionString.Contains("Version")
                ? @"Data Source=TaskManagement.db;"
                : connectionString;
            this.logger = logger;
        }

        public void ApplyMigrations()
        {
            try
            {
                InitializeMigrationTable();

                var appliedMigrations = GetAppliedMigrations();
                var availableMigrations = GetAvailableMigrations()
                    .Where(m => !appliedMigrations.Contains(m.Version))
                    .OrderBy(m => m.Version)
                    .ToList();

                foreach (var migration in availableMigrations)
                {
                    logger.LogInfo($"Applying migration {migration.Version}: {migration.Description}");
                    ExecuteNonQuery(migration.Sql);
                    RecordMigration(migration);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error applying migrations", ex);
                throw;
            }
        }

        private void InitializeMigrationTable()
        {
            const string createTableSql = @"
                CREATE TABLE IF NOT EXISTS __MigrationHistory (
                    Version TEXT PRIMARY KEY,
                    Description TEXT NOT NULL,
                    AppliedOn DATETIME NOT NULL
                );";

            ExecuteNonQuery(createTableSql);
        }

        private HashSet<string> GetAppliedMigrations()
        {
            var applied = new HashSet<string>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand("SELECT Version FROM __MigrationHistory", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        applied.Add(reader.GetString(0));
                    }
                }
            }

            return applied;
        }

        private List<IMigration> GetAvailableMigrations()
        {
            return new List<IMigration>
            {
                new CreateTasksTableMigration(),
                new AddCompletedStatusMigration(),
                new AddPriorityFieldMigration()
            };
        }

        private void RecordMigration(IMigration migration)
        {
            const string insertSql = @"
                INSERT INTO __MigrationHistory (Version, Description, AppliedOn)
                VALUES (@version, @description, @appliedOn)";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(insertSql, connection))
                {
                    command.Parameters.AddWithValue("@version", migration.Version);
                    command.Parameters.AddWithValue("@description", migration.Description);
                    command.Parameters.AddWithValue("@appliedOn", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void ExecuteNonQuery(string sql)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    public class CreateTasksTableMigration : IMigration
    {
        public string Version => "1.0.0";
        public string Description => "Create tasks table";
        public string Sql => @"
            CREATE TABLE IF NOT EXISTS TaskEntities (
                TaskId INTEGER PRIMARY KEY AUTOINCREMENT,
                Description TEXT NOT NULL,
                CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
            );";
    }

    public class AddCompletedStatusMigration : IMigration
    {
        public string Version => "1.0.1";
        public string Description => "Add completed status";
        public string Sql => "ALTER TABLE TaskEntities ADD COLUMN IsCompleted BOOLEAN DEFAULT 0;";
    }

    public class AddPriorityFieldMigration : IMigration
    {
        public string Version => "1.0.2";
        public string Description => "Add priority field";
        public string Sql => "ALTER TABLE TaskEntities ADD COLUMN Priority INTEGER DEFAULT 0;";
    }
}