using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Events.Invitations;

namespace Organization.Application.Features.DomainEventHandlers.Invitations
{
    public class InvitationRevokedEventHandler : INotificationHandler<InvitationRevokedEvent>
    {
        private readonly IInvitationRepository _invitationRepository;

        public InvitationRevokedEventHandler(IInvitationRepository invitationRepository)
        {
            _invitationRepository = invitationRepository;
        }

        public Task Handle(InvitationRevokedEvent notification, CancellationToken cancellationToken)
        {
            var invitation = notification.Invitation;

            // Führe die Lösch-Operation durch
            _invitationRepository.Delete(invitation);

            return Task.CompletedTask;
        }
    }
}
