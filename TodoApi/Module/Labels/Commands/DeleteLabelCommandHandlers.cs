using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Shared.Exceptions;

namespace TodoApi.Module.Labels.Commands;

public class DeleteLabelCommand
{
    public int Id { get; set; }
}
public class DeleteLabelCommandHandlers
{
    private readonly ApplicationDBContext _context;
    public DeleteLabelCommandHandlers(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task HandleAsync(DeleteLabelCommand command)
    {
        var label = await _context.Labels.FindAsync(command.Id);
        if (label == null)
        {
            throw new NotFoundException($"Label with id {command.Id} was not found.");
        }
        _context.Labels.Remove(label);
        await _context.SaveChangesAsync();
    }

}
