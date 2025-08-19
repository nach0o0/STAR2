using CostObject.Application.Features.Commands.ApproveCostObjectRequest;
using CostObject.Application.Features.Commands.CreateCostObject;
using CostObject.Application.Features.Commands.DeactivateCostObject;
using CostObject.Application.Features.Commands.RejectCostObjectRequest;
using CostObject.Application.Features.Commands.SyncTopLevelCostObjects;
using CostObject.Application.Features.Commands.UpdateCostObject;
using CostObject.Application.Features.Queries.GetActiveCostObjectsByGroup;
using CostObject.Application.Features.Queries.GetChildCostObjects;
using CostObject.Application.Features.Queries.GetCostObjectById;
using CostObject.Application.Features.Queries.GetCostObjectHierarchy;
using CostObject.Application.Features.Queries.GetCostObjectsByGroup;
using CostObject.Application.Features.Queries.GetTopLevelCostObjectsByGroup;
using CostObject.Contracts.Requests;
using CostObject.Contracts.Responses;
using CostObject.Domain.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CostObject.Api.Controllers
{
    [ApiController]
    [Route("api/cost-objects")]
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

        [HttpPost("sync-top-level")]
        [Authorize(Policy = CostObjectPermissions.Sync)]
        public async Task<IActionResult> SyncTopLevel(SyncTopLevelCostObjectsRequest request)
        {
            var command = new SyncTopLevelCostObjectsCommand(
                request.EmployeeGroupId,
                request.Names,
                request.ValidFrom,
                request.TopHierarchyLevelId
            );
            await _sender.Send(command);
            return Ok("Synchronization process completed.");
        }

        [HttpGet("{costObjectId:guid}")]
        [Authorize(Policy = CostObjectPermissions.Read)]
        public async Task<IActionResult> GetById(Guid costObjectId)
        {
            var query = new GetCostObjectByIdQuery(costObjectId);
            var result = await _sender.Send(query);

            if (result is null)
            {
                return NotFound();
            }

            var response = new CostObjectResponse(
                result.Id,
                result.Name,
                result.EmployeeGroupId,
                result.ParentCostObjectId,
                result.HierarchyLevelId,
                result.LabelId,
                result.ValidFrom,
                result.ValidTo,
                (ApprovalStatusDto)result.ApprovalStatus // Enum-Mapping
            );

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByGroup([FromQuery] Guid employeeGroupId)
        {
            var query = new GetCostObjectsByGroupQuery(employeeGroupId);
            var result = await _sender.Send(query);

            var response = result.Select(r => new CostObjectResponse(
                r.Id, 
                r.Name, 
                r.EmployeeGroupId, 
                r.ParentCostObjectId, 
                r.HierarchyLevelId,
                r.LabelId, 
                r.ValidFrom, 
                r.ValidTo, 
                (ApprovalStatusDto)r.ApprovalStatus
            ));

            return Ok(response);
        }

        [HttpGet("active-by-group")]
        public async Task<IActionResult> GetActiveByGroup([FromQuery] Guid employeeGroupId)
        {
            var query = new GetActiveCostObjectsByGroupQuery(employeeGroupId);
            var result = await _sender.Send(query);

            var response = result.Select(r => new ActiveCostObjectResponse(
                r.Id,
                r.Name,
                r.ParentCostObjectId,
                r.Depth
            ));

            return Ok(response);
        }

        [HttpGet("{parentCostObjectId:guid}/children")]
        [Authorize(Policy = CostObjectPermissions.Read)]
        public async Task<IActionResult> GetChildren(Guid parentCostObjectId)
        {
            var query = new GetChildCostObjectsQuery(parentCostObjectId);
            var result = await _sender.Send(query);

            var response = result.Select(r => new ChildCostObjectResponse(
                r.Id,
                r.Name,
                (ApprovalStatusDto)r.ApprovalStatus,
                r.HasChildren
            ));

            return Ok(response);
        }
        [HttpGet("{rootCostObjectId:guid}/hierarchy")]
        public async Task<IActionResult> GetHierarchy(Guid rootCostObjectId)
        {
            var query = new GetCostObjectHierarchyQuery(rootCostObjectId);
            var result = await _sender.Send(query);

            if (result is null)
            {
                return NotFound();
            }

            var response = MapToHierarchyResponse(result);

            return Ok(response);
        }

        [HttpGet("top-level-by-group")]
        public async Task<IActionResult> GetTopLevelByGroup(
            [FromQuery] Guid employeeGroupId,
            [FromQuery] bool approvedOnly = false,
            [FromQuery] bool activeOnly = false)
        {
            var query = new GetTopLevelCostObjectsByGroupQuery(employeeGroupId, approvedOnly, activeOnly);
            var result = await _sender.Send(query);

            var response = result.Select(r => new TopLevelCostObjectResponse(
                r.Id,
                r.Name,
                (ApprovalStatusDto)r.ApprovalStatus
            ));

            return Ok(response);
        }






        private HierarchyCostObjectResponse MapToHierarchyResponse(GetCostObjectHierarchyQueryResult result)
        {
            return new HierarchyCostObjectResponse(
                result.Id,
                result.Name,
                (ApprovalStatusDto)result.ApprovalStatus,
                result.Children.Select(MapToHierarchyResponse).ToList()
            );
        }
    }
}
