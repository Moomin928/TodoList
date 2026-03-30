using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Data;

namespace TodoApi.Feature.Labels.Commands;

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
    public async Task<bool> HandleAsync(DeleteLabelCommand command)
    {
        var label = await _context.Labels.FindAsync(command.Id);
        if (label == null)
        {
            return false;
        }
        _context.Labels.Remove(label);
        await _context.SaveChangesAsync();

        return true;

    }

}
