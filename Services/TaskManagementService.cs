using System.Collections.Generic;
using System.Threading.Tasks;
using dz3.Data;
using dz3.Logging;
using dz3.Models;

namespace dz3.Services
{
    public class TaskManagementService
    {
        private readonly ITaskRepository taskRepository;
        private readonly ILogger logger;

        public TaskManagementService(ITaskRepository taskRepository, ILogger logger)
        {
            this.taskRepository = taskRepository;
            this.logger = logger;
        }

        public async Task InitializeAsync()
        {
            await taskRepository.InitializeDatabaseAsync();
        }

        public async Task<List<TaskEntity>> GetAllTasksAsync()
        {
            return await taskRepository.GetAllTasksAsync();
        }

        public async Task<bool> AddTaskAsync(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                logger.LogError("Попытка добавить пустую задачу");
                return false;
            }

            var taskId = await taskRepository.AddTaskAsync(description.Trim());
            return taskId > 0;
        }

        public async Task<bool> UpdateTaskAsync(int id, string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                logger.LogError("Попытка обновить задачу пустым описанием");
                return false;
            }

            return await taskRepository.UpdateTaskAsync(id, description.Trim());
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            return await taskRepository.DeleteTaskAsync(id);
        }
    }
}