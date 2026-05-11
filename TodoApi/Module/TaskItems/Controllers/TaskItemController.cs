using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Module.Labels.Dtos;
using TodoApi.Module.TaskItems.Dtos;
using TodoApi.Module.Entities;
using TodoApi.Module.TaskItems.Commands;
using System.Reflection.Metadata.Ecma335;
using TodoApi.Module.TaskItems.Queries;

namespace TodoApi.Controllers
{
    [Route("api/TaskItem")]
    [ApiController]
    public class TaskItemController : ControllerBase
    {
        private readonly CreateTaskItemCommandsHandlers _createTaskCommandHandlers;
        private readonly UpdateTaskCommandHandlers _updateTaskCommandHandlers;
        private readonly DeleteTaskCommandHandlers _deleteTaskCommandHandlers;
        private readonly GetAllTasksQueryHandler _getAllTaskQueryHandler;
        private readonly GetTasksByIdQueryHandler _getTasksByIdQueryHandler;



        public TaskItemController(
            CreateTaskItemCommandsHandlers createTaskItemCommandsHandlers,
            UpdateTaskCommandHandlers updateTaskCommandHandlers,
            DeleteTaskCommandHandlers deleteTaskCommandHandlers,
            GetAllTasksQueryHandler getAllLabelsQueryHandler,
            GetTasksByIdQueryHandler getTasksByIdQueryHandler

        )
        {
            _createTaskCommandHandlers = createTaskItemCommandsHandlers;
            _updateTaskCommandHandlers = updateTaskCommandHandlers;
            _deleteTaskCommandHandlers = deleteTaskCommandHandlers;
            _getAllTaskQueryHandler = getAllLabelsQueryHandler;
            _getTasksByIdQueryHandler = getTasksByIdQueryHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var task = await _getAllTaskQueryHandler.HandleAsync();
            return Ok(task);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var task = await _getTasksByIdQueryHandler.HandleAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskItemCommand command)
        {
            var task = await _createTaskCommandHandlers.HandleAsync(command);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTaskCommand command)
        {
            command.Id = id;
            var task = await _updateTaskCommandHandlers.HandleAsync(command);
            if (task == null)
                return NotFound();
            return Ok(task);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, DeleteTaskCommand command)
        {
            command.Id = id;
            var task = await _deleteTaskCommandHandlers.HandleAsync(command);
            if (!task)
                return NotFound();
            return NoContent();
        }
    }
}
