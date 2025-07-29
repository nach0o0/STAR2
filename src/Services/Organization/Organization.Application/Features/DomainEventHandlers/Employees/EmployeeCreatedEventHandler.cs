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
    public class EmployeeCreatedEventHandler : INotificationHandler<EmployeeCreatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EmployeeCreatedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Handle(EmployeeCreatedEvent notification, CancellationToken cancellationToken)
        {
            var employee = notification.Employee;
            var integrationEvent = new EmployeeCreatedIntegrationEvent
            {
                EmployeeId = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName
            };

            return _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
