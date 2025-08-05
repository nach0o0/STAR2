using Auth.Application.Features.Commands.ChangePassword;
using Auth.Application.Features.Commands.DeleteMyAccount;
using Auth.Application.Features.Commands.PrivilegedResetPassword;
using Auth.Application.Features.Queries.GetMyProfile;
using Auth.Contracts.Requests;
using Auth.Contracts.Responses;
using Auth.Domain.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var query = new GetMyProfileQuery();
            var result = await _sender.Send(query);

            var response = new MyProfileResponse(result.UserId, result.Email);
            return Ok(response);
        }

        [HttpPost("me/change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var command = new ChangePasswordCommand(request.OldPassword, request.NewPassword);
            await _sender.Send(command);
            return NoContent();
        }

        [HttpDelete("me")]
        public async Task<IActionResult> DeleteMyAccount(DeleteMyAccountRequest request)
        {
            var command = new DeleteMyAccountCommand(request.Password);
            await _sender.Send(command);
            return NoContent();
        }

        [HttpPost("{userId:guid}/privileged-reset-password")]
        [Authorize(Policy = UserPermissions.PrivilegedResetPassword)]
        public async Task<IActionResult> PrivilegedResetPassword(Guid userId, PrivilegedResetPasswordRequest request)
        {
            var command = new PrivilegedResetPasswordCommand(userId, request.NewPassword);
            await _sender.Send(command);
            return NoContent();
        }
    }
}
