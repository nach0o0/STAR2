using MassTransit;
using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Events.Employees;
using Shared.Messages.Events.OrganizationService.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.DomainEventHandlers.Employees
{
    public class EmployeeAssignedToGroupEventHandler : INotificationHandler<EmployeeAssignedToGroupEvent>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public EmployeeAssignedToGroupEventHandler(IEmployeeRepository employeeRepository, IPublishEndpoint publishEndpoint)
        {
            _employeeRepository = employeeRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(EmployeeAssignedToGroupEvent notification, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(notification.EmployeeId, cancellationToken);
            if (employee?.UserId is null) return;

            var integrationEvent = new EmployeeAssignedToGroupIntegrationEvent
            {
                EmployeeId = notification.EmployeeId,
                EmployeeGroupId = notification.EmployeeGroupId,
                UserId = employee.UserId.Value
            };

            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
