using MediatR;
using Organization.Application.Features.Commands.AssignHourlyRateToEmployee;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Events.EmployeeGroups;

namespace Organization.Application.Features.DomainEventHandlers.EmployeeGroups
{
    public class EmployeeGroupTransferredEventHandler : INotificationHandler<EmployeeGroupTransferredEvent>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISender _sender;

        public EmployeeGroupTransferredEventHandler(
            IEmployeeRepository employeeRepository,
            ISender sender)
        {
            _employeeRepository = employeeRepository;
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
        }
    }
}
