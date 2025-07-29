using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.RevokeInvitation
{
    public class RevokeInvitationCommandHandler : IRequestHandler<RevokeInvitationCommand>
    {
        private readonly IInvitationRepository _invitationRepository;

        public RevokeInvitationCommandHandler(IInvitationRepository invitationRepository)
        {
            _invitationRepository = invitationRepository;
        }

        public async Task Handle(RevokeInvitationCommand command, CancellationToken cancellationToken)
        {
            var invitation = await _invitationRepository.GetByIdAsync(command.InvitationId, cancellationToken);
            if (invitation is null)
            {
                throw new NotFoundException(nameof(Invitation), command.InvitationId);
            }

            invitation.Revoke();
        }
    }
}
