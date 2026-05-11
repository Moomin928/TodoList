using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TodoApi.Module.TaskItems.Commands;
using TodoApi.Module.TaskItems.Queries;

namespace TodoApi.Extensions
{
    public static class TaskDependcyInjection
    {
        public static IServiceCollection AddTasksMoudle(this IServiceCollection services)
        {
            services.AddScoped<CreateTaskItemCommandsHandlers>();
            services.AddScoped<UpdateTaskCommandHandlers>();
            services.AddScoped<DeleteTaskCommandHandlers>();
            services.AddScoped<GetAllTasksQueryHandler>();
            services.AddScoped<GetTasksByIdQueryHandler>();

            return services;
        }
    }
}