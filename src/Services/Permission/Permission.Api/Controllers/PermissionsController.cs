using MediatR;
using Microsoft.AspNetCore.Mvc;
using Permission.Application.Features.Commands.RegisterPermissions;
using Permission.Contracts.Requests;

namespace Permission.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly ISender _sender;

        public PermissionsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterPermissionsRequest request)
        {
            var permissionsToRegister = request.Permissions.Select(p => (p.Id, p.Description));
            var command = new RegisterPermissionsCommand(permissionsToRegister);

            await _sender.Send(command);
            return NoContent();
        }
    }
}
