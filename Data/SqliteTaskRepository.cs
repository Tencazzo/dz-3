using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SQLite;
using dz3.Logging;
using dz3.Models;

namespace dz3.Data
{
    public class SqliteTaskRepository : ITaskRepository
    {
        private readonly string connectionString;
        private readonly ILogger logger;

        public SqliteTaskRepository(string connectionString, ILogger logger)
        {
            this.connectionString = connectionString;
            this.logger = logger;
        }

        public async Task InitializeDatabaseAsync()
        {
            try
            {
                // Проверяем существование файла базы данных
                var dbPath = connectionString.Replace("Data Source=", "").Split(';')[0];
                if (!System.IO.File.Exists(dbPath))
                {
                    // Создаем базу данных
                    SQLiteConnection.CreateFile(dbPath);
                }

                using (var connection = new SQLiteConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Tasks (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Description TEXT NOT NULL,
                            CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                        )";

                    using (var command = new SQLiteCommand(createTableQuery, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }

                logger.Log("База данных инициализирована");
            }
            catch (Exception ex)
            {
                logger.LogError($"Ошибка инициализации базы: {ex.Message}");
                throw;
            }
        }

        public async Task<List<TaskEntity>> GetAllTasksAsync()
        {
            var tasks = new List<TaskEntity>();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = "SELECT Id, Description, CreatedAt FROM Tasks ORDER BY Id DESC";

                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tasks.Add(new TaskEntity
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Description = reader["Description"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                            });
                        }
                    }
                }

                logger.Log($"Загружено {tasks.Count} задач");
            }
            catch (Exception ex)
            {
                logger.LogError($"Ошибка загрузки задач: {ex.Message}");
            }

            return tasks;
        }

        public async Task<int> AddTaskAsync(string description)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = "INSERT INTO Tasks (Description) VALUES (@description); SELECT last_insert_rowid();";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@description", description);
                        var result = await command.ExecuteScalarAsync();

                        var taskId = Convert.ToInt32(result);
                        logger.Log($"Создана задача с ID: {taskId}");
                        return taskId;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Ошибка создания задачи: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> UpdateTaskAsync(int id, string description)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = "UPDATE Tasks SET Description = @description WHERE Id = @id";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@id", id);

                        var rowsAffected = await command.ExecuteNonQueryAsync();
                        var success = rowsAffected > 0;

                        if (success)
                            logger.Log($"Обновлена задача ID: {id}");

                        return success;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Ошибка обновления задачи: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = "DELETE FROM Tasks WHERE Id = @id";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        var rowsAffected = await command.ExecuteNonQueryAsync();
                        var success = rowsAffected > 0;

                        if (success)
                            logger.Log($"Удалена задача ID: {id}");

                        return success;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Ошибка удаления задачи: {ex.Message}");
                return false;
            }
        }

        // Синхронные методы
        public List<TaskEntity> GetAllTasks()
        {
            return GetAllTasksAsync().GetAwaiter().GetResult();
        }

        public int AddTask(string description)
        {
            return AddTaskAsync(description).GetAwaiter().GetResult();
        }

        public bool UpdateTask(int id, string description)
        {
            return UpdateTaskAsync(id, description).GetAwaiter().GetResult();
        }

        public bool DeleteTask(int id)
        {
            return DeleteTaskAsync(id).GetAwaiter().GetResult();
        }
    }
}