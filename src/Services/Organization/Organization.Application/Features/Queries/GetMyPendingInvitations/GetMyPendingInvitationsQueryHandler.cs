using MediatR;
using Organization.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetMyPendingInvitations
{
    public class GetMyPendingInvitationsQueryHandler
        : IRequestHandler<GetMyPendingInvitationsQuery, List<GetMyPendingInvitationsQueryResult>>
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUserContext _userContext;

        public GetMyPendingInvitationsQueryHandler(IInvitationRepository invitationRepository, IUserContext userContext)
        {
            _invitationRepository = invitationRepository;
            _userContext = userContext;
        }

        public async Task<List<GetMyPendingInvitationsQueryResult>> Handle(
            GetMyPendingInvitationsQuery request,
            CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser()!;

            // Der Authorizer stellt sicher, dass EmployeeId vorhanden ist.
            var employeeId = currentUser.EmployeeId!.Value;

            var invitations = await _invitationRepository.GetForInviteeAsync(employeeId, cancellationToken);

            return invitations
                .Select(i => new GetMyPendingInvitationsQueryResult(
                    i.Id,
                    i.InviterEmployeeId,
                    i.TargetEntityType,
                    i.TargetEntityId,
                    i.ExpiresAt))
                .ToList();
        }
    }
}
