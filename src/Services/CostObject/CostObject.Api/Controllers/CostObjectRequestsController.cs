using CostObject.Application.Features.Commands.ApproveCostObjectRequest;
using CostObject.Application.Features.Commands.CreateCostObjectRequest;
using CostObject.Application.Features.Commands.RejectCostObjectRequest;
using CostObject.Application.Features.Queries.GetCostObjectRequestsByGroup;
using CostObject.Application.Features.Queries.GetMyCostObjectRequests;
using CostObject.Contracts.Requests;
using CostObject.Contracts.Responses;
using CostObject.Domain.Authorization;
using CostObject.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CostObject.Api.Controllers
{
    [ApiController]
    [Route("api/cost-object-requests")]
    [Authorize]
    public class CostObjectRequestsController : ControllerBase
    {
        private readonly ISender _sender;

        public CostObjectRequestsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCostObjectRequestRequest request)
        {
            var command = new CreateCostObjectRequestCommand(
                request.Name,
                request.EmployeeGroupId,
                request.ParentCostObjectId,
                request.HierarchyLevelId,
                request.LabelId,
                request.ValidFrom
            );

            var costObjectRequestId = await _sender.Send(command);

            var response = new CreateCostObjectRequestResponse(costObjectRequestId);
            return StatusCode(201, response);
        }

        [HttpPatch("{requestId:guid}/approve")]
        [Authorize(Policy = CostObjectPermissions.Approve)]
        public async Task<IActionResult> Approve(Guid requestId)
        {
            var command = new ApproveCostObjectRequestCommand(requestId);
            await _sender.Send(command);

            return NoContent();
        }

        [HttpPatch("{requestId:guid}/reject")]
        [Authorize(Policy = CostObjectPermissions.Approve)]
        public async Task<IActionResult> Reject(Guid requestId, [FromBody] RejectCostObjectRequestRequest request)
        {
            var command = new RejectCostObjectRequestCommand(
                requestId,
                request.RejectionReason,
                request.ReassignmentCostObjectId
            );

            await _sender.Send(command);

            return NoContent();
        }

        [HttpGet]
        [Authorize(Policy = CostObjectRequestPermissions.Read)]
        public async Task<IActionResult> GetByGroup([FromQuery] Guid employeeGroupId, [FromQuery] ApprovalStatus? status)
        {
            var query = new GetCostObjectRequestsByGroupQuery(employeeGroupId, status);
            var result = await _sender.Send(query);

            var response = result.Select(r => new CostObjectRequestResponse(
                r.RequestId,
                r.CostObjectId,
                r.CostObjectName,
                r.RequesterEmployeeId,
                (ApprovalStatusDto)r.Status,
                r.CreatedAt
            ));

            return Ok(response);
        }

        [HttpGet("my-requests")]
        public async Task<IActionResult> GetMyRequests([FromQuery] Guid employeeGroupId)
        {
            var query = new GetMyCostObjectRequestsQuery(employeeGroupId);
            var result = await _sender.Send(query);

            var response = result.Select(r => new MyCostObjectRequestResponse(
                r.RequestId,
                r.CostObjectId,
                r.CostObjectName,
                (ApprovalStatusDto)r.Status,
                r.CreatedAt,
                r.RejectionReason
            ));

            return Ok(response);
        }
    }
}
