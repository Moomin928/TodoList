using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Dtos.TaskItem;
using Microsoft.EntityFrameworkCore;
using TodoApi.Dtos.Label;


namespace TodoApi.Feature.TaskItems.Queries
{
    public class GetTasksByIdQueryHandler
    {
        private readonly ApplicationDBContext _context;
        public GetTasksByIdQueryHandler(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<TaskItemDto?> HandleAsync(int id)
        {
            return await _context.TaskItems
            .Where(t => t.Id == id)
            .Select(t => new TaskItemDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                CreatedAt = t.CreatedAt,
                IsCompleted = t.IsCompleted,
                Label = new TaskLabelDto
                {
                    LabelId = t.LabelId,
                    LabelColor = t.Label != null ? t.Label.Color : null,
                    LabelName = t.Label != null ? t.Label.Name : null
                }
            }).FirstOrDefaultAsync();

        }
    }
}