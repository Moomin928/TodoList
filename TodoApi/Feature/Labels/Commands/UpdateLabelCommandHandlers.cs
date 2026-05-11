using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using TodoApi.Data;
using TodoApi.Dtos.Label;


namespace TodoApi.Feature.Labels.Commands;

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
    public async Task<TaskLabelDto?> HandleAsync(UpdateLabelCommand command)
    {
        var label = await _context.Labels.FindAsync(command.Id);
        if (label == null)
        {
            return null;
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
