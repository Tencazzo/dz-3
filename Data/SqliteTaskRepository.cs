using dz3.Logging;
using dz3.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace dz3.Data
{
    public class SqliteTaskRepository : ITaskRepository
    {
        private readonly string connectionString;
        private readonly ILogger logger;
        private bool disposed = false; // Добавлено поле disposed

        public SqliteTaskRepository(string dbConnectionString, ILogger logger)
        {
            this.connectionString = dbConnectionString;
            this.logger = logger;
        }

        public void InitializeStorage()
        {
            logger.LogInfo("Инициализация хранилища");

            const string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS TaskEntities (
                    TaskId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Description TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    IsCompleted BOOLEAN DEFAULT 0,
                    Priority INTEGER DEFAULT 0
                );";

            try
            {
                ExecuteNonQuery(createTableQuery);
                logger.LogInfo("Хранилище успешно инициализировано");
            }
            catch (Exception ex)
            {
                logger.LogError("Ошибка при инициализации хранилища", ex);
                throw;
            }
        }

        public List<TaskEntity> GetAllTasks()
        {
            logger.LogDebug("Получение всех задач");

            var tasks = new List<TaskEntity>();
            const string selectQuery = @"
                SELECT TaskId, Description, CreatedAt, IsCompleted, Priority 
                FROM TaskEntities 
                ORDER BY Priority DESC, CreatedAt DESC";

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new SqliteCommand(selectQuery, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var task = new TaskEntity
                            {
                                TaskId = reader.GetInt32(0),
                                Description = reader.GetString(1),
                                CreatedAt = reader.GetDateTime(2),
                                IsCompleted = reader.GetBoolean(3),
                                Priority = reader.GetInt32(4)
                            };
                            tasks.Add(task);
                        }
                    }
                }

                logger.LogInfo($"Получено {tasks.Count} задач");
                return tasks;
            }
            catch (Exception ex)
            {
                logger.LogError("Ошибка при получении задач", ex);
                throw;
            }
        }

        public TaskEntity GetTaskById(int taskId)
        {
            logger.LogDebug($"Поиск задачи с ID: {taskId}");

            const string selectQuery = @"
                SELECT TaskId, Description, CreatedAt, IsCompleted, Priority 
                FROM TaskEntities 
                WHERE TaskId = @id";

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new SqliteCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", taskId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new TaskEntity
                                {
                                    TaskId = reader.GetInt32(0),
                                    Description = reader.GetString(1),
                                    CreatedAt = reader.GetDateTime(2),
                                    IsCompleted = reader.GetBoolean(3),
                                    Priority = reader.GetInt32(4)
                                };
                            }
                        }
                    }
                }

                logger.LogWarning($"Задача с ID {taskId} не найдена");
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError($"Ошибка при поиске задачи {taskId}", ex);
                throw;
            }
        }

        public int CreateTask(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                logger.LogWarning("Попытка создания задачи с пустым описанием");
                throw new ArgumentException("Описание не может быть пустым");
            }

            logger.LogInfo($"Создание задачи: {description}");

            const string insertQuery = @"
                INSERT INTO TaskEntities (Description) 
                VALUES (@desc); 
                SELECT last_insert_rowid();";

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new SqliteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@desc", description.Trim());
                        var result = command.ExecuteScalar();
                        var taskId = Convert.ToInt32(result);

                        logger.LogInfo($"Задача создана с ID: {taskId}");
                        return taskId;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Ошибка при создании задачи: {description}", ex);
                throw;
            }
        }

        public bool UpdateTask(int taskId, string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
            {
                logger.LogWarning($"Попытка обновления задачи {taskId} с пустым описанием");
                return false;
            }

            logger.LogInfo($"Обновление задачи {taskId}: {newDescription}");

            const string updateQuery = "UPDATE TaskEntities SET Description = @desc WHERE TaskId = @id";

            try
            {
                var rowsAffected = ExecuteNonQuery(updateQuery,
                    ("@desc", newDescription.Trim()),
                    ("@id", taskId));

                var success = rowsAffected > 0;
                if (success)
                {
                    logger.LogInfo($"Задача {taskId} обновлена");
                }
                else
                {
                    logger.LogWarning($"Задача {taskId} не найдена для обновления");
                }

                return success;
            }
            catch (Exception ex)
            {
                logger.LogError($"Ошибка при обновлении задачи {taskId}", ex);
                throw;
            }
        }

        public bool DeleteTask(int taskId)
        {
            logger.LogInfo($"Удаление задачи {taskId}");

            const string deleteQuery = "DELETE FROM TaskEntities WHERE TaskId = @id";

            try
            {
                var rowsAffected = ExecuteNonQuery(deleteQuery, ("@id", taskId));

                var success = rowsAffected > 0;
                if (success)
                {
                    logger.LogInfo($"Задача {taskId} удалена");
                }
                else
                {
                    logger.LogWarning($"Задача {taskId} не найдена для удаления");
                }

                return success;
            }
            catch (Exception ex)
            {
                logger.LogError($"Ошибка при удалении задачи {taskId}", ex);
                throw;
            }
        }

        public bool ToggleTaskCompletion(int taskId)
        {
            logger.LogInfo($"Переключение статуса задачи {taskId}");

            const string toggleQuery = "UPDATE TaskEntities SET IsCompleted = NOT IsCompleted WHERE TaskId = @id";

            try
            {
                var rowsAffected = ExecuteNonQuery(toggleQuery, ("@id", taskId));

                var success = rowsAffected > 0;
                if (success)
                {
                    logger.LogInfo($"Статус задачи {taskId} переключен");
                }
                else
                {
                    logger.LogWarning($"Задача {taskId} не найдена");
                }

                return success;
            }
            catch (Exception ex)
            {
                logger.LogError($"Ошибка при переключении статуса задачи {taskId}", ex);
                throw;
            }
        }

        private int ExecuteNonQuery(string query, params (string name, object value)[] parameters)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(query, connection))
                {
                    foreach (var (name, value) in parameters)
                    {
                        command.Parameters.AddWithValue(name, value);
                    }
                    return command.ExecuteNonQuery();
                }
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                logger.LogInfo("Освобождение ресурсов SqliteTaskRepository");
                disposed = true;
            }
        }
    }
}