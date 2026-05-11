using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Module.Labels.Dtos;
using TodoApi.Shared.Exceptions;

namespace TodoApi.Module.Labels.Commands;

public class UpdateLabelCommand
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
public class UpdateLabelCommandHandlers
{
    private readonly ApplicationDBContext _context;

    public UpdateLabelCommandHandlers(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<TaskLabelDto> HandleAsync(UpdateLabelCommand command)
    {
        var label = await _context.Labels.FindAsync(command.Id);
        if (label == null)
        {
            throw new NotFoundException($"Label with id {command.Id} was not found.");
        }
        if (string.IsNullOrWhiteSpace(command.Name))
        {
            throw new BadRequestException("Name is required");
        }
        
        var existingLabel = await _context.Labels.FirstOrDefaultAsync(l => l.Name == command.Name);
        if (existingLabel != null && existingLabel.Id != command.Id)
        {
            throw new ConflictException($"Label with name {command.Name} already exists");
        }
        
        label.Name = command.Name;
        label.Description = command.Description;
        label.Color = command.Color;
        await _context.SaveChangesAsync();
        return new TaskLabelDto
        {
            LabelId = label.Id,
            LabelName = label.Name,
            LabelColor = label.Color,
        };
    }
}
