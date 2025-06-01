using System;

namespace dz3.Models
{
    public class TaskEntity
    {
        public int TaskId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsCompleted { get; set; }
        public int Priority { get; set; }

        public TaskEntity() { }

        public TaskEntity(int id, string description, DateTime createdAt = default, bool completed = false, int priority = 0)
        {
            TaskId = id;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            CreatedAt = createdAt == default ? DateTime.Now : createdAt;
            IsCompleted = completed;
            Priority = priority;
        }
    }
}
