using MediatR;
using Organization.Domain.Events.EmployeeGroups;

namespace Organization.Application.Features.DomainEventHandlers.EmployeeGroups
{
    public class EmployeeGroupUpdatedEventHandler : INotificationHandler<EmployeeGroupUpdatedEvent>
    {
        public EmployeeGroupUpdatedEventHandler()
        {
        }

        public Task Handle(EmployeeGroupUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
