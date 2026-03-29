using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions)
            : base(dbContextOptions) { }

        public DbSet<TaskItem> TaskItems { get; set; }

        public DbSet<Label> Labels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Label)
                .WithMany(l => l.TaskItems)
                .HasForeignKey(t => t.LabelId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Label>().HasData(
                new Label { Id = 1, Name = "Urgent", Description = "High priority items", Color = "#ef4444" },
                new Label { Id = 2, Name = "Optional", Description = "Nice to have", Color = "#10b981" }
            );

            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem
                {
                    Id = 1,
                    Title = "Finish homework",
                    Description = "Complete assignment",
                    IsCompleted = false,
                    CreatedAt = new DateTime(2026, 3, 28),
                    LabelId = 1
                },
                new TaskItem
                {
                    Id = 2,
                    Title = "Prepare report",
                    Description = "Weekly update",
                    IsCompleted = false,
                    CreatedAt = new DateTime(2026, 3, 28),
                    LabelId = 2
                }
            );
        }
    }
}
