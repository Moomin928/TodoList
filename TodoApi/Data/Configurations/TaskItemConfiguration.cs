using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApi.Models;

namespace TodoApi.Data.Configurations
{
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(t => t.Description)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(t => t.IsCompleted)
                   .IsRequired();


            builder.HasOne(t => t.Label)
                   .WithMany(l => l.TaskItems)
                   .HasForeignKey(t => t.LabelId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasData(
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