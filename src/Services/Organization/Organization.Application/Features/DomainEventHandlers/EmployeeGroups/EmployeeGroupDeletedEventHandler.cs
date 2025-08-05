using MassTransit;
using MediatR;
using Organization.Application.Features.Commands.RemoveEmployeeFromGroup;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Events.EmployeeGroups;
using Shared.Messages.Events.OrganizationService;

namespace Organization.Application.Features.DomainEventHandlers.EmployeeGroups
{
    public class EmployeeGroupDeletedEventHandler : INotificationHandler<EmployeeGroupDeletedEvent>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISender _sender;

        public EmployeeGroupDeletedEventHandler(
            IEmployeeRepository employeeRepository,
            IPublishEndpoint publishEndpoint,
            ISender sender)
        {
            _employeeRepository = employeeRepository;
            _publishEndpoint = publishEndpoint;
            _sender = sender;
        }

        public async Task Handle(EmployeeGroupDeletedEvent notification, CancellationToken cancellationToken)
        {
            var deletedGroup = notification.EmployeeGroup;

            // 1. Finde alle Mitarbeiter, die Mitglied dieser Gruppe waren.
            var members = await _employeeRepository.GetEmployeesByGroupIdAsync(deletedGroup.Id, cancellationToken);

            // 2. Entferne die Zuweisung bei jedem Mitarbeiter.
            foreach (var member in members)
            {
                await _sender.Send(new RemoveEmployeeFromGroupCommand(member.Id, deletedGroup.Id), cancellationToken);
            }

            // 3. Veröffentliche das Integration Event.
            var integrationEvent = new EmployeeGroupDeletedIntegrationEvent { EmployeeGroupId = deletedGroup.Id };
            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
