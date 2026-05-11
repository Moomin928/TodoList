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
    public class GetAllTasksQueryHandler
    {
        private readonly ApplicationDBContext _context;
        public GetAllTasksQueryHandler(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<TaskItemDto>> HandleAsync()
        {
            return await _context.TaskItems
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
                    LabelName = t.Label != null ? t.Label.Name : null,
                    LabelColor = t.Label != null ? t.Label.Color : null
                }
            }).ToListAsync();
        }
    }
}