using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Permission.Application.Features.Commands.AddPermissionToRole;
using Permission.Application.Features.Commands.CreateRole;
using Permission.Application.Features.Commands.DeleteRole;
using Permission.Application.Features.Commands.RemovePermissionFromRole;
using Permission.Application.Features.Commands.UpdateRole;
using Permission.Application.Features.Queries.GetPermissionsByRole;
using Permission.Application.Features.Queries.GetRolesByScope;
using Permission.Contracts.Requests;
using Permission.Contracts.Responses;
using Permission.Domain.Authorization;

namespace Permission.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly ISender _sender;

        public RolesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize(Policy = RolePermissions.Create)]
        public async Task<IActionResult> Create(CreateRoleRequest request)
        {
            var command = new CreateRoleCommand(request.Name, request.Description, request.Scope);
            var roleId = await _sender.Send(command);

            var response = new CreateRoleResponse(roleId);
            return StatusCode(201, response);
        }

        [HttpPut("{roleId:guid}")]
        [Authorize(Policy = RolePermissions.Update)]
        public async Task<IActionResult> Update(Guid roleId, UpdateRoleRequest request)
        {
            var command = new UpdateRoleCommand(roleId, request.Name, request.Description);
            await _sender.Send(command);
            return NoContent();
        }

        [HttpDelete("{roleId:guid}")]
        [Authorize(Policy = RolePermissions.Delete)]
        public async Task<IActionResult> Delete(Guid roleId)
        {
            var command = new DeleteRoleCommand(roleId);
            await _sender.Send(command);
            return NoContent();
        }

        [HttpPost("{roleId:guid}/permissions")]
        [Authorize(Policy = RolePermissions.AssignPermission)]
        public async Task<IActionResult> AddPermission(Guid roleId, AddPermissionToRoleRequest request)
        {
            var command = new AddPermissionToRoleCommand(roleId, request.PermissionId);
            await _sender.Send(command);
            return NoContent();
        }

        [HttpDelete("{roleId:guid}/permissions/{permissionId}")]
        [Authorize(Policy = RolePermissions.AssignPermission)]
        public async Task<IActionResult> RemovePermission(Guid roleId, string permissionId)
        {
            var command = new RemovePermissionFromRoleCommand(roleId, permissionId);
            await _sender.Send(command);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Policy = RolePermissions.Read)]
        public async Task<IActionResult> GetRolesByScope([FromQuery] string scope)
        {
            var query = new GetRolesByScopeQuery(scope);
            var queryResult = await _sender.Send(query);

            var response = queryResult.Select(r => new RoleResponse(
                r.Id,
                r.Name,
                r.Description,
                r.Permissions
            )).ToList();

            return Ok(response);
        }

        [HttpGet("{roleId:guid}/permissions")]
        [Authorize(Policy = RolePermissions.Read)]
        public async Task<IActionResult> GetPermissionsByRole(Guid roleId)
        {
            var query = new GetPermissionsByRoleQuery(roleId);
            var queryResult = await _sender.Send(query);

            var response = queryResult.Select(p => new PermissionResponse(p.Id, p.Description)).ToList();

            return Ok(response);
        }
    }
}
