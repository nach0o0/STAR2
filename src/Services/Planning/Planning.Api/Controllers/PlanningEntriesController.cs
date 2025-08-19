using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planning.Application.Common.Enums;
using Planning.Application.Features.Commands.CreatePlanningEntry;
using Planning.Application.Features.Commands.DeletePlanningEntry;
using Planning.Application.Features.Commands.UpdatePlanningEntry;
using Planning.Application.Features.Queries.GetPlanningEntriesByCostObject;
using Planning.Application.Features.Queries.GetPlanningEntriesByEmployee;
using Planning.Application.Features.Queries.GetPlanningEntriesByGroup;
using Planning.Application.Features.Queries.GetPlanningSummary;
using Planning.Contracts.Requests;
using Planning.Contracts.Responses;
using Planning.Domain.Authorization;

namespace Planning.Api.Controllers
{
    [ApiController]
    [Route("api/planning-entries")]
    [Authorize]
    public class PlanningEntriesController : ControllerBase
    {
        private readonly ISender _sender;

        public PlanningEntriesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize(Policy = PlanningPermissions.Write)]
        public async Task<IActionResult> Create(CreatePlanningEntryRequest request)
        {
            var command = new CreatePlanningEntryCommand(
                request.EmployeeGroupId,
                request.EmployeeId,
                request.CostObjectId,
                request.PlannedHours,
                request.PlanningPeriodStart,
                request.PlanningPeriodEnd,
                request.Notes
            );

            var planningEntryId = await _sender.Send(command);

            var response = new CreatePlanningEntryResponse(planningEntryId);
            return StatusCode(201, response);
        }

        [HttpPut("{planningEntryId:guid}")]
        [Authorize(Policy = PlanningPermissions.Write)]
        public async Task<IActionResult> Update(Guid planningEntryId, UpdatePlanningEntryRequest request)
        {
            var command = new UpdatePlanningEntryCommand(
                planningEntryId,
                request.PlannedHours,
                request.Notes
            );

            await _sender.Send(command);

            return NoContent();
        }

        [HttpDelete("{planningEntryId:guid}")]
        [Authorize(Policy = PlanningPermissions.Write)]
        public async Task<IActionResult> Delete(Guid planningEntryId)
        {
            var command = new DeletePlanningEntryCommand(planningEntryId);
            await _sender.Send(command);

            return NoContent();
        }

        [HttpGet("by-group")]
        [Authorize(Policy = PlanningPermissions.Read)]
        public async Task<IActionResult> GetByGroup(
            [FromQuery] Guid employeeGroupId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var query = new GetPlanningEntriesByGroupQuery(employeeGroupId, startDate, endDate);
            var result = await _sender.Send(query);

            var response = result.Select(r => new PlanningEntryResponse(
                r.Id,
                r.EmployeeId,
                r.EmployeeName,
                r.CostObjectId,
                r.CostObjectName,
                r.PlannedHours,
                r.PlanningPeriodStart,
                r.PlanningPeriodEnd,
                r.Notes
            ));

            return Ok(response);
        }

        [HttpGet("by-employee")]
        [Authorize(Policy = PlanningPermissions.Read)]
        public async Task<IActionResult> GetByEmployee(
            [FromQuery] Guid employeeId,
            [FromQuery] Guid employeeGroupId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var query = new GetPlanningEntriesByEmployeeQuery(employeeId, employeeGroupId, startDate, endDate);
            var result = await _sender.Send(query);

            var response = result.Select(r => new PlanningEntriesByEmployeeResponse(
                r.Id,
                r.CostObjectId,
                r.CostObjectName,
                r.PlannedHours,
                r.PlanningPeriodStart,
                r.PlanningPeriodEnd,
                r.Notes
            ));

            return Ok(response);
        }

        [HttpGet("by-cost-object")]
        [Authorize(Policy = PlanningPermissions.Read)]
        public async Task<IActionResult> GetByCostObject(
            [FromQuery] Guid costObjectId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var query = new GetPlanningEntriesByCostObjectQuery(costObjectId, startDate, endDate);
            var result = await _sender.Send(query);

            var response = result.Select(r => new PlanningEntriesByCostObjectResponse(
                r.Id,
                r.EmployeeId,
                r.EmployeeName,
                r.PlannedHours,
                r.PlanningPeriodStart,
                r.PlanningPeriodEnd,
                r.Notes
            ));

            return Ok(response);
        }

        [HttpGet("summary-by-group")]
        [Authorize(Policy = PlanningPermissions.Read)]
        public async Task<IActionResult> GetSummary(
            [FromQuery] Guid employeeGroupId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] SummaryGroupBy groupBy = SummaryGroupBy.CostObject)
        {
            var query = new GetPlanningSummaryQuery(employeeGroupId, startDate, endDate, groupBy);
            var result = await _sender.Send(query);

            var response = result.Select(r => new PlanningSummaryResponse(
                r.GroupingId,
                r.GroupingName,
                r.TotalHours
            ));

            return Ok(response);
        }
    }
}
