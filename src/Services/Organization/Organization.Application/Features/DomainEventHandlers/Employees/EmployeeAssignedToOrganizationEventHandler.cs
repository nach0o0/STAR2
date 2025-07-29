using MassTransit;
using MediatR;
using Organization.Application.Features.Commands.AddEmployeeToGroup;
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
    public class EmployeeAssignedToOrganizationEventHandler : INotificationHandler<EmployeeAssignedToOrganizationEvent>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISender _sender;

        public EmployeeAssignedToOrganizationEventHandler(
            IEmployeeRepository employeeRepository,
            IOrganizationRepository organizationRepository,
            IPublishEndpoint publishEndpoint,
            ISender sender)
        {
            _employeeRepository = employeeRepository;
            _organizationRepository = organizationRepository;
            _publishEndpoint = publishEndpoint;
            _sender = sender;
        }

        public async Task Handle(EmployeeAssignedToOrganizationEvent notification, CancellationToken cancellationToken)
        {
            // 1. Lade die notwendigen Entitäten
            var employee = await _employeeRepository.GetByIdAsync(notification.EmployeeId, cancellationToken);
            var organization = await _organizationRepository.GetByIdAsync(notification.OrganizationId, cancellationToken);

            if (employee is null || organization is null || !organization.DefaultEmployeeGroupId.HasValue)
            {
                return;
            }

            // 2. Sende einen Command, um den Mitarbeiter zur Standardgruppe hinzuzufügen.
            //    Dieser Command hat seine eigene Autorisierung (die hier übersprungen wird) und Logik.
            var addEmployeeToGroupCommand = new AddEmployeeToGroupCommand(
                employee.Id,
                organization.DefaultEmployeeGroupId.Value);

            await _sender.Send(addEmployeeToGroupCommand, cancellationToken);

            // 3. Veröffentliche das Integration Event, um über die Zuweisung zur Organisation zu informieren.
            if (employee.UserId.HasValue)
            {
                var integrationEvent = new EmployeeAssignedToOrganizationIntegrationEvent
                {
                    EmployeeId = notification.EmployeeId,
                    OrganizationId = notification.OrganizationId,
                    UserId = employee.UserId.Value
                };
                await _publishEndpoint.Publish(integrationEvent, cancellationToken);
            }
        }
    }
}
