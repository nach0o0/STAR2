using Attendance.Application.Features.Commands.CreateOrUpdateAttendanceEntry;
using Attendance.Application.Features.Commands.DeleteAttendanceEntry;
using Attendance.Application.Features.Queries.GetAttendanceEntriesForEmployeeByDateRange;
using Attendance.Contracts.Requests;
using Attendance.Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Api.Controllers
{
    [ApiController]
    [Route("api/attendance-entries")]
    [Authorize]
    public class AttendanceEntriesController : ControllerBase
    {
        private readonly ISender _sender;

        public AttendanceEntriesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPut] // PUT ist ideal für "Upsert"-Operationen
        public async Task<IActionResult> CreateOrUpdate(Guid employeeId, CreateOrUpdateAttendanceEntryRequest request)
        {
            var command = new CreateOrUpdateAttendanceEntryCommand(
                employeeId,
                request.Date,
                request.AttendanceTypeId,
                request.Note
            );

            var attendanceEntryId = await _sender.Send(command);

            var response = new CreateOrUpdateAttendanceEntryResponse(attendanceEntryId);
            return Ok(response);
        }

        [HttpDelete("{attendanceEntryId:guid}")]
        public async Task<IActionResult> Delete(Guid attendanceEntryId)
        {
            var command = new DeleteAttendanceEntryCommand(attendanceEntryId);
            await _sender.Send(command);

            return NoContent();
        }

        [HttpGet("{employeeId:guid}")]
        public async Task<IActionResult> GetForEmployeeByDateRange(
            Guid employeeId,
            [FromQuery] List<Guid> employeeGroupIds,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var query = new GetAttendanceEntriesForEmployeeByDateRangeQuery(employeeId, employeeGroupIds, startDate, endDate);
            var result = await _sender.Send(query);

            var response = result.Select(e => new AttendanceEntryResponse(
                e.Id,
                e.Date,
                e.AttendanceTypeId,
                e.AttendanceTypeName,
                e.AttendanceTypeColor,
                e.Note
            ));

            return Ok(response);
        }
    }
}
