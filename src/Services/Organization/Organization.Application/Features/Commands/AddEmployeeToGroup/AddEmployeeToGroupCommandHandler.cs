using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using Shared.Application.Exceptions;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.AddEmployeeToGroup
{
    public class AddEmployeeToGroupCommandHandler : IRequestHandler<AddEmployeeToGroupCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeGroupRepository _employeeGroupRepository;

        public AddEmployeeToGroupCommandHandler(
            IEmployeeRepository employeeRepository,
            IEmployeeGroupRepository employeeGroupRepository)
        {
            _employeeRepository = employeeRepository;
            _employeeGroupRepository = employeeGroupRepository;
        }

        public async Task Handle(AddEmployeeToGroupCommand command, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(command.EmployeeId, cancellationToken);
            if (employee is null || !employee.OrganizationId.HasValue)
            {
                throw new NotFoundException(nameof(Employee), command.EmployeeId);
            }

            var employeeGroup = await _employeeGroupRepository.GetByIdAsync(command.EmployeeGroupId, cancellationToken);
            if (employeeGroup is null)
            {
                throw new NotFoundException(nameof(EmployeeGroup), command.EmployeeGroupId);
            }

            // --- ENTSCHEIDENDE GESCHÄFTSREGEL ---
            // Stellt sicher, dass die Organisation des Mitarbeiters mit der führenden
            // Organisation der Gruppe übereinstimmt.
            if (employee.OrganizationId.Value != employeeGroup.LeadingOrganizationId)
            {
                throw new ValidationException(new Dictionary<string, string[]>
            {
                { "EmployeeId", new[] { "The employee and the employee group do not belong to the same organization. Send an invitation instead." } }
            });
            }
            // ------------------------------------

            // Annahme: Es wird hier kein spezifischer Stundensatz zugewiesen.
            employee.AssignToGroup(command.EmployeeGroupId, null);
        }
    }
}
