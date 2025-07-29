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

namespace Organization.Application.Features.Commands.AddEmployeeToOrganization
{
    public class AddEmployeeToOrganizationCommandHandler : IRequestHandler<AddEmployeeToOrganizationCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public AddEmployeeToOrganizationCommandHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task Handle(AddEmployeeToOrganizationCommand command, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(command.EmployeeId, cancellationToken);
            if (employee is null)
            {
                throw new NotFoundException(nameof(Employee), command.EmployeeId);
            }

            if (employee.OrganizationId.HasValue)
            {
                throw new ValidationException(new Dictionary<string, string[]>
            {
                { "EmployeeId", new[] { "Employee is already assigned to an organization." } }
            });
            }

            employee.AssignToOrganization(command.OrganizationId);
        }
    }
}
