using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Dtos.Label;
using TodoApi.Dtos.TaskItem;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/TaskItem")]
    [ApiController]
    public class TaskItemController(ApplicationDBContext context) : ControllerBase
    {
        private static TaskItemDto MapToDto(TaskItem t) => new()
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            IsCompleted = t.IsCompleted,
            CreatedAt = t.CreatedAt,
            Label = new TaskLabelDto
            {
                LabelId = t.LabelId,
                LabelName = t.Label?.Name,
                LabelColor = t.Label?.Color
            }
        };

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await context.TaskItems
                .Include(t => t.Label)
                .ToListAsync();
            return Ok(items.Select(MapToDto).ToList());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var item = await context.TaskItems
                .Include(t => t.Label)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (item == null)
                return NotFound();
            return Ok(MapToDto(item));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskItemRequestDto taskItemDto)
        {



            if (taskItemDto.LabelId.HasValue)
            {
                var labelExists = await context.Labels.AnyAsync(l => l.Id == taskItemDto.LabelId);
                if (!labelExists)
                    return BadRequest($"Label with id {taskItemDto.LabelId} does not exist.");
            }

            var item = new TaskItem
            {
                Title = taskItemDto.Title,
                Description = taskItemDto.Description,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,
                LabelId = taskItemDto.LabelId
            };
            context.TaskItems.Add(item);
            await context.SaveChangesAsync();

            var created = await context.TaskItems
                .Include(t => t.Label)
                .FirstOrDefaultAsync(t => t.Id == item.Id);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, MapToDto(created!));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTaskItemRequestDto taskItemDto)
        {
            var item = await context.TaskItems.FindAsync(id);
            if (item == null)
                return NotFound();


            if (taskItemDto.LabelId.HasValue)
            {
                var labelExists = await context.Labels.AnyAsync(l => l.Id == taskItemDto.LabelId);
                if (!labelExists)
                    return BadRequest($"Label with id {taskItemDto.LabelId} does not exist.");
            }

            item.Title = taskItemDto.Title;
            item.Description = taskItemDto.Description;
            item.IsCompleted = taskItemDto.IsCompleted;
            item.LabelId = taskItemDto.LabelId;
            await context.SaveChangesAsync();

            var updated = await context.TaskItems
                .Include(t => t.Label)
                .FirstOrDefaultAsync(t => t.Id == id);
            return Ok(MapToDto(updated!));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var item = await context.TaskItems.FindAsync(id);
            if (item == null)
                return NotFound();
            context.TaskItems.Remove(item);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
