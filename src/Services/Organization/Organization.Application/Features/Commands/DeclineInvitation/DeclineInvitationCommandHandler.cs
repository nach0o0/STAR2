using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.DeclineInvitation
{
    public class DeclineInvitationCommandHandler : IRequestHandler<DeclineInvitationCommand>
    {
        private readonly IInvitationRepository _invitationRepository;

        public DeclineInvitationCommandHandler(IInvitationRepository invitationRepository)
        {
            _invitationRepository = invitationRepository;
        }

        public async Task Handle(DeclineInvitationCommand command, CancellationToken cancellationToken)
        {
            var invitation = await _invitationRepository.GetByIdAsync(command.InvitationId, cancellationToken);

            if (invitation is null)
            {
                throw new NotFoundException(nameof(Invitation), command.InvitationId);
            }

            // Ruft die Methode auf der Entität auf, um den Status zu ändern.
            invitation.Decline();

            // Die UnitOfWork-Pipeline speichert die Statusänderung.
        }
    }
}
