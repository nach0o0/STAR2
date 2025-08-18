using MediatR;
using Organization.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetMySentInvitations
{
    public class GetMySentInvitationsQueryHandler
        : IRequestHandler<GetMySentInvitationsQuery, List<GetMySentInvitationsQueryResult>>
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUserContext _userContext;

        public GetMySentInvitationsQueryHandler(IInvitationRepository invitationRepository, IUserContext userContext)
        {
            _invitationRepository = invitationRepository;
            _userContext = userContext;
        }

        public async Task<List<GetMySentInvitationsQueryResult>> Handle(
            GetMySentInvitationsQuery request,
            CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser()!;
            var employeeId = currentUser.EmployeeId!.Value;

            var invitations = await _invitationRepository.GetSentByInviterAsync(employeeId, cancellationToken);

            return invitations
                .Select(i => new GetMySentInvitationsQueryResult(
                    i.Id,
                    i.InviteeEmployeeId,
                    i.TargetEntityType,
                    i.TargetEntityId,
                    i.Status,
                    i.ExpiresAt))
                .ToList();
        }
    }
}
