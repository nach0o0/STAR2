using MediatR;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetInvitationsByTargetEntity
{
    public class GetInvitationsByTargetEntityQueryHandler
        : IRequestHandler<GetInvitationsByTargetEntityQuery, List<GetInvitationsByTargetEntityQueryResult>>
    {
        private readonly IInvitationRepository _invitationRepository;

        public GetInvitationsByTargetEntityQueryHandler(IInvitationRepository invitationRepository)
        {
            _invitationRepository = invitationRepository;
        }

        public async Task<List<GetInvitationsByTargetEntityQueryResult>> Handle(
            GetInvitationsByTargetEntityQuery request,
            CancellationToken cancellationToken)
        {
            var invitations = await _invitationRepository.GetByTargetEntityAsync(
                request.TargetType,
                request.TargetId,
                cancellationToken);

            return invitations
                .Select(i => new GetInvitationsByTargetEntityQueryResult(
                    i.Id,
                    i.InviteeEmployeeId,
                    i.ExpiresAt))
                .ToList();
        }
    }
}
