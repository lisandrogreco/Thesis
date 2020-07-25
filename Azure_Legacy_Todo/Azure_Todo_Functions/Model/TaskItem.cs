using System;
using System.Collections.Generic;
using System.Text;

namespace Azure_Todo_Functions.Model
{
    public class TaskItem
    {
        public Guid id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public string Username { get; set; }

    }
}
