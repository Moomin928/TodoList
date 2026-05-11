using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Module.TaskItems.Dtos;
using Microsoft.EntityFrameworkCore;
using TodoApi.Module.Labels.Dtos;
using TodoApi.Shared.Exceptions;

namespace TodoApi.Module.TaskItems.Queries
{
    public class GetTasksByIdQueryHandler
    {
        private readonly ApplicationDBContext _context;
        public GetTasksByIdQueryHandler(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<TaskItemDto> HandleAsync(int id)
        {
            var task = await _context.TaskItems
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
            if (task == null)
                throw new NotFoundException($"Task with id {id} was not found.");
            return task;
        }
    }
}