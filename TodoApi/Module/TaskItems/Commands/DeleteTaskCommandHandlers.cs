using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Shared.Exceptions;

namespace TodoApi.Module.TaskItems.Commands;

public class DeleteTaskCommand
{
    public int Id { get; set; }
}

public class DeleteTaskCommandHandlers
{
    private readonly ApplicationDBContext _context;

    public DeleteTaskCommandHandlers(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task HandleAsync(DeleteTaskCommand command)
    {
        var task = await _context.TaskItems.FindAsync(command.Id);
        if (task == null)
        {
            throw new NotFoundException($"Task with id {command.Id} was not found.");
        }
        _context.TaskItems.Remove(task);
        await _context.SaveChangesAsync();
    }

}
