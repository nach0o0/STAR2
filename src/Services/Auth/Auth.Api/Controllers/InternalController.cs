using Auth.Application.Features.Queries.GetUserByEmail;
using Auth.Application.Features.Queries.GetUserById;
using Auth.Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [ApiController]
    [Route("api/internal/users")]
    public class InternalController : ControllerBase
    {
        private readonly ISender _sender;

        public InternalController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("by-email")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            var query = new GetUserByEmailQuery(email);
            var queryResult = await _sender.Send(query);

            if (queryResult is null)
            {
                return NotFound();
            }

            var response = new UserResponse(queryResult.UserId, queryResult.Email);
            return Ok(response);
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var query = new GetUserByIdQuery(userId);
            var queryResult = await _sender.Send(query);

            if (queryResult is null)
            {
                return NotFound();
            }

            var response = new UserResponse(queryResult.UserId, queryResult.Email);
            return Ok(response);
        }
    }
}
