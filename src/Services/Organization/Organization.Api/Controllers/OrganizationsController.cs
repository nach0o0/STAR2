using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Features.Commands.CreateOrganization;
using Organization.Application.Features.Commands.DeleteOrganization;
using Organization.Application.Features.Commands.ReassignOrganizationParent;
using Organization.Application.Features.Commands.UpdateOrganization;
using Organization.Application.Features.Queries.GetEmployeesByOrganization;
using Organization.Contracts.Requests;
using Organization.Contracts.Responses;
using Organization.Domain.Authorization;

namespace Organization.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrganizationsController : ControllerBase
    {
        private readonly ISender _sender;

        public OrganizationsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize(Policy = OrganizationPermissions.Create)]
        public async Task<IActionResult> Create(CreateOrganizationRequest request)
        {
            var command = new CreateOrganizationCommand(
                request.Name,
                request.Abbreviation,
                request.ParentOrganizationId);

            var organizationId = await _sender.Send(command);

            var response = new CreateOrganizationResponse(organizationId);
            return StatusCode(201, response);
        }

        [HttpPut("{organizationId:guid}")]
        [Authorize(Policy = OrganizationPermissions.Update)]
        public async Task<IActionResult> Update(Guid organizationId, UpdateOrganizationRequest request)
        {
            var command = new UpdateOrganizationCommand(
                organizationId,
                request.Name,
                request.Abbreviation);

            await _sender.Send(command);

            return NoContent();
        }

        [HttpDelete("{organizationId:guid}")]
        [Authorize(Policy = OrganizationPermissions.Delete)]
        public async Task<IActionResult> Delete(
            Guid organizationId,
            [FromQuery] bool deleteSubOrganizations = false) // Liest den Parameter aus der URL, z.B. ?deleteSubOrganizations=true
        {
            var command = new DeleteOrganizationCommand(organizationId, deleteSubOrganizations);
            await _sender.Send(command);
            return NoContent();
        }

        [HttpPatch("{organizationId:guid}/parent")]
        [Authorize(Policy = OrganizationPermissions.ReassignToParent)]
        public async Task<IActionResult> ReassignParent(Guid organizationId, ReassignOrganizationToParentRequest request)
        {
            var command = new ReassignOrganizationToParentCommand(organizationId, request.NewParentId);
            await _sender.Send(command);
            return NoContent();
        }

        [HttpGet("{organizationId:guid}/employees")]
        public async Task<IActionResult> GetEmployees(Guid organizationId)
        {
            var query = new GetEmployeesByOrganizationQuery(organizationId);
            var result = await _sender.Send(query);

            var response = result.Select(e => new EmployeeResponse(
                e.Id,
                e.FirstName,
                e.LastName,
                e.UserId,
                organizationId
            ));

            return Ok(response);
        }
    }
}
