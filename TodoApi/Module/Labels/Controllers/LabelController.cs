using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Module.Labels.Dtos;
using TodoApi.Module.Entities;
using TodoApi.Module.Labels.Commands;
using TodoApi.Module.Labels.Queries;

namespace TodoApi.Controllers
{
    [Route("api/label")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly CreateLabelCommandHandlers _createLabelCommandHandlers;
        private readonly UpdateLabelCommandHandlers _updateLableCommandHandlers;
        private readonly DeleteLabelCommandHandlers _deleteLabelCommandHandlers;
        private readonly GetAllLabelsQueryHandler _getAllLabelQueryHandler;
        private readonly GetLabelByIdQueryHandler _getLabelByIdQueryHandler;

        public LabelController(
            CreateLabelCommandHandlers createLabelCommandsHandlers,
            UpdateLabelCommandHandlers updateLabelCommandHandlers,
            DeleteLabelCommandHandlers deleteLabelCommandHandlers,
            GetAllLabelsQueryHandler getAllLabelsQueryHandler,
            GetLabelByIdQueryHandler getLabelByIdQueryHandler
        )
        {
            _createLabelCommandHandlers = createLabelCommandsHandlers;
            _updateLableCommandHandlers = updateLabelCommandHandlers;
            _deleteLabelCommandHandlers = deleteLabelCommandHandlers;
            _getAllLabelQueryHandler = getAllLabelsQueryHandler;
            _getLabelByIdQueryHandler = getLabelByIdQueryHandler;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var labels = await _getAllLabelQueryHandler.HandleAsync();
            return Ok(labels);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var label = await _getLabelByIdQueryHandler.HandleAsync(id);
            if (label == null)
                return NotFound();
            return Ok(label);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLabelCommand command)
        {
            var label = await _createLabelCommandHandlers.HandleAsync(command);
            return CreatedAtAction(nameof(GetById), new { id = label.LabelId }, label);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateLabelCommand command)
        {
            command.Id = id;
            var label = await _updateLableCommandHandlers.HandleAsync(command);
            if (label == null)
            {
                return NotFound();
            }
            return Ok(label);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, DeleteLabelCommand command)
        {
            command.Id = id;
            var label = await _deleteLabelCommandHandlers.HandleAsync(command);
            if (!label)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
