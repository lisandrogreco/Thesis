using Azure_Todo_Functions.Data;
using Azure_Todo_Functions.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Todo_Functions.Services
{
    public class TaskService
    {
        readonly ICosmosDbService _context;
        readonly ILogger _logger;
        public TaskService(ICosmosDbService context, ILoggerFactory factory)
        {
            _context = context;
            _logger = factory.CreateLogger<TaskService>();
        }

        public async Task<ICollection<TaskItem>> GetTasks()
        {
            var data = await _context.GetItemsAsync("SELECT * FROM c");
            return data.ToList();
        } 


        public TaskItem GetTask(string taskId)
        {
            return _context.GetItemAsync(taskId).GetAwaiter().GetResult();

        }

       
        /// <summary>
        /// Create a new task
        /// </summary>
        /// <returns>The new task</returns>
        public TaskItem CreateTask(string Description)
        {
            var item = new TaskItem { Description = Description, id = Guid.NewGuid(), IsCompleted = false };
            _context.AddItemAsync(item).Wait();
            return item;
        }

        /// <summary>
        /// Updateds an existing task
        /// </summary>        
        /// <returns>The updated Task</returns>
        public TaskItem UpdateTask(string Id, string Description)
        {

            var item = _context.GetItemAsync(Id).GetAwaiter().GetResult();
            if (item == null) { throw new Exception("Unable to find the Task"); }

            item.Description = Description;
            _context.UpdateItemAsync(item.id.ToString(), item).Wait();
            return item;
        }

        /// <summary>
        /// Delets a Task
        /// </summary>
        /// <param name="taskId">Id of Task</param>        
        public void DeleteTask(string taskId)
        {
            _context.DeleteItemAsync(taskId).Wait();
        }
    }
}
