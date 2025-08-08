using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Permission.Application.Features.Queries.GetPermissionsByScope;
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

        [HttpGet]
        [Authorize(Policy = PermissionPermissions.Read)]
        public async Task<IActionResult> GetPermissionsByScope([FromQuery] string scope)
        {
            var query = new GetPermissionsByScopeQuery(scope);
            var result = await _sender.Send(query);
            return Ok(result);
        }
    }
}
