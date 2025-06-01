using System;
using System.Collections.Generic;
using dz3.Data;
using dz3.Logging;
using dz3.Models;

namespace dz3.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _repository;
        private readonly ILogger _logger;

        public TaskService(ITaskRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public List<TaskEntity> GetAllTasks()
        {
            return _repository.GetAllTasks();
        }

        public bool AddTask(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                _logger.LogError("Попытка добавить пустую задачу");
                return false;
            }

            var taskId = _repository.AddTask(description.Trim());
            return taskId > 0;
        }

        public bool UpdateTask(int id, string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                _logger.LogError("Попытка обновить задачу пустым описанием");
                return false;
            }

            return _repository.UpdateTask(id, description.Trim());
        }

        public bool DeleteTask(int id)
        {
            return _repository.DeleteTask(id);
        }
    }
}