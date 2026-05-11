using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace TodoApi.Data.Configurations
{
    public class LabelConfiguration : IEntityTypeConfiguration<Label>
    {
        public void Configure(EntityTypeBuilder<Label> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(l => l.Description)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(l => l.Color)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasData(
            new Label
            {
                Id = 1,
                Name = "Urgent",
                Description = "High priority items",
                Color = "#ef4444"
            },

            new Label
            {
                Id = 2,
                Name = "Optional",
                Description = "Nice to have",
                Color = "#10b981"
            }
            );
        }
    }
}