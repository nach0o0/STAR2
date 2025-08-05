using MassTransit;
using MediatR;
using Organization.Domain.Events.Employees;
using Shared.Messages.Events.OrganizationService;

namespace Organization.Application.Features.DomainEventHandlers.Employees
{
    public class UserUnassignedFromEmployeeEventHandler : INotificationHandler<UserUnassignedFromEmployeeEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public UserUnassignedFromEmployeeEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Handle(UserUnassignedFromEmployeeEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new UserUnassignedFromEmployeeIntegrationEvent
            {
                EmployeeId = notification.EmployeeId,
                UserId = notification.UserId
            };

            return _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
