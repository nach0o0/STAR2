using MassTransit;
using MediatR;
using Organization.Application.Features.Commands.RemoveEmployeeFromGroup;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Events.Employees;
using Shared.Messages.Events.OrganizationService;

namespace Organization.Application.Features.DomainEventHandlers.Employees
{
    public class EmployeeUnassignedFromOrganizationEventHandler : INotificationHandler<EmployeeUnassignedFromOrganizationEvent>
    {
        private readonly IEmployeeGroupRepository _employeeGroupRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISender _sender;

        public EmployeeUnassignedFromOrganizationEventHandler(
            IEmployeeGroupRepository employeeGroupRepository,
            IPublishEndpoint publishEndpoint,
            ISender sender)
        {
            _employeeGroupRepository = employeeGroupRepository;
            _publishEndpoint = publishEndpoint;
            _sender = sender;
        }

        public async Task Handle(EmployeeUnassignedFromOrganizationEvent notification, CancellationToken cancellationToken)
        {
            var employee = notification.Employee;
            if (employee.UserId is null) return;

            var previousOrganizationId = notification.PreviousOrganizationId;

            // Finde alle Gruppen der früheren Organisation.
            var groupsInOrganization = await _employeeGroupRepository.GetByLeadingOrganizationIdAsync(previousOrganizationId, cancellationToken);
            var groupIdsInOrganization = groupsInOrganization.Select(g => g.Id);

            // Finde die Links des Mitarbeiters zu diesen Gruppen.
            var linksToRemove = employee.EmployeeGroupLinks
                .Where(l => groupIdsInOrganization.Contains(l.EmployeeGroupId))
                .ToList();

            // Sende für jeden Link einen Command, um den Mitarbeiter aus der Gruppe zu entfernen.
            foreach (var link in linksToRemove)
            {
                await _sender.Send(new RemoveEmployeeFromGroupCommand(employee.Id, link.EmployeeGroupId), cancellationToken);
            }

            // Veröffentliche das Integration Event.
            var integrationEvent = new EmployeeOrganizationAssignmentChangedIntegrationEvent
            {
                UserId = employee.UserId.Value,
                EmployeeId = employee.Id,
                OrganizationId = null
            };
            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
