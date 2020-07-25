using Amazon.DynamoDBv2.DataModel;
using System;

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

    [DynamoDBTable("TaskDb")]
    public class TaskItem
    {
        [DynamoDBHashKey]
        public string id { get; set; }
        [DynamoDBProperty]
        public string Description { get; set; }
        [DynamoDBProperty]
        public bool IsCompleted { get; set; }
        [DynamoDBProperty]
        public string Username { get; set; }

    }
}
