using MassTransit;
using MediatR;
using Organization.Application.Features.Commands.AssignHourlyRateToEmployee;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Events.HourlyRates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.DomainEventHandlers.HourlyRates
{
    public class HourlyRateDeletedEventHandler : INotificationHandler<HourlyRateDeletedEvent>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISender _sender;

        public HourlyRateDeletedEventHandler(IEmployeeRepository employeeRepository, ISender sender)
        {
            _employeeRepository = employeeRepository;
            _sender = sender;
        }

        public async Task Handle(HourlyRateDeletedEvent notification, CancellationToken cancellationToken)
        {
            var deletedRateId = notification.HourlyRate.Id;

            // 1. Finde alle Mitarbeiter, denen dieser Stundensatz irgendwo zugewiesen ist.
            var affectedEmployees = await _employeeRepository.GetEmployeesByHourlyRateIdAsync(deletedRateId, cancellationToken);

            // 2. Entferne die Zuweisung bei jedem betroffenen Mitarbeiter.
            foreach (var employee in affectedEmployees)
            {
                var linksToUpdate = employee.EmployeeGroupLinks.Where(l => l.HourlyRateId == deletedRateId);
                foreach (var link in linksToUpdate)
                {
                    await _sender.Send(
                        new AssignHourlyRateToEmployeeCommand(employee.Id, link.EmployeeGroupId, null),
                        cancellationToken);
                }
            }
        }
    }
}
