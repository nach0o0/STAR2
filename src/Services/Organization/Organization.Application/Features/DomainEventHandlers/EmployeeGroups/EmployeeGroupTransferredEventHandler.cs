using MassTransit;
using MediatR;
using Organization.Application.Features.Commands.AssignHourlyRateToEmployee;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Events.EmployeeGroups;
using Shared.Messages.Events.OrganizationService.EmployeeGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.DomainEventHandlers.EmployeeGroups
{
    public class EmployeeGroupTransferredEventHandler : INotificationHandler<EmployeeGroupTransferredEvent>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISender _sender;

        public EmployeeGroupTransferredEventHandler(
            IEmployeeRepository employeeRepository,
            IPublishEndpoint publishEndpoint,
            ISender sender)
        {
            _employeeRepository = employeeRepository;
            _publishEndpoint = publishEndpoint;
            _sender = sender;
        }

        public async Task Handle(EmployeeGroupTransferredEvent notification, CancellationToken cancellationToken)
        {
            // 1. Finde alle Mitglieder der transferierten Gruppe.
            var members = await _employeeRepository.GetEmployeesByGroupIdAsync(notification.EmployeeGroupId, cancellationToken);

            // 2. Sende für jedes Mitglied einen Command, um die Stundensatz-Zuweisung zu entfernen (auf null zu setzen).
            foreach (var member in members)
            {
                var command = new AssignHourlyRateToEmployeeCommand(member.Id, notification.EmployeeGroupId, null);
                await _sender.Send(command, cancellationToken);
            }

            // 3. Veröffentliche das Integration Event.
            var integrationEvent = new EmployeeGroupTransferredIntegrationEvent
            {
                EmployeeGroupId = notification.EmployeeGroupId,
                SourceOrganizationId = notification.SourceOrganizationId,
                DestinationOrganizationId = notification.DestinationOrganizationId
            };

            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
