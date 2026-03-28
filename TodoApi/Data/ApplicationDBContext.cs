using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using TodoApi.Models;

namespace TodoApi.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions)
        : base(dbContextOptions)
        {

        }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Label> Labels { get; set; }

    }
}