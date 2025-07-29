using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Features.Commands.AcceptInvitation;
using Organization.Application.Features.Commands.CreateInvitation;
using Organization.Application.Features.Commands.DeclineInvitation;
using Organization.Application.Features.Commands.RevokeInvitation;
using Organization.Contracts.Requests;
using Organization.Contracts.Responses;
using Organization.Domain.Authorization;

namespace Organization.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InvitationsController : ControllerBase
    {
        private readonly ISender _sender;

        public InvitationsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize(Policy = InvitationPermissions.Create)]
        public async Task<IActionResult> Create(CreateInvitationRequest request)
        {
            var command = new CreateInvitationCommand(
                request.InviteeEmployeeId,
                request.TargetEntityType,
                request.TargetEntityId);

            var invitationId = await _sender.Send(command);

            var response = new CreateInvitationResponse(invitationId);
            return StatusCode(201, response);
        }

        [HttpPost("{invitationId:guid}/accept")]
        public async Task<IActionResult> Accept(Guid invitationId)
        {
            var command = new AcceptInvitationCommand(invitationId);
            await _sender.Send(command);
            return NoContent();
        }

        [HttpPost("{invitationId:guid}/decline")]
        public async Task<IActionResult> Decline(Guid invitationId)
        {
            var command = new DeclineInvitationCommand(invitationId);
            await _sender.Send(command);
            return NoContent();
        }

        [HttpDelete("{invitationId:guid}")]
        public async Task<IActionResult> Revoke(Guid invitationId)
        {
            var command = new RevokeInvitationCommand(invitationId);
            await _sender.Send(command);
            return NoContent();
        }
    }
}
