using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Module.Labels.Dtos;
using TodoApi.Module.Entities;
using TodoApi.Shared.Exceptions;

namespace TodoApi.Module.Labels.Commands;


public class CreateLabelCommand
{
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class CreateLabelCommandHandlers
{
    private readonly ApplicationDBContext _context;

    public CreateLabelCommandHandlers(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<TaskLabelDto> HandleAsync(CreateLabelCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
        {
            throw new BadRequestException("Label name is required");
        }
        var label = new Label
        {
            Name = command.Name,
            Color = command.Color,
            Description = command.Description
        };
        _context.Labels.Add(label);
        await _context.SaveChangesAsync();
        return new TaskLabelDto
        {
            LabelId = label.Id,
            LabelColor = label.Color,
            LabelName = label.Name,
        };
    }

}
