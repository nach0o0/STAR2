using MassTransit;
using MediatR;
using Organization.Domain.Events.Employees;
using Shared.Messages.Events.OrganizationService.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.DomainEventHandlers.Employees
{
    public class UserAssignedToEmployeeEventHandler : INotificationHandler<UserAssignedToEmployeeEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public UserAssignedToEmployeeEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Handle(UserAssignedToEmployeeEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new UserAssignedToEmployeeIntegrationEvent
            {
                EmployeeId = notification.EmployeeId,
                UserId = notification.UserId
            };

            return _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
