using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Dtos.Label;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/label")]
    [ApiController]
    public class LabelController(ApplicationDBContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var labels = await context.Labels.ToListAsync();
            return Ok(labels);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var label = await context.Labels.FindAsync(id);
            if (label == null)
                return NotFound();
            return Ok(label);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLabelRequestDto dto)
        {
            var label = new Label
            {
                Name = dto.Name,
                Description = dto.Description,
                Color = dto.Color
            };
            context.Labels.Add(label);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = label.Id }, label);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateLabelRequestDto dto)
        {
            var label = await context.Labels.FindAsync(id);
            if (label == null)
                return NotFound();
            label.Name = dto.Name;
            label.Description = dto.Description;
            label.Color = dto.Color;
            await context.SaveChangesAsync();
            return Ok(label);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var label = await context.Labels.FindAsync(id);
            if (label == null)
                return NotFound();
            context.Labels.Remove(label);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
