using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Dtos.Label
{
    public class CreateLabelRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Name cannot be over 10  characters")]
        public string Name { get; set; } = string.Empty;
    }
}