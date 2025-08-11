using MassTransit;
using MediatR;
using Organization.Application.Features.Commands.AddEmployeeToGroup;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Events.EmployeeGroups;
using Shared.Application.Interfaces.Security;
using Shared.Messages.Events.OrganizationService;

namespace Organization.Application.Features.DomainEventHandlers.EmployeeGroups
{
    public class EmployeeGroupCreatedEventHandler : INotificationHandler<EmployeeGroupCreatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISender _sender;

        public EmployeeGroupCreatedEventHandler(
            IPublishEndpoint publishEndpoint,
            IEmployeeRepository employeeRepository,
            ISender sender)
        {
            _publishEndpoint = publishEndpoint;
            _employeeRepository = employeeRepository;
            _sender = sender;
        }

        public async Task Handle(EmployeeGroupCreatedEvent notification, CancellationToken cancellationToken)
        {
            var group = notification.EmployeeGroup;
            var creatorUserId = notification.CreatorUserId;

            // Finde den Mitarbeiter des Erstellers
            var creatorEmployee = await _employeeRepository.GetByUserIdAsync(creatorUserId, cancellationToken);
            if (creatorEmployee is not null && creatorEmployee.OrganizationId.HasValue)
            {
                // Sende den Command, um den Ersteller zur neuen Gruppe hinzuzufügen
                var command = new AddEmployeeToGroupCommand(creatorEmployee.Id, group.Id);
                await _sender.Send(command, cancellationToken);
            }

            // Veröffentliche das Integration Event
            var integrationEvent = new EmployeeGroupCreatedIntegrationEvent
            {
                EmployeeGroupId = group.Id,
                Name = group.Name,
                LeadingOrganizationId = group.LeadingOrganizationId,
                CreatorUserId = creatorUserId
            };

            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
