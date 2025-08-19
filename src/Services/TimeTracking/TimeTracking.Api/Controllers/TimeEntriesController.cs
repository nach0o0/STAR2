using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracking.Application.Common.Enums;
using TimeTracking.Application.Features.Commands.AnonymizeTimeEntry;
using TimeTracking.Application.Features.Commands.CreateTimeEntry;
using TimeTracking.Application.Features.Commands.DeleteTimeEntry;
using TimeTracking.Application.Features.Commands.UpdateTimeEntry;
using TimeTracking.Application.Features.Queries.GetAnonymizedTimeEntry;
using TimeTracking.Application.Features.Queries.GetMyTimeEntriesByDateRange;
using TimeTracking.Application.Features.Queries.GetTimeEntriesByCostObject;
using TimeTracking.Application.Features.Queries.GetTimeEntriesByGroup;
using TimeTracking.Application.Features.Queries.GetTimeEntryById;
using TimeTracking.Application.Features.Queries.GetTimeTrackingSummary;
using TimeTracking.Contracts.Requests;
using TimeTracking.Contracts.Responses;
using TimeTracking.Domain.Authorization;

namespace TimeTracking.Api.Controllers
{
    [ApiController]
    [Route("api/time-entries")]
    [Authorize]
    public class TimeEntriesController : ControllerBase
    {
        private readonly ISender _sender;

        public TimeEntriesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTimeEntryRequest request)
        {
            var command = new CreateTimeEntryCommand(
                null,
                request.CostObjectId,
                request.EntryDate,
                request.Hours,
                request.HourlyRate,
                request.Description,
                request.CreateAnonymously
            );

            var result = await _sender.Send(command);

            var response = new CreateTimeEntryResponse(result.TimeEntryId, result.AccessKey);
            return StatusCode(201, response);
        }

        [HttpPut("{timeEntryId:guid}")]
        public async Task<IActionResult> Update(Guid timeEntryId, UpdateTimeEntryRequest request)
        {
            var command = new UpdateTimeEntryCommand(
                timeEntryId,
                request.EntryDate,
                request.CostObjectId,
                request.Hours,
                request.Description,
                request.AccessKey
            );

            await _sender.Send(command);

            return NoContent();
        }

        [HttpDelete("{timeEntryId:guid}")]
        public async Task<IActionResult> Delete(Guid timeEntryId, [FromBody] DeleteTimeEntryRequest request)
        {
            var command = new DeleteTimeEntryCommand(timeEntryId, request.AccessKey);
            await _sender.Send(command);

            return NoContent();
        }

        [HttpPatch("{timeEntryId:guid}/anonymize")]
        public async Task<IActionResult> Anonymize(Guid timeEntryId)
        {
            var command = new AnonymizeTimeEntryCommand(timeEntryId);
            var result = await _sender.Send(command);

            var response = new AnonymizeTimeEntryResponse(result.TimeEntryId, result.AccessKey);

            return Ok(response);
        }

        [HttpPost("my-entries-by-range")]
        public async Task<IActionResult> GetMyEntriesByRange([FromBody] GetMyTimeEntriesRequest request)
        {
            var query = new GetMyTimeEntriesByDateRangeQuery(request.StartDate, request.EndDate, request.AccessKeys);
            var result = await _sender.Send(query);

            var response = result.Select(r => new MyTimeEntryResponse(
                r.Id,
                r.EntryDate,
                r.Hours,
                r.Description,
                r.CostObjectId,
                r.IsAnonymized,
                r.AccessKey
            ));

            return Ok(response);
        }

        [HttpGet("{timeEntryId:guid}")]
        public async Task<IActionResult> GetById(Guid timeEntryId)
        {
            var query = new GetTimeEntryByIdQuery(timeEntryId);
            var result = await _sender.Send(query);

            if (result is null)
            {
                return NotFound();
            }

            var response = new TimeEntryResponse(
                result.Id,
                result.EntryDate,
                result.Hours,
                result.HourlyRate,
                result.Description,
                result.CostObjectId,
                result.EmployeeGroupId,
                result.EmployeeId
            );

            return Ok(response);
        }

        [HttpPost("anonymized")]
        public async Task<IActionResult> GetAnonymized([FromBody] GetAnonymizedTimeEntryRequest request)
        {
            var query = new GetAnonymizedTimeEntryQuery(request.AccessKey);
            var result = await _sender.Send(query);

            if (result is null)
            {
                return NotFound();
            }

            var response = new AnonymizedTimeEntryResponse(
                result.Id,
                result.EntryDate,
                result.Hours,
                result.HourlyRate,
                result.Description,
                result.CostObjectId,
                result.EmployeeGroupId
            );

            return Ok(response);
        }

        [HttpGet("by-group")]
        [Authorize(Policy = TimeEntryPermissions.ReadAll)]
        public async Task<IActionResult> GetByGroup(
            [FromQuery] Guid employeeGroupId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var query = new GetTimeEntriesByGroupQuery(employeeGroupId, startDate, endDate);
            var result = await _sender.Send(query);

            var response = result.Select(r => new TimeEntriesByGroupResponse(
                r.Id,
                r.EmployeeId,
                r.EntryDate,
                r.Hours,
                r.Description,
                r.CostObjectId,
                r.CostObjectName
            ));

            return Ok(response);
        }

        [HttpGet("by-cost-object")]
        [Authorize(Policy = TimeEntryPermissions.ReadAll)]
        public async Task<IActionResult> GetByCostObject(
            [FromQuery] Guid costObjectId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var query = new GetTimeEntriesByCostObjectQuery(costObjectId, startDate, endDate);
            var result = await _sender.Send(query);

            var response = result.Select(r => new TimeEntriesByCostObjectResponse(
                r.Id,
                r.EmployeeId,
                r.EntryDate,
                r.Hours,
                r.Description
            ));

            return Ok(response);
        }

        [HttpGet("summary-by-group")]
        [Authorize(Policy = TimeEntryPermissions.ReadAll)]
        public async Task<IActionResult> GetSummaryByGroup(
            [FromQuery] Guid employeeGroupId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] SummaryGroupBy groupBy = SummaryGroupBy.CostObject)
        {
            var query = new GetTimeTrackingSummaryQuery(employeeGroupId, startDate, endDate, groupBy);
            var result = await _sender.Send(query);

            var response = result.Select(r => new TimeTrackingSummaryResponse(
                r.GroupingId,
                r.GroupingName,
                r.TotalHours
            ));

            return Ok(response);
        }
    }
}
