using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Legacy_Todo.Models
{
    public class AddTaskViewModel
    {
        public string Description { get; set; }
    }

    public class UpdateTaskViewModel
    {
        public string Description { get; set; }
        public Guid Id { get; set; }
    }

    public class DeleteTaskViewModel
    {
        public Guid Id { get; set; }
    }

    class TaskItem
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public string Username { get; set; }

    }
}
