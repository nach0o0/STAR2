using CostObject.Application.Features.Commands.CreateHierarchyDefinition;
using CostObject.Application.Features.Commands.CreateHierarchyLevel;
using CostObject.Application.Features.Commands.DeleteHierarchyDefinition;
using CostObject.Application.Features.Commands.DeleteHierarchyLevel;
using CostObject.Application.Features.Commands.UpdateHierarchyDefinition;
using CostObject.Application.Features.Commands.UpdateHierarchyLevel;
using CostObject.Application.Features.Queries.GetHierarchyDefinitionsByGroup;
using CostObject.Application.Features.Queries.GetHierarchyLevelsByDefinition;
using CostObject.Contracts.Requests;
using CostObject.Contracts.Responses;
using CostObject.Domain.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CostObject.Api.Controllers
{
    [ApiController]
    [Route("api/hierarchies")]
    [Authorize]
    public class HierarchyController : ControllerBase
    {
        private readonly ISender _sender;

        public HierarchyController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("levels")]
        [Authorize(Policy = HierarchyPermissions.Manage)]
        public async Task<IActionResult> CreateLevel(CreateHierarchyLevelRequest request)
        {
            var command = new CreateHierarchyLevelCommand(
                request.Name,
                request.Depth,
                request.HierarchyDefinitionId);

            var levelId = await _sender.Send(command);

            var response = new CreateHierarchyLevelResponse(levelId);
            return StatusCode(201, response);
        }

        [HttpPut("levels/{levelId:guid}")]
        [Authorize(Policy = HierarchyPermissions.Manage)]
        public async Task<IActionResult> UpdateLevel(Guid levelId, UpdateHierarchyLevelRequest request)
        {
            var command = new UpdateHierarchyLevelCommand(
                levelId,
                request.Name,
                request.Depth);

            await _sender.Send(command);

            return NoContent();
        }

        [HttpDelete("levels/{levelId:guid}")]
        [Authorize(Policy = HierarchyPermissions.Manage)]
        public async Task<IActionResult> DeleteLevel(Guid levelId)
        {
            var command = new DeleteHierarchyLevelCommand(levelId);
            await _sender.Send(command);

            return NoContent();
        }

        [HttpPost("definitions")]
        [Authorize(Policy = HierarchyPermissions.Manage)]
        public async Task<IActionResult> CreateDefinition(CreateHierarchyDefinitionRequest request)
        {
            var command = new CreateHierarchyDefinitionCommand(
                request.Name,
                request.EmployeeGroupId,
                request.RequiredBookingLevelId);

            var definitionId = await _sender.Send(command);

            var response = new CreateHierarchyDefinitionResponse(definitionId);
            return StatusCode(201, response);
        }

        [HttpPut("definitions/{definitionId:guid}")]
        [Authorize(Policy = HierarchyPermissions.Manage)]
        public async Task<IActionResult> UpdateDefinition(Guid definitionId, UpdateHierarchyDefinitionRequest request)
        {
            var command = new UpdateHierarchyDefinitionCommand(
                definitionId,
                request.Name,
                request.RequiredBookingLevelId);

            await _sender.Send(command);

            return NoContent();
        }

        [HttpDelete("definitions/{definitionId:guid}")]
        [Authorize(Policy = HierarchyPermissions.Manage)]
        public async Task<IActionResult> DeleteDefinition(Guid definitionId)
        {
            var command = new DeleteHierarchyDefinitionCommand(definitionId);
            await _sender.Send(command);

            return NoContent();
        }

        [HttpGet("definitions")]
        public async Task<IActionResult> GetDefinitionsByGroup([FromQuery] Guid employeeGroupId)
        {
            var query = new GetHierarchyDefinitionsByGroupQuery(employeeGroupId);
            var result = await _sender.Send(query);

            var response = result.Select(r => new HierarchyDefinitionResponse(
                r.Id,
                r.Name,
                r.RequiredBookingLevelId
            ));

            return Ok(response);
        }

        [HttpGet("definitions/{definitionId:guid}/levels")]
        public async Task<IActionResult> GetLevelsByDefinition(Guid definitionId)
        {
            var query = new GetHierarchyLevelsByDefinitionQuery(definitionId);
            var result = await _sender.Send(query);

            var response = result.Select(r => new HierarchyLevelResponse(
                r.Id,
                r.Name,
                r.Depth
            ));

            return Ok(response);
        }
    }
}
