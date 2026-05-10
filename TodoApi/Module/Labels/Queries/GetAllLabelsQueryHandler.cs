using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Module.Labels.Dtos;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Module.Labels.Queries
{
    public class GetAllLabelsQueryHandler
    {
        private readonly ApplicationDBContext _context;
        public GetAllLabelsQueryHandler(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<LabelDto>> HandleAsync()
        {
            return await _context.Labels
            .Select(l => new LabelDto
            {
                Id = l.Id,
                Name = l.Name,
                Description = l.Description,
                Color = l.Color
            }).ToListAsync();
        }
    }
}