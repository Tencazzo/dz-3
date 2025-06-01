using System.Collections.Generic;
using System.Threading.Tasks;
using dz3.Models;

namespace dz3.Data
{
    public interface ITaskRepository
    {
        Task InitializeDatabaseAsync();

        // Асинхронные
        Task<List<TaskEntity>> GetAllTasksAsync();
        Task<int> AddTaskAsync(string description);
        Task<bool> UpdateTaskAsync(int id, string description);
        Task<bool> DeleteTaskAsync(int id);

        // Синхронные
        List<TaskEntity> GetAllTasks();
        int AddTask(string description);
        bool UpdateTask(int id, string description);
        bool DeleteTask(int id);
    }
}