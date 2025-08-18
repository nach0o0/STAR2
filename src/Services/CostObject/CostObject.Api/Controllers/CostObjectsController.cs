using CostObject.Application.Features.Commands.CreateCostObject;
using CostObject.Application.Features.Commands.DeactivateCostObject;
using CostObject.Application.Features.Commands.UpdateCostObject;
using CostObject.Contracts.Requests;
using CostObject.Contracts.Responses;
using CostObject.Domain.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CostObject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CostObjectsController : ControllerBase
    {
        private readonly ISender _sender;

        public CostObjectsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize(Policy = CostObjectPermissions.Create)]
        public async Task<IActionResult> Create(CreateCostObjectRequest request)
        {
            var command = new CreateCostObjectCommand(
                request.Name,
                request.EmployeeGroupId,
                request.ParentCostObjectId,
                request.HierarchyLevelId,
                request.LabelId,
                request.ValidFrom
            );

            var costObjectId = await _sender.Send(command);

            var response = new CreateCostObjectResponse(costObjectId);
            return StatusCode(201, response);
        }

        [HttpPut("{costObjectId:guid}")]
        [Authorize(Policy = CostObjectPermissions.Update)]
        public async Task<IActionResult> Update(Guid costObjectId, UpdateCostObjectRequest request)
        {
            var command = new UpdateCostObjectCommand(
                costObjectId,
                request.Name,
                request.ParentCostObjectId,
                request.HierarchyLevelId,
                request.LabelId
            );

            await _sender.Send(command);

            return NoContent();
        }

        [HttpPatch("{costObjectId:guid}/deactivate")]
        [Authorize(Policy = CostObjectPermissions.Deactivate)]
        public async Task<IActionResult> Deactivate(Guid costObjectId, DeactivateCostObjectRequest request)
        {
            var command = new DeactivateCostObjectCommand(costObjectId, request.ValidTo);
            await _sender.Send(command);

            return NoContent();
        }
    }
}
