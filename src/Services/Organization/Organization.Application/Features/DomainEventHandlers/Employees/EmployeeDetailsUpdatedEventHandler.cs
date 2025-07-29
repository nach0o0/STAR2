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
    public class EmployeeDetailsUpdatedEventHandler : INotificationHandler<EmployeeDetailsUpdatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EmployeeDetailsUpdatedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Handle(EmployeeDetailsUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var employee = notification.Employee;
            var integrationEvent = new EmployeeUpdatedIntegrationEvent
            {
                EmployeeId = employee.Id,
                NewFirstName = employee.FirstName,
                NewLastName = employee.LastName
            };

            return _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
