using Attendance.Application.Features.Queries.GetAttendanceEntriesForEmployeeGroupByDateRange;
using Attendance.Application.Features.Queries.GetTodaysAbsencesByGroup;
using Attendance.Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Api.Controllers
{
    [ApiController]
    [Route("api/attendance/employee-groups/{employeeGroupId:guid}/entries")]
    [Authorize]
    public class EmployeeGroupAttendanceController : ControllerBase
    {
        private readonly ISender _sender;

        public EmployeeGroupAttendanceController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetForGroup(
            Guid employeeGroupId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var query = new GetAttendanceEntriesForEmployeeGroupByDateRangeQuery(employeeGroupId, startDate, endDate);
            var result = await _sender.Send(query);

            var response = result.Select(e => new AttendanceEntryForGroupResponse(
                e.Id,
                e.EmployeeId,
                e.Date,
                e.AttendanceTypeId,
                e.AttendanceTypeName,
                e.AttendanceTypeColor,
                e.Note
            ));

            return Ok(response);
        }

        [HttpGet("absences")]
        public async Task<IActionResult> GetAbsences(Guid employeeGroupId, [FromQuery] DateTime? date)
        {
            var queryDate = date?.Date ?? DateTime.UtcNow.Date;

            var query = new GetTodaysAbsencesByGroupQuery(employeeGroupId, queryDate);
            var result = await _sender.Send(query);

            var response = result.Select(a => new AbsenceResponse(
                a.EmployeeId,
                a.AttendanceTypeName,
                a.Note
            ));

            return Ok(response);
        }
    }
}
