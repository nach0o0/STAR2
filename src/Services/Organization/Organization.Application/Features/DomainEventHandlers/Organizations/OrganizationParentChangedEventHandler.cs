using MediatR;
using Organization.Domain.Events.Organizations;

namespace Organization.Application.Features.DomainEventHandlers.Organizations
{
    public class OrganizationParentChangedEventHandler : INotificationHandler<OrganizationParentChangedEvent>
    {
        public Task Handle(OrganizationParentChangedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
