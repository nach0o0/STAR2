using MediatR;
using Organization.Domain.Events.Invitations;

namespace Organization.Application.Features.DomainEventHandlers.Invitations
{
    public class InvitationCreatedEventHandler : INotificationHandler<InvitationCreatedEvent>
    {
        public Task Handle(InvitationCreatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
