using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Features.Commands.AssignHourlyRateToEmployee;
using Organization.Application.Features.Commands.CreateHourlyRate;
using Organization.Application.Features.Commands.DeleteHourlyRate;
using Organization.Application.Features.Commands.UpdateHourlyRate;
using Organization.Contracts.Requests;
using Organization.Contracts.Responses;
using Organization.Domain.Authorization;

namespace Organization.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HourlyRatesController : ControllerBase
    {
        private readonly ISender _sender;

        public HourlyRatesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize(Policy = HourlyRatePermissions.Create)]
        public async Task<IActionResult> Create(CreateHourlyRateRequest request)
        {
            var command = new CreateHourlyRateCommand(
                request.Name,
                request.Rate,
                request.ValidFrom,
                request.OrganizationId,
                request.ValidTo,
                request.Description);

            var hourlyRateId = await _sender.Send(command);

            var response = new CreateHourlyRateResponse(hourlyRateId);
            return StatusCode(201, response);
        }

        [HttpPut("{hourlyRateId:guid}")]
        [Authorize(Policy = HourlyRatePermissions.Update)]
        public async Task<IActionResult> Update(Guid hourlyRateId, UpdateHourlyRateRequest request)
        {
            var command = new UpdateHourlyRateCommand(
                hourlyRateId,
                request.Name,
                request.Rate,
                request.ValidFrom,
                request.ValidTo,
                request.Description);

            await _sender.Send(command);
            return NoContent();
        }

        [HttpDelete("{hourlyRateId:guid}")]
        [Authorize(Policy = HourlyRatePermissions.Delete)]
        public async Task<IActionResult> Delete(Guid hourlyRateId)
        {
            var command = new DeleteHourlyRateCommand(hourlyRateId);
            await _sender.Send(command);
            return NoContent();
        }
    }
}
