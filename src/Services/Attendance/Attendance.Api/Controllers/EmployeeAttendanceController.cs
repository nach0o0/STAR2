using Attendance.Application.Features.Queries.CalculateTargetWorkingHours;
using Attendance.Application.Features.Queries.GetAttendanceEntryByDate;
using Attendance.Application.Features.Queries.GetAttendanceSummaryForEmployee;
using Attendance.Application.Features.Queries.GetWorkModelAssignmentsForEmployee;
using Attendance.Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Api.Controllers
{
    [ApiController]
    [Route("api/attendance/employees/{employeeId:guid}")]
    [Authorize]
    public class EmployeeAttendanceController : ControllerBase
    {
        private readonly ISender _sender;

        public EmployeeAttendanceController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("work-model-assignments")]
        public async Task<IActionResult> GetWorkModelAssignments(Guid employeeId)
        {
            var query = new GetWorkModelAssignmentsForEmployeeQuery(employeeId);
            var result = await _sender.Send(query);

            var response = result.Select(a => new WorkModelAssignmentResponse(
                a.AssignmentId,
                employeeId,
                a.WorkModelId,
                a.WorkModelName,
                a.ValidFrom,
                a.ValidTo
            ));

            return Ok(response);
        }

        [HttpGet("entries/{date:datetime}")]
        public async Task<IActionResult> GetEntryByDate(Guid employeeId, DateTime date)
        {
            var query = new GetAttendanceEntryByDateQuery(employeeId, date);
            var result = await _sender.Send(query);

            if (result is null)
            {
                return NotFound();
            }

            var response = new AttendanceEntryResponse(
                result.Id,
                result.Date,
                result.AttendanceTypeId,
                result.AttendanceTypeName,
                result.AttendanceTypeColor,
                result.Note
            );

            return Ok(response);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary(
            Guid employeeId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var query = new GetAttendanceSummaryForEmployeeQuery(employeeId, startDate, endDate);
            var result = await _sender.Send(query);

            var responseItems = result.Select(item => new AttendanceSummaryItem(
                item.AttendanceTypeId,
                item.Name,
                item.Color,
                item.Count
            )).ToList();

            var response = new AttendanceSummaryResponse(responseItems);

            return Ok(response);
        }

        [HttpGet("target-hours")]
        public async Task<IActionResult> CalculateTargetHours(
            Guid employeeId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var query = new CalculateTargetWorkingHoursQuery(employeeId, startDate, endDate);
            var result = await _sender.Send(query);

            var response = new TargetWorkingHoursResponse(result.TargetHours);
            return Ok(response);
        }
    }
}
