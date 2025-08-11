using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Permission.Application.Features.Queries.GetAllPermissions;
using Permission.Application.Features.Queries.GetPermissionsByScope;
using Permission.Contracts.Responses;
using Permission.Domain.Authorization;

namespace Permission.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PermissionsController : ControllerBase
    {
        private readonly ISender _sender;

        public PermissionsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("all")]
        [Authorize(Policy = RolePermissions.Read)] // Schützt den Endpunkt
        public async Task<IActionResult> GetAllPermissions()
        {
            var query = new GetAllPermissionsQuery();
            var queryResult = await _sender.Send(query);

            var response = queryResult.Select(p => new PermissionResponse(p.Id, p.Description)).ToList();
            return Ok(response);
        }

        [HttpGet]
        [Authorize(Policy = PermissionPermissions.Read)]
        public async Task<IActionResult> GetPermissionsByScope([FromQuery] string scope)
        {
            var query = new GetPermissionsByScopeQuery(scope);
            var queryResult = await _sender.Send(query);

            var response = queryResult.Select(p => new PermissionResponse(p.Id, p.Description)).ToList();
            return Ok(response);
        }
    }
}
