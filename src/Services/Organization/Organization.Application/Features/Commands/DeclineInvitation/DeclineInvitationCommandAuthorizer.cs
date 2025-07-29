using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.DeclineInvitation
{
    public class DeclineInvitationCommandAuthorizer : ICommandAuthorizer<DeclineInvitationCommand>
    {
        private readonly IInvitationRepository _invitationRepository;

        public DeclineInvitationCommandAuthorizer(IInvitationRepository invitationRepository)
        {
            _invitationRepository = invitationRepository;
        }

        public async Task AuthorizeAsync(DeclineInvitationCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var invitation = await _invitationRepository.GetByIdAsync(command.InvitationId, cancellationToken);

            if (invitation is null)
            {
                throw new NotFoundException(nameof(Invitation), command.InvitationId);
            }

            if (invitation.InviteeEmployeeId != currentUser.EmployeeId)
            {
                throw new ForbiddenAccessException("You are not authorized to decline this invitation.");
            }
        }
    }
}
