using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Legacy_Todo.Models;
using Legacy_Todo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Legacy_Todo.Controllers
{
    [Route("api/[controller]")]
    [Authorize]    
    [ApiController]
    public class TaskController : ControllerBase
    {
        // private static List<TaskItem> _tasks = new List<TaskItem>() { new TaskItem { Id=Guid.NewGuid(),Username="MAx", Description="mein task", IsCompleted=false} };

        TaskService service;

        public TaskController(TaskService service)
        {
            this.service = service;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await service.GetTasks();
            return new OkObjectResult(tasks);
        }

        [HttpPost]
        public IActionResult Add(AddTaskViewModel item)
        {
            service.CreateTask(item.Description);
            //_tasks.Add(new TaskItem { Description = item.Description, Id = Guid.NewGuid(), IsCompleted = false });
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateTaskViewModel item)
        {
            await service.UpdateTask(item.Id.ToString(), item.Description);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteTaskViewModel item)
        {
            await service.DeleteTask(item.Id.ToString());
            return NoContent();
        }

    }

   
}