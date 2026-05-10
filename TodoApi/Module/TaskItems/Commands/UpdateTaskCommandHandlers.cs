using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Module.Labels.Dtos;
using TodoApi.Module.TaskItems.Dtos;
using TodoApi.Module.Entities;


namespace TodoApi.Module.TaskItems.Commands;

public class UpdateTaskCommand
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public int? LabelId { get; set; }


}

public class UpdateTaskCommandHandlers
{
    private readonly ApplicationDBContext _context;

    public UpdateTaskCommandHandlers(ApplicationDBContext context)
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
        Label = new TaskLabelDto
        {
            LabelId = t.LabelId,
            LabelColor = t.Label?.Color,
            LabelName = t.Label?.Name
        }
    };
    public async Task<TaskItemDto?> HandleAsync(UpdateTaskCommand command)
    {
        var task = await _context.TaskItems.FindAsync(command.Id);
        if (task == null)
        {
            return null;
        }
        if (command.LabelId.HasValue)
        {
            var labelExists = await _context.Labels.AnyAsync(l => l.Id == command.LabelId.Value);
            if (!labelExists)
            {
                throw new Exception($"Label with id {command.LabelId.Value} does not exit.");
            }
        }
        task.Title = command.Title;
        task.Description = command.Description;
        task.IsCompleted = command.IsCompleted;
        task.LabelId = command.LabelId;
        await _context.SaveChangesAsync();

        var updateTask = await _context.TaskItems
        .Include(t => t.Label)
        .FirstOrDefaultAsync(t => t.Id == command.Id);

        return MapToDto(updateTask!);
    }

}

