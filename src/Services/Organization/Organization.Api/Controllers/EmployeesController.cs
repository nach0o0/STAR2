using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Features.Commands.AssignHourlyRateToEmployee;
using Organization.Application.Features.Commands.CreateMyEmployeeProfile;
using Organization.Application.Features.Commands.RemoveEmployeeFromOrganization;
using Organization.Application.Features.Commands.UpdateMyEmployeeProfile;
using Organization.Application.Features.Queries.GetMyEmployeeProfile;
using Organization.Contracts.Requests;
using Organization.Contracts.Responses;
using Organization.Domain.Authorization;

namespace Organization.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly ISender _sender;

        public EmployeesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var query = new GetMyEmployeeProfileQuery();
            var result = await _sender.Send(query);

            if (result is null)
            {
                return NotFound();
            }

            var response = new MyEmployeeProfileResponse(result.EmployeeId, result.FirstName, result.LastName);
            return Ok(response);
        }

        [HttpPost("me")]
        public async Task<IActionResult> CreateMyProfile(CreateEmployeeRequest request)
        {
            var command = new CreateMyEmployeeProfileCommand(request.FirstName, request.LastName);
            var employeeId = await _sender.Send(command);

            var response = new CreateEmployeeResponse(employeeId);
            return StatusCode(201, response);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile(UpdateEmployeeRequest request)
        {
            var command = new UpdateMyEmployeeProfileCommand(request.FirstName, request.LastName);
            await _sender.Send(command);
            return NoContent();
        }

        [HttpPut("{employeeId:guid}/groups/{employeeGroupId:guid}/hourly-rate")]
        [Authorize(Policy = EmployeePermissions.AssignHouryRate)]
        public async Task<IActionResult> AssignHourlyRate(
            Guid employeeId,
            Guid employeeGroupId,
            AssignHourlyRateToEmployeeRequest request)
        {
            var command = new AssignHourlyRateToEmployeeCommand(employeeId, employeeGroupId, request.HourlyRateId);
            await _sender.Send(command);
            return NoContent();
        }

        [HttpDelete("{employeeId:guid}/organization")]
        [Authorize(Policy = EmployeePermissions.RemoveFromOrganization)]
        public async Task<IActionResult> RemoveFromOrganization(Guid employeeId)
        {
            var command = new RemoveEmployeeFromOrganizationCommand(employeeId);
            await _sender.Send(command);
            return NoContent();
        }
    }
}
