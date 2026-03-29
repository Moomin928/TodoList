using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dtos.TaskItem
{
    public class UpdateTaskItemRequestDto
    {
        [Required]
        [MaxLength(20, ErrorMessage = "Title cannot be over 20 characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(200, ErrorMessage = "Description cannot be over 200 characters")]
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
        public int? CategoryId { get; set; }
        public int? LabelId { get; set; }
    }
}
