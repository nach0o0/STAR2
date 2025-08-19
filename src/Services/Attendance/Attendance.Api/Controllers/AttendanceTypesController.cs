using Attendance.Application.Features.Commands.CreateAttendanceType;
using Attendance.Application.Features.Commands.DeleteAttendanceType;
using Attendance.Application.Features.Commands.UpdateAttendanceType;
using Attendance.Application.Features.Queries.GetAttendanceTypesByEmployeeGroup;
using Attendance.Contracts.Requests;
using Attendance.Contracts.Responses;
using Attendance.Domain.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Api.Controllers
{
    [ApiController]
    [Route("api/attendance-types")]
    [Authorize]
    public class AttendanceTypesController : ControllerBase
    {
        private readonly ISender _sender;

        public AttendanceTypesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize(Policy = AttendanceTypePermissions.Create)]
        public async Task<IActionResult> Create(CreateAttendanceTypeRequest request)
        {
            var command = new CreateAttendanceTypeCommand(
                request.EmployeeGroupId,
                request.Name,
                request.Abbreviation,
                request.IsPresent,
                request.Color
            );

            var attendanceTypeId = await _sender.Send(command);

            var response = new CreateAttendanceTypeResponse(attendanceTypeId);
            return StatusCode(201, response);
        }

        [HttpPut("{attendanceTypeId:guid}")]
        [Authorize(Policy = AttendanceTypePermissions.Update)]
        public async Task<IActionResult> Update(Guid attendanceTypeId, UpdateAttendanceTypeRequest request)
        {
            var command = new UpdateAttendanceTypeCommand(
                attendanceTypeId,
                request.Name,
                request.Abbreviation,
                request.IsPresent,
                request.Color
            );

            await _sender.Send(command);

            return NoContent();
        }

        [HttpDelete("{attendanceTypeId:guid}")]
        [Authorize(Policy = AttendanceTypePermissions.Delete)]
        public async Task<IActionResult> Delete(Guid attendanceTypeId)
        {
            var command = new DeleteAttendanceTypeCommand(attendanceTypeId);
            await _sender.Send(command);

            return NoContent();
        }

        [HttpGet]
        [Authorize(Policy = AttendanceTypePermissions.Read)]
        public async Task<IActionResult> GetByGroup([FromQuery] Guid employeeGroupId)
        {
            var query = new GetAttendanceTypesByEmployeeGroupQuery(employeeGroupId);
            var result = await _sender.Send(query);

            var response = result.Select(at => new AttendanceTypeResponse(
                at.Id,
                at.Name,
                at.Abbreviation,
                at.IsPresent,
                at.Color
            ));

            return Ok(response);
        }
    }
}
