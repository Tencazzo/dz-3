using dz3.Data;
using dz3.Logging;
using dz3.Models;
using System;
using System.Collections.Generic;

namespace dz3.Services
{
    public class TaskManagementService : IDisposable
    {
        private readonly ITaskRepository taskRepository;
        private readonly ILogger logger;
        private bool disposed = false;

        public TaskManagementService(ITaskRepository repository, ILogger logger)
        {
            taskRepository = repository;
            this.logger = logger;
            logger.LogInfo("Инициализация TaskManagementService");
        }

        public void Initialize()
        {
            logger.LogInfo("Инициализация сервиса управления задачами");

            try
            {
                taskRepository.InitializeStorage();
                logger.LogInfo("Сервис успешно инициализирован");
            }
            catch (Exception ex)
            {
                logger.LogError("Ошибка при инициализации сервиса", ex);
                throw;
            }
        }


        public List<TaskEntity> GetAllTasks()
        {
            logger.LogDebug("Запрос всех задач");

            try
            {
                var tasks = taskRepository.GetAllTasks();
                logger.LogInfo($"Возвращено {tasks.Count} задач");
                return tasks;
            }
            catch (Exception ex)
            {
                logger.LogError("Ошибка при получении задач", ex);
                throw;
            }
        }

        public bool AddNewTask(string taskDescription)
        {
            if (string.IsNullOrWhiteSpace(taskDescription))
            {
                logger.LogWarning("Попытка добавления пустой задачи");
                return false;
            }

            logger.LogInfo($"Добавление задачи: {taskDescription}");

            try
            {
                var taskId = taskRepository.CreateTask(taskDescription);
                var success = taskId > 0;

                if (success)
                {
                    logger.LogInfo($"Задача добавлена с ID: {taskId}");
                }

                return success;
            }
            catch (Exception ex)
            {
                logger.LogError($"Ошибка при добавлении задачи: {taskDescription}", ex);
                return false;
            }
        }

        public bool UpdateTask(int taskId, string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
            {
                logger.LogWarning($"Попытка обновления задачи {taskId} пустым описанием");
                return false;
            }

            logger.LogInfo($"Обновление задачи {taskId}: {newDescription}");

            try
            {
                var success = taskRepository.UpdateTask(taskId, newDescription);

                if (success)
                {
                    logger.LogInfo($"Задача {taskId} обновлена");
                }

                return success;
            }
            catch (Exception ex)
            {
                logger.LogError($"Ошибка при обновлении задачи {taskId}", ex);
                return false;
            }
        }

        public bool DeleteTask(int taskId)
        {
            logger.LogInfo($"Удаление задачи {taskId}");

            try
            {
                var success = taskRepository.DeleteTask(taskId);

                if (success)
                {
                    logger.LogInfo($"Задача {taskId} удалена");
                }

                return success;
            }
            catch (Exception ex)
            {
                logger.LogError($"Ошибка при удалении задачи {taskId}", ex);
                return false;
            }
        }

        public bool ToggleTaskCompletion(int taskId)
        {
            logger.LogInfo($"Переключение статуса задачи {taskId}");

            try
            {
                var success = taskRepository.ToggleTaskCompletion(taskId);

                if (success)
                {
                    logger.LogInfo($"Статус задачи {taskId} переключен");
                }

                return success;
            }
            catch (Exception ex)
            {
                logger.LogError($"Ошибка при переключении статуса задачи {taskId}", ex);
                return false;
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                logger.LogInfo("Освобождение ресурсов TaskManagementService");
                taskRepository?.Dispose();
                disposed = true;
            }
        }
    }
}
