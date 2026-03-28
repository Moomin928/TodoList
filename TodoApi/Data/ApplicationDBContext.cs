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
        public DbSet<Category> Categories { get; set; }
        public DbSet<Label> Labels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.Category)
            .WithMany(c => c.TaskItems)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Study", Description = "Study tasks", Color = "blue" },
                new Category { Id = 2, Name = "Work", Description = "Work tasks", Color = "red" }
            );
            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem
                {
                    Id = 1,
                    Title = "Finish homework",
                    Description = "Complete assignment",
                    IsCompleted = false,
                    CreatedAt = new DateTime(2026, 3, 28),
                    CategoryId = 1
                },
                new TaskItem
                {
                    Id = 2,
                    Title = "Prepare report",
                    Description = "Weekly update",
                    IsCompleted = false,
                    CreatedAt = new DateTime(2026, 3, 28),
                    CategoryId = 2
                }
            );
        }
    }
}