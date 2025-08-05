using MediatR;
using Organization.Domain.Events.Employees;

namespace Organization.Application.Features.DomainEventHandlers.Employees
{
    public class EmployeeDetailsUpdatedEventHandler : INotificationHandler<EmployeeDetailsUpdatedEvent>
    {
        public Task Handle(EmployeeDetailsUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
