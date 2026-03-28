using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;
using TodoApi.Models;
using TodoApi.Dtos.Label;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TodoApi.Controllers
{
    [Route("api/label")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public LabelController(ApplicationDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Label = await _context.Labels.ToListAsync();
            return Ok(Label);

        }

        [HttpGet]
        [Route("{id:int}")]

        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var Label = await _context.Labels.FindAsync(id);
            if (Label == null)
            {
                return NotFound();
            }
            return Ok(Label);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLabelRequestDto LabelDto)
        {
            var Label = new Models.Label
            {
                Name = LabelDto.Name
            };
            _context.Labels.Add(Label);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = Label.Id }, Label);
        }



        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateLabelRequestDto LabelDto)
        {
            var Label = await _context.Labels.FindAsync(id);
            if (Label == null)
            {
                return NotFound();
            }
            Label.Name = LabelDto.Name;
            await _context.SaveChangesAsync();
            return Ok(Label);
        }

        [HttpDelete]
        [Route("{id:int}")]

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var Label = await _context.Labels.FindAsync(id);
            if (Label == null)
            {
                return NotFound();
            }
            _context.Labels.Remove(Label);
            await _context.SaveChangesAsync();
            return NoContent();

        }
    }
}