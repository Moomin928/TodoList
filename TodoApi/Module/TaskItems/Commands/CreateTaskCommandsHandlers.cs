using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Module.Labels.Dtos;
using TodoApi.Module.TaskItems.Dtos;
using TodoApi.Module.Entities;
using TodoApi.Shared.Exceptions;

namespace TodoApi.Module.TaskItems.Commands;

public class CreateTaskItemCommand
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int? LabelId { get; set; }

}

public class CreateTaskItemCommandsHandlers
{
    private readonly ApplicationDBContext _context;

    public CreateTaskItemCommandsHandlers(ApplicationDBContext context)
    {
        _context = context;
    }

    public static TaskItemDto MapToDto(TaskItem t) => new()
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

    public async Task<TaskItemDto> HandleAsync(CreateTaskItemCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Title))
        {
            throw new BadRequestException("Title is required");
        }
        if (command.LabelId.HasValue)
        {
            var labelExists = await _context.Labels.AnyAsync(l => l.Id == command.LabelId.Value);
            if (!labelExists)
            {
                throw new BadRequestException($"Label with ID {command.LabelId.Value} was not found.");
            }

        }
        var existingTask = await _context.TaskItems.FirstOrDefaultAsync(t => t.Title == command.Title);
        if (existingTask != null)
        {
            throw new ConflictException($"Task with title {command.Title} already exists");
        }
        var isTitleTaken = await _context.TaskItems.AnyAsync(t => t.Title == command.Title);
        if (isTitleTaken)
        {
            throw new ConflictException($"Task with title {command.Title} already exists");
        }
        var task = new TaskItem
        {
            Title = command.Title,
            Description = command.Description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow,
            LabelId = command.LabelId
        };
        _context.TaskItems.Add(task);
        await _context.SaveChangesAsync();
        var createdTask = await _context.TaskItems
            .Include(t => t.Label)
            .FirstOrDefaultAsync(t => t.Id == task.Id);

        return MapToDto(createdTask!);

    }
}

