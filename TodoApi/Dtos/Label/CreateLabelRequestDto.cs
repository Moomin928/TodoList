using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dtos.Label
{
    public class CreateLabelRequestDto
    {
        [Required]
        [MaxLength(20, ErrorMessage = "Name cannot be over 20 characters")]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public string Color { get; set; } = string.Empty;
    }
}
