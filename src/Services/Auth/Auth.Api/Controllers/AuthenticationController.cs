using Auth.Application.Features.Commands.RegisterUser;
using Auth.Application.Features.Queries.Login;
using Auth.Contracts.Requests;
using Auth.Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthenticationController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            var command = new RegisterUserCommand(request.Email, request.Password);
            var userId = await _sender.Send(command);

            var response = new RegisterUserResponse(userId);
            return StatusCode(201, response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var query = new LoginQuery(request.Email, request.Password);
            var token = await _sender.Send(query);

            var response = new LoginResponse(token);
            return Ok(response);
        }
    }
}
