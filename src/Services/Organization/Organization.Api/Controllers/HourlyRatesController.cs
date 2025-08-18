using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Features.Commands.AssignHourlyRateToEmployee;
using Organization.Application.Features.Commands.CreateHourlyRate;
using Organization.Application.Features.Commands.DeleteHourlyRate;
using Organization.Application.Features.Commands.UpdateHourlyRate;
using Organization.Application.Features.Queries.GetHourlyRateById;
using Organization.Application.Features.Queries.GetHourlyRatesByOrganization;
using Organization.Contracts.Requests;
using Organization.Contracts.Responses;
using Organization.Domain.Authorization;

namespace Organization.Api.Controllers
{
    [ApiController]
    [Route("api/hourly-rates")]
    [Authorize]
    public class HourlyRatesController : ControllerBase
    {
        private readonly ISender _sender;

        public HourlyRatesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Authorize(Policy = HourlyRatePermissions.Read)]
        public async Task<IActionResult> GetByOrganization(Guid organizationId)
        {
            var query = new GetHourlyRatesByOrganizationQuery(organizationId);
            var result = await _sender.Send(query);

            var response = result.Select(hr => new HourlyRateResponse(
                hr.Id,
                hr.Name,
                hr.Rate,
                hr.ValidFrom,
                hr.ValidTo,
                hr.Description
            ));

            return Ok(response);
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

        [HttpGet("{hourlyRateId:guid}")]
        [Authorize(Policy = HourlyRatePermissions.Read)]
        public async Task<IActionResult> GetById(Guid hourlyRateId)
        {
            var query = new GetHourlyRateByIdQuery(hourlyRateId);
            var result = await _sender.Send(query);

            if (result is null)
            {
                return NotFound();
            }

            var response = new HourlyRateResponse(
                result.Id,
                result.Name,
                result.Rate,
                result.ValidFrom,
                result.ValidTo,
                result.Description
            );

            return Ok(response);
        }
    }
}
