using MediatR;
using Microsoft.AspNetCore.Mvc;
using Session.Application.Features.Commands.CreateSession;
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
    }
}
