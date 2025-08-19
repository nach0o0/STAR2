using Attendance.Application.Features.Commands.CreatePublicHoliday;
using Attendance.Application.Features.Commands.DeletePublicHoliday;
using Attendance.Application.Features.Queries.GetPublicHolidaysByDateRange;
using Attendance.Contracts.Requests;
using Attendance.Contracts.Responses;
using Attendance.Domain.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Api.Controllers
{
    [ApiController]
    [Route("api/public-holidays")]
    [Authorize]
    public class PublicHolidaysController : ControllerBase
    {
        private readonly ISender _sender;

        public PublicHolidaysController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize(Policy = PublicHolidayPermissions.Create)]
        public async Task<IActionResult> Create(CreatePublicHolidayRequest request)
        {
            var command = new CreatePublicHolidayCommand(
                request.EmployeeGroupId,
                request.Date,
                request.Name
            );

            var publicHolidayId = await _sender.Send(command);

            var response = new CreatePublicHolidayResponse(publicHolidayId);
            return StatusCode(201, response);
        }

        [HttpDelete("{publicHolidayId:guid}")]
        [Authorize(Policy = PublicHolidayPermissions.Delete)]
        public async Task<IActionResult> Delete(Guid publicHolidayId)
        {
            var command = new DeletePublicHolidayCommand(publicHolidayId);
            await _sender.Send(command);

            return NoContent();
        }

        [HttpGet]
        [Authorize(Policy = PublicHolidayPermissions.Read)]
        public async Task<IActionResult> GetByDateRange(
            [FromQuery] List<Guid> employeeGroupIds,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var query = new GetPublicHolidaysByDateRangeQuery(employeeGroupIds, startDate, endDate);
            var result = await _sender.Send(query);

            var response = result.Select(ph => new PublicHolidayResponse(
                ph.Id,
                ph.Name,
                ph.Date,
                ph.EmployeeGroupId
            ));

            return Ok(response);
        }
    }
}
