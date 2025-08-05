using MediatR;
using Microsoft.AspNetCore.Mvc;
using Permission.Application.Features.Commands.RegisterPermissions;
using Permission.Application.Features.Queries.GetPermissionsForUser;
using Permission.Contracts.Requests;
using Permission.Contracts.Responses;

namespace Permission.Api.Controllers
{
    [ApiController]
    [Route("api/internal")]
    public class InternalController : ControllerBase
    {
        private readonly ISender _sender;

        public InternalController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("user-permissions")]
        public async Task<IActionResult> GetPermissionsForUser(GetUserPermissionsRequest request)
        {
            var query = new GetPermissionsForUserQuery(request.UserId, request.Scopes);
            var permissionsDictionary = await _sender.Send(query);

            var response = new GetUserPermissionsResponse(permissionsDictionary);
            return Ok(response);
        }

        [HttpPost("permissions/register")]
        public async Task<IActionResult> Register(RegisterPermissionsRequest request)
        {
            var permissionsToRegister = request.Permissions
                .Select(p => (p.Id, p.Description, p.PermittedScopeTypes));
            var command = new RegisterPermissionsCommand(permissionsToRegister);

            await _sender.Send(command);
            return NoContent();
        }
    }
}
