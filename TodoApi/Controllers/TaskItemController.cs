using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Dtos.TaskItem;
using TodoApi.Models;

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

       private static TaskItemDto MapToDto(TaskItem t) => new()
{
    Id = t.Id,
    Title = t.Title,
    Description = t.Description,
    IsCompleted = t.IsCompleted,
    CreatedAt = t.CreatedAt,
    Category = new TaskCategoryDto
    {
        CategoryId = t.CategoryId,
        CategoryName = t.Category?.Name,
        CategoryColor = t.Category?.Color
    }
};

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _context.TaskItems
                .Include(t => t.Category)
                .ToListAsync();
  				var entities = items.Select(MapToDto).ToList();
            return Ok(entities);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var item = await _context.TaskItems
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (item == null)
                return NotFound();
            return Ok(MapToDto(item));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskItemRequestDto taskItemDto)
        {
            if (taskItemDto.CategoryId.HasValue)
            {
                var categoryExists = await _context.Categories.AnyAsync(c => c.Id == taskItemDto.CategoryId);
                if (!categoryExists)
                    return BadRequest($"Category with id {taskItemDto.CategoryId} does not exist.");
            }

            var item = new TaskItem
            {
                Title = taskItemDto.Title,
                Description = taskItemDto.Description,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,
                CategoryId = taskItemDto.CategoryId
            };
            _context.TaskItems.Add(item);
            await _context.SaveChangesAsync();

            var created = await _context.TaskItems
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == item.Id);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, MapToDto(created!));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTaskItemRequestDto taskItemDto)
        {
            var item = await _context.TaskItems.FindAsync(id);
            if (item == null)
                return NotFound();

            if (taskItemDto.CategoryId.HasValue)
            {
                var categoryExists = await _context.Categories.AnyAsync(c => c.Id == taskItemDto.CategoryId);
                if (!categoryExists)
                    return BadRequest($"Category with id {taskItemDto.CategoryId} does not exist.");
            }

            item.Title = taskItemDto.Title;
            item.Description = taskItemDto.Description;
            item.IsCompleted = taskItemDto.IsCompleted;
            item.CategoryId = taskItemDto.CategoryId;
            await _context.SaveChangesAsync();

            var updated = await _context.TaskItems
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);
            return Ok(MapToDto(updated!));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var item = await _context.TaskItems.FindAsync(id);
            if (item == null)
                return NotFound();
            _context.TaskItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
