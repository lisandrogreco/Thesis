using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using Legacy_Todo.Services;
using Azure_Todo_Functions.Services;

namespace Azure_Todo_Functions
{

    public class TaskFunctions
    {
        IAuthService authService;
        TaskService taskService;
        public TaskFunctions(IAuthService authService, TaskService taskService)
        {
            this.authService = authService;
            this.taskService = taskService;
        }

        [FunctionName("GetTasks")]
        public async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Task")] HttpRequest req,
            ILogger log)
        {

            if (!authService.ValidateToken(req))
            {
                return new UnauthorizedResult();
            }

            log.LogInformation("C# HTTP trigger function processed a request.");
            var tasks = await taskService.GetTasks();
            return new OkObjectResult(tasks);
        }

        [FunctionName("AddTask")]
        public async Task<IActionResult> Add(
           [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Task")] HttpRequest req,
           ILogger log)
        {
            if (!authService.ValidateToken(req))
            {
                return new UnauthorizedResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string taskDescription = data.Description;
            try { 
            var newItem = taskService.CreateTask(taskDescription);
            log.LogInformation("Task added.");
            return new OkObjectResult(newItem);
            } catch(Exception ex)
            {
                log.LogError(ex, "Error");
            }
            return new BadRequestResult();
        }

        [FunctionName("UpdateTask")]
        public async Task<IActionResult> Update(
           [HttpTrigger(AuthorizationLevel.Function, "put", Route = "Task")] HttpRequest req,
           ILogger log)
        {
            if (!authService.ValidateToken(req))
            {
                return new UnauthorizedResult();
            }
            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string id = data.Id;
            string taskDescription = data.Description;

            try
            {
                var updatedTask = taskService.UpdateTask(id, taskDescription);
                return new OkObjectResult(updatedTask);
            }
            catch (Exception)
            {
                return new NotFoundResult();
            }

        }

        [FunctionName("DeleteTask")]
        public async Task<IActionResult> Delete(
           [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "Task")] HttpRequest req,
           ILogger log)
        {
            log.LogInformation("Task deleted.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string id = data.Id;
            taskService.DeleteTask(id);
            return new NoContentResult();
        }
    }
}
