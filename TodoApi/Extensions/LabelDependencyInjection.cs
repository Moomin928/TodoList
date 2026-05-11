using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Feature;
using Microsoft.Extensions.DependencyInjection;
using TodoApi.Feature.Labels.Commands;
using TodoApi.Feature.Labels.Queries;

namespace TodoApi.Extensions
{
    public static class LabelDependencyInjection
    {
        public static IServiceCollection AddLabelsMoudle(this IServiceCollection services)
        {
            services.AddScoped<CreateLabelCommandHandlers>();
            services.AddScoped<UpdateLabelCommandHandlers>();
            services.AddScoped<DeleteLabelCommandHandlers>();
            services.AddScoped<GetAllLabelsQueryHandler>();
            services.AddScoped<GetLabelByIdQueryHandler>();
            return services;
        }
    }
}