using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Features.Commands.CreateOrganization;
using Organization.Application.Features.Commands.DeleteOrganization;
using Organization.Application.Features.Commands.ReassignOrganizationParent;
using Organization.Application.Features.Commands.UpdateOrganization;
using Organization.Application.Features.Queries.GetEmployeeGroupsByOrganization;
using Organization.Application.Features.Queries.GetEmployeesByOrganization;
using Organization.Application.Features.Queries.GetOrganizationById;
using Organization.Application.Features.Queries.GetOrganizationHierarchy;
using Organization.Application.Features.Queries.GetRelevantOrganizationsForUser;
using Organization.Application.Features.Queries.GetSubOrganizations;
using Organization.Application.Features.Queries.GetTopLevelOrganizations;
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

        [HttpGet("relevant")]
        public async Task<IActionResult> GetRelevantOrganizations()
        {
            var query = new GetRelevantOrganizationsForUserQuery();
            var result = await _sender.Send(query);

            var response = result.Select(r => new RelevantOrganizationResponse(r.Id, r.Name, r.IsPrimary));

            return Ok(response);
        }

        [HttpGet("{organizationId:guid}")]
        public async Task<IActionResult> GetById(Guid organizationId)
        {
            var query = new GetOrganizationByIdQuery(organizationId);
            var result = await _sender.Send(query);

            if (result is null)
            {
                return NotFound();
            }

            var response = new OrganizationDetailsResponse(
                result.Id,
                result.Name,
                result.Abbreviation,
                result.ParentOrganizationId,
                result.DefaultEmployeeGroupId
            );

            return Ok(response);
        }

        [HttpGet("{organizationId:guid}/sub-organizations")]
        [Authorize(Policy = OrganizationPermissions.Read)]
        public async Task<IActionResult> GetSubOrganizations(Guid organizationId)
        {
            var query = new GetSubOrganizationsQuery(organizationId);
            var result = await _sender.Send(query);

            var response = result.Select(org => new SubOrganizationResponse(
                org.Id,
                org.Name,
                org.Abbreviation
            ));

            return Ok(response);
        }

        [HttpGet("{organizationId:guid}/employee-groups")]
        [Authorize(Policy = EmployeeGroupPermissions.Read)]
        public async Task<IActionResult> GetEmployeeGroups(Guid organizationId)
        {
            var query = new GetEmployeeGroupsByOrganizationQuery(organizationId);
            var result = await _sender.Send(query);

            // Hier können wir das bestehende EmployeeGroupResponse-DTO wiederverwenden
            var response = result.Select(eg => new EmployeeGroupResponse(
                eg.Id,
                eg.Name,
                organizationId
            ));

            return Ok(response);
        }

        [HttpGet("top-level")]
        [Authorize(Policy = OrganizationPermissions.Read)]
        public async Task<IActionResult> GetTopLevel()
        {
            var query = new GetTopLevelOrganizationsQuery();
            var result = await _sender.Send(query);

            // Wir können das SubOrganizationResponse-DTO wiederverwenden.
            var response = result.Select(org => new SubOrganizationResponse(
                org.Id,
                org.Name,
                org.Abbreviation
            ));

            return Ok(response);
        }

        [HttpGet("{organizationId:guid}/hierarchy")]
        [Authorize(Policy = OrganizationPermissions.Read)]
        public async Task<IActionResult> GetHierarchy(Guid organizationId)
        {
            var query = new GetOrganizationHierarchyQuery(organizationId);
            var result = await _sender.Send(query);

            if (result is null)
            {
                return NotFound();
            }

            var response = MapToResponse(result);
            return Ok(response);
        }





        private OrganizationHierarchyNodeResponse MapToResponse(GetOrganizationHierarchyQueryResult result)
        {
            return new OrganizationHierarchyNodeResponse(
                result.Id,
                result.Name,
                result.Abbreviation,
                result.Children.Select(MapToResponse).ToList()
            );
        }
    }
}
