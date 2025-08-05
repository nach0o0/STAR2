using MediatR;
using Organization.Domain.Events.Invitations;

namespace Organization.Application.Features.DomainEventHandlers.Invitations
{
    public class InvitationDeclinedEventHandler : INotificationHandler<InvitationDeclinedEvent>
    {
        public Task Handle(InvitationDeclinedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
