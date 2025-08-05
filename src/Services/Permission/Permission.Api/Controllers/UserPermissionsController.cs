using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Permission.Application.Features.Commands.AssignDirectPermissionToUser;
using Permission.Application.Features.Commands.AssignRoleToUser;
using Permission.Application.Features.Commands.RemoveDirectPermissionFromUser;
using Permission.Application.Features.Commands.RemoveRoleFromUser;
using Permission.Contracts.Requests;
using Permission.Contracts.Responses;
using Permission.Domain.Authorization;

namespace Permission.Api.Controllers
{
    [ApiController]
    [Route("api/user-permissions")]
    [Authorize]
    public class UserPermissionsController : ControllerBase
    {
        private readonly ISender _sender;

        public UserPermissionsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("direct")]
        [Authorize(Policy = AssignmentPermissions.AssignDirect)]
        public async Task<IActionResult> AssignDirectPermission(AssignDirectPermissionToUserRequest request)
        {
            var command = new AssignDirectPermissionToUserCommand(
                request.UserId,
                request.PermissionId,
                request.Scope);

            var assignmentId = await _sender.Send(command);

            var response = new AssignDirectPermissionToUserResponse(assignmentId);
            return StatusCode(201, response);
        }

        [HttpDelete("direct")]
        [Authorize(Policy = AssignmentPermissions.AssignDirect)]
        public async Task<IActionResult> RemoveDirectPermission(RemoveDirectPermissionFromUserRequest request)
        {
            var command = new RemoveDirectPermissionFromUserCommand(
                request.UserId,
                request.PermissionId,
                request.Scope);

            await _sender.Send(command);

            return NoContent();
        }

        [HttpPost("roles")]
        [Authorize(Policy = AssignmentPermissions.AssignRole)]
        public async Task<IActionResult> AssignRole(AssignRoleToUserRequest request)
        {
            var command = new AssignRoleToUserCommand(request.UserId, request.RoleId, request.Scope);
            var assignmentId = await _sender.Send(command);

            var response = new AssignRoleToUserResponse(assignmentId);
            return StatusCode(201, response);
        }

        [HttpDelete("roles")]
        [Authorize(Policy = AssignmentPermissions.AssignRole)]
        public async Task<IActionResult> RemoveRole(RemoveRoleFromUserRequest request)
        {
            var command = new RemoveRoleFromUserCommand(request.UserId, request.RoleId, request.Scope);
            await _sender.Send(command);
            return NoContent();
        }
    }
}
