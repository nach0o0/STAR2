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

namespace Organization.Application.Features.Commands.AssignHourlyRateToEmployee
{
    public class AssignHourlyRateToEmployeeCommandHandler : IRequestHandler<AssignHourlyRateToEmployeeCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHourlyRateRepository _hourlyRateRepository;

        public AssignHourlyRateToEmployeeCommandHandler(
            IEmployeeRepository employeeRepository,
            IHourlyRateRepository hourlyRateRepository)
        {
            _employeeRepository = employeeRepository;
            _hourlyRateRepository = hourlyRateRepository;
        }

        public async Task Handle(AssignHourlyRateToEmployeeCommand command, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdWithGroupsAsync(command.EmployeeId, cancellationToken);
            if (employee is null || !employee.OrganizationId.HasValue)
            {
                throw new NotFoundException(nameof(Employee), command.EmployeeId);
            }

            // --- VALIDIERUNG DER GESCHÄFTSREGEL ---
            if (command.HourlyRateId.HasValue)
            {
                var hourlyRate = await _hourlyRateRepository.GetByIdAsync(command.HourlyRateId.Value, cancellationToken);
                if (hourlyRate is null)
                {
                    throw new NotFoundException(nameof(HourlyRate), command.HourlyRateId.Value);
                }

                // Prüft, ob der Stundensatz zur selben Organisation wie der Mitarbeiter gehört.
                if (hourlyRate.OrganizationId != employee.OrganizationId.Value)
                {
                    throw new ValidationException(new Dictionary<string, string[]>
                {
                    { "HourlyRateId", new[] { "The specified hourly rate does not belong to the employee's organization." } }
                });
                }
            }
            // ------------------------------------

            employee.UpdateHourlyRateForGroup(command.EmployeeGroupId, command.HourlyRateId);
        }
    }
}
