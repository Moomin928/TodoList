using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Dtos.TaskItem
{
    public class UpdateTaskItemRequestDto
    {
        [Required]
        [MaxLength(20, ErrorMessage = "Title cannot be over 20 over characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(200, ErrorMessage = "Description cannot be over 200 over characters")]
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
        
        public int? CategoryId { get; set; }
    }
}