using CostObject.Application.Features.Commands.CreateLabel;
using CostObject.Application.Features.Commands.DeleteLabel;
using CostObject.Application.Features.Commands.UpdateLabel;
using CostObject.Application.Features.Queries.GetLabelsByGroup;
using CostObject.Contracts.Requests;
using CostObject.Contracts.Responses;
using CostObject.Domain.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CostObject.Api.Controllers
{
    [ApiController]
    [Route("api/labels")]
    [Authorize]
    public class LabelsController : ControllerBase
    {
        private readonly ISender _sender;

        public LabelsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize(Policy = LabelPermissions.Create)]
        public async Task<IActionResult> Create(CreateLabelRequest request)
        {
            var command = new CreateLabelCommand(request.Name, request.EmployeeGroupId);
            var labelId = await _sender.Send(command);

            var response = new CreateLabelResponse(labelId);
            return StatusCode(201, response);
        }

        [HttpPut("{labelId:guid}")]
        [Authorize(Policy = LabelPermissions.Update)]
        public async Task<IActionResult> Update(Guid labelId, UpdateLabelRequest request)
        {
            var command = new UpdateLabelCommand(labelId, request.Name);
            await _sender.Send(command);

            return NoContent();
        }

        [HttpDelete("{labelId:guid}")]
        [Authorize(Policy = LabelPermissions.Delete)]
        public async Task<IActionResult> Delete(Guid labelId)
        {
            var command = new DeleteLabelCommand(labelId);
            await _sender.Send(command);

            return NoContent();
        }

        [HttpGet]
        [Authorize(Policy = LabelPermissions.Read)]
        public async Task<IActionResult> GetByGroup([FromQuery] Guid employeeGroupId)
        {
            var query = new GetLabelsByGroupQuery(employeeGroupId);
            var result = await _sender.Send(query);

            var response = result.Select(r => new LabelResponse(r.Id, r.Name));

            return Ok(response);
        }
    }
}
