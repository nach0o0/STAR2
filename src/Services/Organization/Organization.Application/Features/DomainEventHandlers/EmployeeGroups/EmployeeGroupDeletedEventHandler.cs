using MassTransit;
using MediatR;
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
    public class EmployeeGroupDeletedEventHandler : INotificationHandler<EmployeeGroupDeletedEvent>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public EmployeeGroupDeletedEventHandler(IEmployeeRepository employeeRepository, IPublishEndpoint publishEndpoint)
        {
            _employeeRepository = employeeRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(EmployeeGroupDeletedEvent notification, CancellationToken cancellationToken)
        {
            var deletedGroup = notification.EmployeeGroup;

            // 1. Finde alle Mitarbeiter, die Mitglied dieser Gruppe waren.
            var members = await _employeeRepository.GetEmployeesByGroupIdAsync(deletedGroup.Id, cancellationToken);

            // 2. Entferne die Zuweisung bei jedem Mitarbeiter.
            foreach (var member in members)
            {
                member.RemoveFromGroup(deletedGroup.Id, null);
            }

            // 3. Veröffentliche das Integration Event.
            var integrationEvent = new EmployeeGroupDeletedIntegrationEvent { EmployeeGroupId = deletedGroup.Id };
            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
