using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Features.Commands.AddEmployeeToGroup;
using Organization.Application.Features.Commands.CreateEmployeeGroup;
using Organization.Application.Features.Commands.DeleteEmployeeGroup;
using Organization.Application.Features.Commands.RemoveEmployeeFromGroup;
using Organization.Application.Features.Commands.TransferEmployeeGroup;
using Organization.Application.Features.Commands.UpdateEmployeeGroup;
using Organization.Contracts.Requests;
using Organization.Contracts.Responses;
using Organization.Domain.Authorization;

namespace Organization.Api.Controllers
{
    [ApiController]
    [Route("api/employee-groups")]
    [Authorize]
    public class EmployeeGroupsController : ControllerBase
    {
        private readonly ISender _sender;

        public EmployeeGroupsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize(Policy = EmployeeGroupPermissions.Create)]
        public async Task<IActionResult> Create(CreateEmployeeGroupRequest request)
        {
            var command = new CreateEmployeeGroupCommand(request.Name, request.LeadingOrganizationId);
            var employeeGroupId = await _sender.Send(command);

            var response = new CreateEmployeeGroupResponse(employeeGroupId);
            return StatusCode(201, response);
        }

        [HttpPut("{employeeGroupId:guid}")]
        [Authorize(Policy = EmployeeGroupPermissions.Update)]
        public async Task<IActionResult> Update(Guid employeeGroupId, UpdateEmployeeGroupRequest request)
        {
            var command = new UpdateEmployeeGroupCommand(employeeGroupId, request.Name);
            await _sender.Send(command);
            return NoContent();
        }

        [HttpPatch("{employeeGroupId:guid}/transfer")]
        [Authorize(Policy = EmployeeGroupPermissions.Transfer)]
        public async Task<IActionResult> Transfer(Guid employeeGroupId, TransferEmployeeGroupRequest request)
        {
            var command = new TransferEmployeeGroupCommand(employeeGroupId, request.DestinationOrganizationId);
            await _sender.Send(command);
            return NoContent();
        }

        [HttpDelete("{employeeGroupId:guid}")]
        [Authorize(Policy = EmployeeGroupPermissions.Delete)]
        public async Task<IActionResult> Delete(Guid employeeGroupId)
        {
            var command = new DeleteEmployeeGroupCommand(employeeGroupId);
            await _sender.Send(command);
            return NoContent();
        }

        [HttpPost("{employeeGroupId:guid}/members")]
        [Authorize(Policy = EmployeePermissions.AssignToGroup)]
        public async Task<IActionResult> AddMember(Guid employeeGroupId, [FromBody] AddEmployeeToGroupRequest request)
        {
            var command = new AddEmployeeToGroupCommand(request.EmployeeId, employeeGroupId);
            await _sender.Send(command);
            return NoContent();
        }

        [HttpDelete("{employeeGroupId:guid}/members/{employeeId:guid}")]
        [Authorize(Policy = EmployeePermissions.AssignToGroup)]
        public async Task<IActionResult> RemoveMember(Guid employeeGroupId, Guid employeeId)
        {
            var command = new RemoveEmployeeFromGroupCommand(employeeId, employeeGroupId);
            await _sender.Send(command);
            return NoContent();
        }
    }
}
