using System;

namespace dz3.Models
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}