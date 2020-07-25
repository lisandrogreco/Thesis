using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Legacy_Todo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Legacy_Todo.Services
{
    public class TaskService
    {
        private const string TaskTableName = "TaskDb";
        AmazonDynamoDBClient _client;
        public TaskService()
        {
            var credentials = new BasicAWSCredentials("REMOVE_ACCESS_KEY", "REMOVE_SECRET_KEY");
            _client = new AmazonDynamoDBClient(credentials);
        }

        public async Task<ICollection<TaskItem>> GetTasks()
        {
            var ctx = new DynamoDBContext(_client);
            var items = await ctx.ScanAsync<TaskItem>(new List<ScanCondition>()).GetRemainingAsync();
            return items.ToList();
        }


        public async Task<TaskItem> GetTask(string taskId)
        {
            var ctx = new DynamoDBContext(_client);
            var item = await ctx.LoadAsync<TaskItem>(taskId);
            return item;
        }


        /// <summary>
        /// Create a new task
        /// </summary>
        /// <returns>The new task</returns>
        public TaskItem CreateTask(string Description)
        {
            var item = new TaskItem { Description = Description, id = Guid.NewGuid().ToString(), IsCompleted = false };

            var ctx = new DynamoDBContext(_client);
            ctx.SaveAsync(item).Wait();
            return item;
        }

        /// <summary>
        /// Updateds an existing task
        /// </summary>        
        /// <returns>The updated Task</returns>
        public async Task<TaskItem> UpdateTask(string Id, string Description)
        {
            var ctx = new DynamoDBContext(_client);
            var item = await ctx.LoadAsync<TaskItem>(Id);
            //var item = _context.GetItemAsync(Id).GetAwaiter().GetResult();
            if (item == null) { throw new Exception("Unable to find the Task"); }

            item.Description = Description;
            await ctx.SaveAsync(item);            
            return item;
        }

        /// <summary>
        /// Delets a Task
        /// </summary>
        /// <param name="taskId">Id of Task</param>        
        public async 
        /// <summary>
        /// Delets a Task
        /// </summary>
        /// <param name="taskId">Id of Task</param>        
        Task
DeleteTask(string taskId)
        {
            var ctx = new DynamoDBContext(_client);
            await ctx.DeleteAsync<TaskItem>(taskId);
        }
    }
}
