using dz3.Models;
using System;
using System.Collections.Generic;

namespace dz3.Data
{
    public interface ITaskRepository : IDisposable
    {
        void InitializeStorage();
        List<TaskEntity> GetAllTasks();
        TaskEntity GetTaskById(int taskId);
        int CreateTask(string description);
        bool UpdateTask(int taskId, string newDescription);
        bool DeleteTask(int taskId);
        bool ToggleTaskCompletion(int taskId);
    }
}
