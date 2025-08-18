using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Features.Commands.AcceptInvitation;
using Organization.Application.Features.Commands.CreateInvitation;
using Organization.Application.Features.Commands.DeclineInvitation;
using Organization.Application.Features.Commands.RevokeInvitation;
using Organization.Application.Features.Queries.GetInvitationById;
using Organization.Application.Features.Queries.GetInvitationsByTargetEntity;
using Organization.Application.Features.Queries.GetMyPendingInvitations;
using Organization.Application.Features.Queries.GetMySentInvitations;
using Organization.Contracts.Requests;
using Organization.Contracts.Responses;
using Organization.Domain.Authorization;
using Shared.Enums;

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
                request.InviteeEmail,
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

        [HttpGet]
        [Authorize(Policy = InvitationPermissions.Read)]
        public async Task<IActionResult> GetByTarget(
            [FromQuery] InvitationTargetEntityType targetType,
            [FromQuery] Guid targetId)
        {
            var query = new GetInvitationsByTargetEntityQuery(targetType, targetId);
            var result = await _sender.Send(query);

            var response = result.Select(i => new InvitationResponse(
                i.InvitationId,
                i.InviteeEmployeeId,
                i.ExpiresAt
            ));

            return Ok(response);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyPending()
        {
            var query = new GetMyPendingInvitationsQuery();
            var result = await _sender.Send(query);

            var response = result.Select(i => new MyInvitationResponse(
                i.InvitationId,
                i.InviterEmployeeId,
                i.TargetType,
                i.TargetId,
                i.ExpiresAt
            ));

            return Ok(response);
        }

        [HttpGet("sent-by-me")]
        public async Task<IActionResult> GetMySent()
        {
            var query = new GetMySentInvitationsQuery();
            var result = await _sender.Send(query);

            var response = result.Select(i => new MySentInvitationResponse(
                i.InvitationId,
                i.InviteeEmployeeId,
                i.TargetType,
                i.TargetId,
                (InvitationStatusDto)i.Status, // Cast auf das DTO-Enum
                i.ExpiresAt
            ));

            return Ok(response);
        }

        [HttpGet("{invitationId:guid}")]
        public async Task<IActionResult> GetById(Guid invitationId)
        {
            var query = new GetInvitationByIdQuery(invitationId);
            var result = await _sender.Send(query);

            if (result is null)
            {
                return NotFound();
            }

            var response = new InvitationDetailsResponse(
                result.InvitationId,
                result.InviterEmployeeId,
                result.InviteeEmployeeId,
                result.TargetType,
                result.TargetId,
                (InvitationStatusDto)result.Status,
                result.ExpiresAt,
                result.CreatedAt
            );

            return Ok(response);
        }
    }
}
