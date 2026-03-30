using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Dtos.Label;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Feature.Labels.Queries
{
    public class GetLabelByIdQueryHandler
    {
        private readonly ApplicationDBContext _context;
        public GetLabelByIdQueryHandler(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<LabelDto?> HandleAsync(int id)
        {
            return await _context.Labels
            .Where(l => l.Id == id)
            .Select(l => new LabelDto
            {
                Id = l.Id,
                Name = l.Name,
                Description = l.Description,
                Color = l.Color
            }).FirstOrDefaultAsync();
        }
    }
}