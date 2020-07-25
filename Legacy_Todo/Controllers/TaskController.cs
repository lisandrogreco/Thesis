using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Legacy_Todo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Legacy_Todo.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private static List<TaskItem> _tasks = new List<TaskItem>() { new TaskItem { Id = Guid.NewGuid(), Username = "MAx", Description = "mein task", IsCompleted = false } };


        [HttpGet]
        public IActionResult GetTasks()
        {
            return new OkObjectResult(_tasks);
        }

        [HttpPost]
        public IActionResult Add(AddTaskViewModel item)
        {
            _tasks.Add(new TaskItem { Description = item.Description, Id = Guid.NewGuid(), IsCompleted = false });
            return Ok();
        }

        [HttpPut]
        public IActionResult Update(UpdateTaskViewModel item)
        {
            var taskToUpdate = _tasks.FirstOrDefault(t => t.Id == item.Id);
            if (taskToUpdate != null)
            {
                taskToUpdate.Description = item.Description;
                return Ok();
            }
            return NotFound();
        }

        [HttpDelete]
        public IActionResult Delete(DeleteTaskViewModel item)
        {
            var removeTask = _tasks.FirstOrDefault(t => t.Id == item.Id);
            if (removeTask != null)
            {
                _tasks.Remove(removeTask);
                return NoContent();
            }
            return NotFound();
        }

    }


}