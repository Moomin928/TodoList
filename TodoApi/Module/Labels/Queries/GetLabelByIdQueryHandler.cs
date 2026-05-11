using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Module.Labels.Dtos;
using Microsoft.EntityFrameworkCore;
using TodoApi.Shared.Exceptions;

namespace TodoApi.Module.Labels.Queries
{
    public class GetLabelByIdQueryHandler
    {
        private readonly ApplicationDBContext _context;
        public GetLabelByIdQueryHandler(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<LabelDto> HandleAsync(int id)
        {
            var label = await _context.Labels
            .Where(l => l.Id == id)
            .Select(l => new LabelDto
            {
                Id = l.Id,
                Name = l.Name,
                Description = l.Description,
                Color = l.Color
            }).FirstOrDefaultAsync();
            if (label == null)
                throw new NotFoundException($"Label with id {id} was not found.");
            return label;
        }
    }
}