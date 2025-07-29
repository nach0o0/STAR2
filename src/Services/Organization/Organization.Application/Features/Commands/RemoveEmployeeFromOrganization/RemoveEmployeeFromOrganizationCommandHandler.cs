using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.RemoveEmployeeFromOrganization
{
    public class RemoveEmployeeFromOrganizationCommandHandler : IRequestHandler<RemoveEmployeeFromOrganizationCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public RemoveEmployeeFromOrganizationCommandHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task Handle(RemoveEmployeeFromOrganizationCommand command, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(command.EmployeeId, cancellationToken);
            if (employee is null)
            {
                throw new NotFoundException(nameof(Employee), command.EmployeeId);
            }

            employee.UnassignFromOrganization();

            // Die UnitOfWork-Pipeline speichert die Änderung am Employee und veröffentlicht das Event.
            // Der EventHandler wird dann automatisch aufgerufen und speichert die weiteren Änderungen.
        }
    }
}
