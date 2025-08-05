using MassTransit;
using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Events.Employees;
using Shared.Messages.Events.OrganizationService;

namespace Organization.Application.Features.DomainEventHandlers.Employees
{
    public class EmployeeUnassignedFromGroupEventHandler : INotificationHandler<EmployeeUnassignedFromGroupEvent>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public EmployeeUnassignedFromGroupEventHandler(IEmployeeRepository employeeRepository, IPublishEndpoint publishEndpoint)
        {
            _employeeRepository = employeeRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(EmployeeUnassignedFromGroupEvent notification, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdWithGroupsAsync(notification.EmployeeId, cancellationToken);
            if (employee is null || !employee.UserId.HasValue) return;

            var integrationEvent = new EmployeeEmployeeGroupAssignmentChangedIntegrationEvent
            {
                UserId = employee.UserId.Value,
                EmployeeId = employee.Id,
                EmployeeGroupIds = employee.EmployeeGroupLinks.Select(l => l.EmployeeGroupId).ToList()
            };

            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
