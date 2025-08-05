using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Session.Application.Features.Commands.CreateSession;
using Session.Application.Features.Commands.RefreshToken;
using Session.Application.Features.Commands.RevokeSession;
using Session.Application.Features.Queries.ListMySessions;
using Session.Contracts.Requests;
using Session.Contracts.Responses;

namespace Session.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionsController : ControllerBase
    {
        private readonly ISender _sender;

        public SessionsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSession(CreateSessionRequest request)
        {
            var clientInfo = HttpContext.Request.Headers.UserAgent.ToString();

            var command = new CreateSessionCommand(request.BasicToken, clientInfo);

            var (accessToken, refreshToken) = await _sender.Send(command);

            var response = new SessionTokensResponse(accessToken, refreshToken);

            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenRequest request)
        {
            var command = new RefreshTokenCommand(request.RefreshToken);
            var (accessToken, refreshToken) = await _sender.Send(command);

            var response = new SessionTokensResponse(accessToken, refreshToken);
            return Ok(response);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RevokeSession(RevokeSessionRequest request)
        {
            var command = new RevokeSessionCommand(request.RefreshToken);
            await _sender.Send(command);
            return NoContent();
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMySessions()
        {
            var query = new ListMySessionsQuery();
            var result = await _sender.Send(query);

            var response = result.Select(s => new ActiveSessionResponse(s.SessionId, s.CreatedAt, s.ClientInfo));
            return Ok(response);
        }
    }
}
