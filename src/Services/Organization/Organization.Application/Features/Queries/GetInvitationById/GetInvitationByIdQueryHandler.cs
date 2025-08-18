using MediatR;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetInvitationById
{
    public class GetInvitationByIdQueryHandler
        : IRequestHandler<GetInvitationByIdQuery, GetInvitationByIdQueryResult?>
    {
        private readonly IInvitationRepository _invitationRepository;

        public GetInvitationByIdQueryHandler(IInvitationRepository invitationRepository)
        {
            _invitationRepository = invitationRepository;
        }

        public async Task<GetInvitationByIdQueryResult?> Handle(
            GetInvitationByIdQuery request,
            CancellationToken cancellationToken)
        {
            var invitation = await _invitationRepository.GetByIdAsync(request.InvitationId, cancellationToken);

            if (invitation is null)
            {
                return null;
            }

            return new GetInvitationByIdQueryResult(
                invitation.Id,
                invitation.InviterEmployeeId,
                invitation.InviteeEmployeeId,
                invitation.TargetEntityType,
                invitation.TargetEntityId,
                invitation.Status,
                invitation.ExpiresAt,
                invitation.CreatedAt
            );
        }
    }
}
