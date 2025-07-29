using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.AcceptInvitation
{
    public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand>
    {
        private readonly IInvitationRepository _invitationRepository;

        public AcceptInvitationCommandHandler(IInvitationRepository invitationRepository)
        {
            _invitationRepository = invitationRepository;
        }

        public async Task Handle(AcceptInvitationCommand command, CancellationToken cancellationToken)
        {
            // Der Authorizer hat bereits sichergestellt, dass die Einladung existiert
            // und der Benutzer berechtigt ist. Wir können sie also direkt laden.
            var invitation = await _invitationRepository.GetByIdAsync(command.InvitationId, cancellationToken);

            // Sollte nie passieren, wenn der Authorizer korrekt arbeitet, aber als Sicherheitsnetz.
            if (invitation is null)
            {
                throw new NotFoundException(nameof(Invitation), command.InvitationId);
            }

            // Ruft die Methode auf der Entität auf. Diese löst das 'InvitationAcceptedEvent' aus.
            invitation.Accept();

            // Die UnitOfWork-Pipeline speichert die Statusänderung der Einladung
            // und veröffentlicht das Domain Event.
        }
    }
}
