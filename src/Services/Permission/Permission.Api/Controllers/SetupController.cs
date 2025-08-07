using MediatR;
using Microsoft.AspNetCore.Mvc;
using Permission.Application.Features.Commands.SeedInitialAdmin;
using Permission.Contracts.Requests;

namespace Permission.Api.Controllers
{
    [ApiController]
    [Route("api/internal/setup")]
    public class SetupController : ControllerBase
    {
        private readonly ISender _sender;

        public SetupController(ISender sender)
        {
            _sender = sender;
        }

        // ACHTUNG: Dieser Endpunkt hat KEIN [Authorize]-Attribut.
        // Die Sicherheit wird durch das InternalApiGateway gewährleistet.
        [HttpPost("initial-admin")]
        public async Task<IActionResult> SeedInitialAdmin(SeedInitialAdminRequest request)
        {
            // Das DTO wird in den Command umgewandelt
            var command = new SeedInitialAdminCommand(request.UserId, request.RoleId, request.Scope);
            await _sender.Send(command);
            return Ok("Initial admin seeded successfully.");
        }
    }
}
