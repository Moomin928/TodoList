using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TodoApi.Data;
using TodoApi.Dtos.TaskItem;

namespace TodoApi.Controllers
{

    [Route("api/TaskItem")]
    [ApiController]
    public class TaskItemController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public TaskItemController(ApplicationDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var TaskItem = await _context.TaskItems.ToListAsync();
            return Ok(TaskItem);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var TaskItem = await _context.TaskItems.FindAsync(id);
            if (TaskItem == null)
            {
                return NotFound();
            }
            return Ok(TaskItem);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskItemRequestDto taskItemDto)
        {
            var TaskItem = new Models.TaskItem
            {
                Title = taskItemDto.Title,
                Description = taskItemDto.Description,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };
            _context.TaskItems.Add(TaskItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = TaskItem.Id }, TaskItem);
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTaskItemRequestDto taskItemDto)
        {
            var TaskItem = await _context.TaskItems.FindAsync(id);
            if (TaskItem == null)
            {
                return NotFound();
            }
            TaskItem.Title = taskItemDto.Title;
            TaskItem.Description = taskItemDto.Description;
            TaskItem.IsCompleted = taskItemDto.IsCompleted;

            await _context.SaveChangesAsync();
            return Ok(TaskItem);
        }


        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var TaskItem = await _context.TaskItems.FindAsync(id);
            if (TaskItem == null)
            {
                return NotFound();
            }
            _context.TaskItems.Remove(TaskItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}